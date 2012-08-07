using System;
using System.Collections.Generic;
using System.Linq;
using Cardio.UI.Components;
using Cardio.UI.Core;
using Cardio.UI.Core.Alive;
using Cardio.UI.Exceptions;
using Cardio.UI.Input.Touch;
using Cardio.UI.Scenes.Actions;
using Cardio.UI.Scenes.Scripts;
using Cardio.UI.Screens;
using Cardio.UI.Scripts;
using Cardio.UI.Tutorial;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.Scenes
{
    public sealed class Level
    {
        private readonly LevelScreen _host;
        private Game _game;

        ///// <summary>
        ///// Registered scenes for this level.
        ///// </summary>
        //public IList<Scene> Scenes
        //{
        //    get; set;
        //}

        private SpriteBatch _spriteBatch;

        private List<IObstacle> _stops = new List<IObstacle>();

        public IList<DrawableGameObject> CustomLevelObjects { get; private set; }

        public IList<LevelText> Texts { get; private set; }

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

        public LevelFinished LevelFinishedComponent { get; set; }

        /// <summary>
        /// Collection of reactions for this level.
        /// </summary>
        public IDictionary<PlayerAction, Action<GameState>> Reactions { get; private set; }

        /// <summary>
        /// Is invoked when all the scenes are finished.
        /// </summary>
        public event EventHandler Finished;

        public Level()
        {
            CustomLevelObjects = new List<DrawableGameObject>();
            //Scenes = new List<Scene>();
            Texts = new List<LevelText>();
        }

        //public void AddScene(Scene scene)
        //{
        //    if (IsInitialized)
        //    {
        //        throw new UnableToModifyStateAfterInitializationException();
        //    }
            
        //    Scenes.Add(scene);
        //}

        public void AddLevelObject(DrawableGameObject value)
        {
            CustomLevelObjects.Add(value);
        }

        public bool RemoveLevelObject(DrawableGameObject value)
        {
            return CustomLevelObjects.Remove(value);
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

        public void Initialize(Game game, SpriteBatch spriteBatch, ICamera2D camera)
        {
            if (IsInitialized)
            {
                throw new InvalidOperationException("This level has already been initialized.");
            }

            _game = game;
            _spriteBatch = spriteBatch;

            InitializeReactions();
            
            LevelFinishedComponent = new LevelFinished(_game);
            LevelFinishedComponent.Initialize();

            // We also initialize all predefined level objects that were specified before Initialize call.
            // Note that this behavior may change in the future.
            for (int index = 0; index < CustomLevelObjects.Count; index++)
            {
                var levelObject = CustomLevelObjects[index];
                levelObject.Initialize(game, spriteBatch, camera);
            }

            for (int index = 0; index < Texts.Count; index++)
            {
                var text = Texts[index];
                text.Initialize();
            }

            IsInitialized = true;
        }

        public void Update(GameTime gameTime, GameState state)
        {
            EnsureIsInitialized();

            UpdateScripts(gameTime, state);

            UpdateCustomObjects(state, gameTime);

            for (int index = 0; index < Texts.Count; index++)
            {
                var text = Texts[index];
                text.Update(gameTime);
            }

            if (state.Player.WorldPosition.X >= Distance && !IsFinished)
            {
                OnFinished();
            }

            if (IsFinished)
            {
                LevelFinishedComponent.Update(gameTime);
            }
        }

        private void InitializeReactions()
        {
            Action<GameState, Int32> useSlot = (state, slotIndex) =>
            {
                if (state.Inventory.Slots.Count <= slotIndex)
                {
                    state.Player.Confuse();
                }

                state.Inventory.UseItemFromSlot(slotIndex, state);
            };

            Reactions = new Dictionary<PlayerAction, Action<GameState>>();
            Reactions.Add(PlayerAction.Move, StartMoving);
            Reactions.Add(PlayerAction.UseItem01, state => useSlot(state, 0));
            Reactions.Add(PlayerAction.UseItem02, state => useSlot(state, 1));
            Reactions.Add(PlayerAction.UseItem03, state => useSlot(state, 2));
            Reactions.Add(PlayerAction.UseItem04, state => useSlot(state, 3));
            Reactions.Add(PlayerAction.Shoot, Shoot);
        }

        private void Shoot(GameState state)
        {
            var enemiesInRange =
                CustomLevelObjects.Where(obj => obj is IPlayerAttackTarget).Cast<IPlayerAttackTarget>().Where(
                    enemy => enemy.WorldPosition.X <= state.Player.WorldPosition.X + state.Player.AttackRange);
            if (enemiesInRange.Any())
            {
                state.Player.AddScript(new PlayerShootScript(enemiesInRange));
            }
            else
            {
                state.Player.Confuse();
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
                CustomLevelObjects[i].Update(gameState, gameTime);
            }
            

        }

        public void HandleInput(GameState state, IList<ButtonTouch> sequence)
        {
            EnsureIsInitialized();

            if (IsFinished || state.IsHandlingSequence)
            {
                return;
            }

            var action = ActionCatalog.CheckSequence(sequence.Select(x => x.Button).ToList());
            Action<GameState> handler;
            if (action.HasValue && Reactions.TryGetValue(action.Value, out handler))
            {
                handler(state);
                state.Combo.IncreaseComboLevel();
            }

            //bool actionInvoked = (action != null);
            //if (CurrentScene != null)
            //{
            //    actionInvoked = CurrentScene.ProcessTouchSequence(state, sequence.Select(x => x.Button).ToList()) ||
            //                    actionInvoked;
            //}

            //if (actionInvoked)
            //{
                
            //}
        }

        public void Draw(GameTime gameTime, GameState state)
        {
            EnsureIsInitialized();

            //for (int index = 0; index < Scenes.Count; index++)
            //{
            //    var scene = Scenes[index];
            //    if (IsSceneVisible(state, scene))
            //    {
            //        scene.Draw(gameTime);
            //    }
            //}

            for (int index = 0; index < CustomLevelObjects.Count; index++)
            {
                var target = CustomLevelObjects[index];
                target.Draw(gameTime);
            }

            for (int index = 0; index < Texts.Count; index++)
            {
                var text = Texts[index];
                text.Draw(gameTime);
            }

            if (IsFinished)
            {
                LevelFinishedComponent.Draw(gameTime);
            }
        }

        private void OnFinished()
        {
            IsFinished = true;

            var script = new LevelFinishedScript();
            script.Stopped += (s, e) =>
            {
                var handler = Finished;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            };

            _game.Services.GetService<GameState>().AddScript(script);
        }

        private static void StartMoving(GameState state)
        {
            if (!state.Player.IsMoving)
            {
                state.AddScript(new MovePlayerScript {Distance = 800});
            }
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
