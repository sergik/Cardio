using System;
using System.Collections.Generic;
using Cardio.UI.Components;
using Cardio.UI.Core.Alive;
using Cardio.UI.Extensions;
using Cardio.UI.Levels;
using Cardio.UI.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.Scripts
{
    public class GenerateEnemiesScript : GameEntityScript
    {
        public event EventHandler PreparingForGeneration;
        public event EventHandler Generated;

        private bool _preparingForGeneration;

        public Level Level
        {
            get; set;
        }

        public int GenerationStartDistance
        {
            get; set;
        }

        public List<String> EnemyTypes
        {
            get;
            set;
        }

        public int EnemiesToGenerateMin
        {
            get;
            set;
        }

        public int EnemiesToGenerateMax
        {
            get;
            set;
        }

        public TimeSpan GenerateEnemiesIntervalMin
        {
            get;
            set;
        }

        public TimeSpan GenerateEnemiesIntervalMax
        {
            get;
            set;
        }

        public TimeSpan IntervalForEnemiesGeneration
        {
            get;
            set;
        }

        public Game Game
        {
            get; set;
        }

        public SpriteBatch SpriteBatch
        {
            get; set;
        }

        public Vector2 WorldPosition
        {
            get; set;
        }

        public ICamera2D Camera
        {
            get; set;
        }

        public void OnPreparingForGeneration()
        {
            var handler = PreparingForGeneration;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        public void GenerateEnemies()
        {
            var r = new Random();

            int enemiesToGenerateCount = r.Next(EnemiesToGenerateMax - EnemiesToGenerateMin) + EnemiesToGenerateMin;
            for (int i = 0; i < enemiesToGenerateCount; i++)
            {
                int enemyTypeIndex = r.Next(EnemyTypes.Count);
                string enemyType = EnemyTypes[enemyTypeIndex];
                var enemy = EnemyMapper.GetEnemy(enemyType, Game.Content);
                var blow = BlowEffect.FromMetadata(Game.Content.Load<BlowEffectMetadata>(@"Effects\Blows\EnemyGenerationBlow"), Game.Content); 
                enemy.Initialize(Game, SpriteBatch, Camera);
                blow.Initialize(Game, SpriteBatch, Camera);
                blow.Finished += Level.OnAnimationFinished;
                var positionX = WorldPosition.X - r.Next(300);
                var positionY = WorldPosition.Y + r.Next(400);
                enemy.WorldPosition = new Vector2(positionX, positionY);
                blow.WorldPosition = new Vector2(positionX, positionY);
                Level.AddEnemy(enemy);
                Level.AddLevelObject(blow);
            }
        }

        public GenerateEnemiesScript()
        {
            EnemyTypes = new List<string>();
            IntervalForEnemiesGeneration = GenerateEnemiesTimespan();
            _preparingForGeneration = false;
        }

        private TimeSpan GenerateEnemiesTimespan()
        {
            var r = new Random();
            return TimeSpan.FromSeconds(r.Next((int)(GenerateEnemiesIntervalMax.TotalSeconds - GenerateEnemiesIntervalMin.TotalSeconds)) +
                                     GenerateEnemiesIntervalMin.TotalSeconds);
        }

        public virtual void Initialize(Game game, SpriteBatch spriteBatch, ICamera2D camera)
        {
            Game = game;
            SpriteBatch = spriteBatch;
            Camera = camera;
        }

        public override void Update(Core.GameState gameState, GameTime gameTime)
        {
            base.Update(gameState, gameTime);

            if (WorldPosition.X - gameState.Player.WorldPosition.X <= GenerationStartDistance)
            {
                IntervalForEnemiesGeneration =
                    IntervalForEnemiesGeneration.Add(-TimeSpan.FromMilliseconds(gameTime.ElapsedGameTime.Milliseconds));

                if (IntervalForEnemiesGeneration.TotalMilliseconds < 0.3 * GenerateEnemiesIntervalMin.TotalMilliseconds && !_preparingForGeneration)
                {
                    OnPreparingForGeneration();
                    _preparingForGeneration = true;
                }

                if (IntervalForEnemiesGeneration.TotalMilliseconds < 0)
                {
                    IntervalForEnemiesGeneration = GenerateEnemiesTimespan();
                    GenerateEnemies();
                    Generated.Fire(this, () => EventArgs.Empty);
                    _preparingForGeneration = false;
                }
            }
        }
    }
}
