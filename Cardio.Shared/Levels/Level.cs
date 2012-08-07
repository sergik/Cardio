using System;
using System.Collections.Generic;
using System.Linq;
using Cardio.UI.Actions;
using Cardio.UI.Backgrounds;
using Cardio.UI.Characters;
using Cardio.UI.Characters.Bosses;
using Cardio.UI.Characters.Ranged;
using Cardio.UI.Components;
using Cardio.UI.Core;
using Cardio.UI.Core.Alive;
using Cardio.UI.Exceptions;
using Cardio.UI.Extensions;
using Cardio.UI.Input.Touch;
using Cardio.UI.Inventory;
using Cardio.UI.Rhythm;
using Cardio.UI.Scripts;
using Cardio.UI.Sounds;
using Cardio.UI.Tutorial;
using Cardio.UI.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.Levels
{
    public sealed class Level
    {
        private Game _game;

        private ResourceBuilder _resourceBuilder;

        private ReactionsCatalog _reactionCatalog;

        public BackgroundController Background
        {
            get; private set;
        }

        private readonly List<IObstacle> _stops = new List<IObstacle>();

        public string MelodyAsset { get; private set; }

        public IList<DrawableGameObject> CustomLevelObjects { get; private set; }

        public IList<LevelText> Texts { get; private set; }

        public string Name { get; private set; }

        public float? NextStop { get; set; }

        /// <summary>
        /// True if the level was properly initialized.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Distance of the level, will be compared with X of players' WorldPositon
        /// </summary>
        public float Distance { get; set; }

        /// <summary>
        /// Current virtual level position.
        /// </summary>
        public float CurrentPosition { get; set; }

        /// <summary>
        /// True if all the scenes at this level are finished.
        /// </summary>
        public bool IsFinished { get; private set; }

        /// <summary>
        /// Is invoked when all the scenes are finished.
        /// </summary>
        public event EventHandler Finished;

        public Level()
        {
            CustomLevelObjects = new List<DrawableGameObject>();
            Texts = new List<LevelText>();
        }

        public void AddLevelObject(DrawableGameObject value)
        {
            CustomLevelObjects.Add(value);
        }

        public bool RemoveLevelObject(DrawableGameObject value)
        {
            return CustomLevelObjects.Remove(value);
        }

        public void AddEnemy(ObstacleGameObject enemy)
        {
            enemy.Die += OnEnemyDie;
            enemy.DamageTaken += OnEnemyDamage;

            AddLevelObject(enemy);
            if (enemy is RangeEnemy)
            {
                AddStop(enemy);
            }
        }

        public void AddBoss(Boss boss)
        {
            boss.Level = this;
            AddEnemy(boss);
        }

        public void AddObstacle(ObstacleGameObject obstacle)
        {
            obstacle.Die += OnEnemyDie;
            obstacle.DamageTaken += OnEnemyDamage;
            AddLevelObject(obstacle);
            AddStop(obstacle);
        }

        public void AddStop(IObstacle stop)
        {
            _stops.Add(stop);
            if (!NextStop.HasValue || NextStop.Value > (stop.WorldPosition.X - stop.StopDistance))
            {
                NextStop = stop.WorldPosition.X - stop.StopDistance;
            }
        }

        public void RemoveStop(IObstacle stop)
        {
            _stops.Remove(stop);
            if (_stops.Count > 0)
            {
                NextStop = _stops.Select(s => s.WorldPosition.X - s.StopDistance).Min();
            }
            else
            {
                NextStop = null;
            }
        }

        public void Initialize(Game game, SpriteBatch spriteBatch, ICamera2D camera, GameState gameState)
        {
            if (IsInitialized)
            {
                throw new InvalidOperationException("This level has already been initialized.");
            }

            _game = game;
            _resourceBuilder = _game.Services.GetService<ResourceBuilder>();
            _game.Services.GetService<RhythmEngine>();
            _reactionCatalog = _game.Services.GetService<ReactionsCatalog>();

            // let action will be inertioned for 2 beats
            gameState.ReactionProgress.ReactionInertia = 1200;

            Background.Initialize(game, spriteBatch, camera);

            // We also initialize all predefined level objects that were specified before Initialize call.
            // Note that this behavior may change in the future.
            for (int index = 0; index < CustomLevelObjects.Count; index++)
            {
                var levelObject = CustomLevelObjects[index];
                levelObject.Initialize(game, spriteBatch, camera);
            }

            foreach (var text in Texts)
            {
                text.Initialize(game);
            }

            var nanobots = gameState.Player.Nanobots;

            foreach (var bot in nanobots)
            {
                bot.DamageTaken += OnNanobotDamage;
            }

            IsInitialized = true;
        }

        public void Update(GameTime gameTime, GameState state)
        {
            EnsureIsInitialized();

            Background.Update(gameTime);

            UpdateScripts(gameTime, state);

            UpdateCustomObjects(state, gameTime);

            if (state.Player.WorldPosition.X >= Distance && !IsFinished)
            {
                OnFinished(state);
            }
        }

        public static Level FromMetadata(LevelMetadata metadata, ContentManager contentManager)
        {
            var backgroundMetadata = contentManager.Load<BackgroundMetadata>(metadata.Background);

            var level = new Level
            {
                Distance = metadata.Distance,
                Background = BackgroundController.FromMetadata(backgroundMetadata, contentManager),
                Name = metadata.Name,
                MelodyAsset = metadata.Melody
            };

            foreach (var enemy in metadata.Enemies)
            {
                var character = EnemyMapper.GetEnemy(enemy.EnemyType, contentManager);
                character.WorldPosition = enemy.Position;
                level.AddEnemy(character);
            }

            foreach (var obstacle in metadata.Obstacles)
            {
                var enemy = EnemyMapper.GetEnemy(obstacle.EnemyType, contentManager);
                enemy.WorldPosition = obstacle.Position;
                level.AddObstacle(enemy);
            }

            foreach (var bossMetadata in metadata.Bosses)
            {
                var boss = EnemyMapper.GetBoss(bossMetadata.EnemyType, contentManager);
                if (boss != null)
                {
                    boss.WorldPosition = bossMetadata.Position;
                    level.AddBoss(boss);
                }
            }

            foreach (var message in metadata.Messages)
            {
                level.Texts.Add(new LevelText(message.Text, message.Position));
            }

            return level;
        }

        private void OnEnemyDamage(IAlive sender, DamageTakenEventArgs e)
        {
            var damageAnimations = sender.ToDamageAnimation();

            foreach (var damageAnimation in damageAnimations)
            {
                damageAnimation.Finished += OnAnimationFinished;
                AddLevelObject(damageAnimation);
                damageAnimation.Initialize(_game, _game.Services.GetService<SpriteBatch>(), _game.Services.GetService<GameState>().Camera);
            }
        }

        private void OnNanobotDamage(IAlive sender, DamageTakenEventArgs e)
        {
            var damageAnimations = sender.ToDamageAnimation();

            foreach (var damageAnimation in damageAnimations)
            {
                damageAnimation.Finished += OnAnimationFinished;
                damageAnimation.WorldPosition = e.DamageTakenPoint;
                AddLevelObject(damageAnimation);
                damageAnimation.Initialize(_game, _game.Services.GetService<SpriteBatch>(), _game.Services.GetService<GameState>().Camera);
            }
        }

        private void OnEnemyDie(IAlive sender, DieEventArgs e)
        {
            var enemy = sender as ObstacleGameObject;

            if (enemy != null)
            {
                var deathAnimations = sender.ToDeathAnimation();

                foreach (var deathAnimation in deathAnimations)
                {
                    deathAnimation.Finished += OnAnimationFinished;

                    AddLevelObject(deathAnimation);
                    deathAnimation.Initialize(_game, _game.Services.GetService<SpriteBatch>(),
                        _game.Services.GetService<GameState>().Camera);
                }

                SoundManager.EnemyBlow.Play();

                var bio = _resourceBuilder.BuildBio(_game.Content, enemy.BiomaerialGeneratedMin, enemy.BiomaerialGeneratedMax);
                if (enemy.Size.HasValue && bio.Size.HasValue)
                {
                    bio.WorldPosition = enemy.WorldPosition + (enemy.Size.Value - bio.Size.Value)/2;
                }
                else
                {
                    bio.WorldPosition = enemy.WorldPosition;
                }
                bio.Initialize(_game, _game.Services.GetService<SpriteBatch>(),
                    _game.Services.GetService<GameState>().Camera);
                AddLevelObject(bio);
            }

            if (sender is DrawableGameObject)
            {
                RemoveLevelObject(sender as DrawableGameObject);
            }

            if (sender is IObstacle)
            {
                RemoveStop(sender as IObstacle);
            }
            sender.Die -= OnEnemyDie;

        }

        public void OnAnimationFinished(object sender, EventArgs e)
        {
            var target = sender as ParticleEffect;
            if (target != null)
            {
                target.Finished -= OnAnimationFinished;
                RemoveLevelObject(target);
            }
        }

        private static void UpdateScripts(GameTime gameTime, GameState state)
        {
            state.UpdateScripts(gameTime);
        }

        private void UpdateCustomObjects(GameState gameState, GameTime gameTime)
        {
#warning This is not a good design. But for now, objects can remove themselves during Update calls, so we have to stick with this option

            for (int i = CustomLevelObjects.Count - 1; i >= 0; i-- )
            {
                var target = CustomLevelObjects[i];
                var bounding = target.Content.BoundingRectangle;
                bounding.Offset((int) target.WorldPosition.X, (int) target.WorldPosition.Y);
                var rect = gameState.Camera.GetScreenRectangle(bounding);
                if (rect.Right >= 0 && rect.Left <= gameState.Camera.ViewportWidth)
                {
                    target.Update(gameState, gameTime);
                }
            }
        }

        public bool HandleInput(GameState state, IList<ButtonTouch> sequence)
        {
            EnsureIsInitialized();

            if (IsFinished)
            {
                return false;
            }

            var action = ActionCatalog.CheckSequence(sequence);

            return (action.HasValue && _reactionCatalog.ContainsReaction(action.Value) &&
                    _reactionCatalog.TryInvokeReaction(action.Value, state));
        }

        public void Draw(GameTime gameTime, GameState state)
        {
            EnsureIsInitialized();

            Background.Draw(gameTime);

            for (int index = 0; index < CustomLevelObjects.Count; index++)
            {
                var target = CustomLevelObjects[index];
                var bounding = target.Content.BoundingRectangle;
                bounding.Offset((int)target.WorldPosition.X, (int)target.WorldPosition.Y);
                var rect = state.Camera.GetScreenRectangle(bounding);
                if (rect.Right >= 0 && rect.Left <= state.Camera.ViewportWidth)
                {
                    target.Draw(gameTime);
                }
            }

            for (int index = 0; index < Texts.Count; index++)
            {
                var text = Texts[index];
                if (Math.Abs(text.WorldPosition.X - state.Camera.Position.X) * state.Camera.Scale <= state.Camera.ViewportWidth)
                {
                    text.Draw(gameTime);
                }
            }
        }

        private void OnFinished(GameState state)
        {
            IsFinished = true;

            var script = new LevelFinishedScript(_game);
            script.Stopped += (s, e) =>
            {
                var handler = Finished;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            };

            state.AddScript(script);
            state.GameStory.MarkLevelAsPassed(Name);
        }

        private void EnsureIsInitialized()
        {
            if (!IsInitialized)
            {
                throw new ComponentIsNotInitializedException();
            }
        }
    }
}
