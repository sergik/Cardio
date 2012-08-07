using System;
using System.Collections.Generic;
using Cardio.UI.Core;
using Cardio.UI.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Cardio.UI.Characters.Player
{
    public class Player : GameEntity, IPlayer
    {
        private static readonly Random Random = new Random();

        private const int NanobotsCount = 4;

        public List<Nanobot> Nanobots { get; set; }

        public event EventHandler Confused;

        private readonly GroupPositionSwitch _groupSwitch;

        public float MaxHealth
        {
            get
            {
                var result = 0f;
                for (int i = 0; i < Nanobots.Count; i++)
                {
                    result += Nanobots[i].MaxHealth;
                }
                return result;
            }
            set
            {
                for (int index = 0; index < Nanobots.Count; index++)
                {
                    Nanobots[index].MaxHealth = value / Nanobots.Count;
                }
            }
        }

        public float AttackDamage
        {
            get
            {
                return Nanobots.Select(n => n.Gun.Damage).Sum();
            }
            set
            {
            }
        }

        public float AttackRange { get; set; }

        public float Health
        {
            get
            {
                var result = 0f;
                for (int i = 0; i < Nanobots.Count; i++)
                {
                    result += Nanobots[i].Health;
                }
                return result;
            }
            set
            {
                var perOne = value / Nanobots.Count;
                for (int index = 0; index < Nanobots.Count; index++)
                {
                    Nanobots[index].Health = perOne;
                }
            }
        }

        public Vector2 WorldPosition { get; set; }

        public Vector2 BoundingSize { get; private set; }

        public bool IsMoving
        {
            get
            {
                for (int i = 0; i < Nanobots.Count; i++)
                {
                    if (Nanobots[i].IsMoving)
                    {
                        return true;
                    }
                }

                return false;
            }
            set
            {
                for (int i = 0; i < Nanobots.Count; i++ )
                {
                    Nanobots[i].IsMoving = value;
                }
            }
        }

        public bool IsShooting
        {
            get
            {
                for (int i = 0; i < Nanobots.Count; i++)
                {
                    if (Nanobots[i].IsShooting)
                    {
                        return true;
                    }
                }

                return false;
            }
            set
            {
                for (int i = 0; i < Nanobots.Count; i++)
                {
                    Nanobots[i].IsShooting = value;
                }
            }
        }

        public bool CanEvadeAttack { get; set; }

        public bool CanSwitch { get; set; }

        public Player(): this(NanobotsCount) {}

        public Player(int botCount)
        {
            CreateNanobots(botCount);
            _groupSwitch = new GroupPositionSwitch(Nanobots);
        }

        private void CreateNanobots(int nanobotsCount)
        {
            Nanobots = new List<Nanobot>();
            for (int i = 0; i < nanobotsCount; i++)
            {
                var bot = new Nanobot
                {
                    GroupPosition = new Vector2(i % 2 * 100, i * 75 - 75)
                };
                Nanobots.Add(bot);
            }
        }

        public void Confuse()
        {
            Nanobots[0].Confuse();

            var handler = Confused;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public void Initialize(Game game, SpriteBatch spriteBatch)
        {
            var service = game.Services.GetService(typeof(GameState)) as GameState;
            CanSwitch = true;
            foreach (var t in Nanobots)
            {
                t.Initialize(game, spriteBatch, service.Camera);
            }

        }

        public override void Update(GameState gameState, GameTime gameTime)
        {
            base.Update(gameState, gameTime);

            if (CanSwitch || _groupSwitch.IsSwitching)
            {
                _groupSwitch.Update(gameTime);
            }

            for (int i = 0; i < Nanobots.Count; i++)
            {
                var bot = Nanobots[i];
                bot.WorldPosition = WorldPosition + bot.GroupPosition;
                bot.Update(gameState, gameTime);
            }

            BoundingSize = GetBoundingSize();
        }

        private Vector2 GetBoundingSize()
        {
            var minX = float.MaxValue;
            var minY = float.MaxValue;
            var maxX = float.MinValue;
            var maxY = float.MinValue;
            for (int i = 0; i < Nanobots.Count; i++)
            {
                var bot = Nanobots[i];
                if (bot.WorldPosition.X < minX)
                {
                    minX = bot.WorldPosition.X;
                }

                if (bot.WorldPosition.Y < minY)
                {
                    minY = bot.WorldPosition.Y;
                }

                var right = bot.WorldPosition.X + bot.CollisionRectangle.Width;
                var bottom = bot.WorldPosition.Y + bot.CollisionRectangle.Height;
                if (right > maxX)
                {
                    maxX = right;
                }
                if (bottom > maxY)
                {
                    maxY = bottom;
                }
            }

            return new Vector2(maxX - minX, maxY - minY);
        }

        public void Draw(GameTime gameTime)
        {
            for (int i = 0; i < Nanobots.Count; i++ )
            {
                Nanobots[i].Draw(gameTime);
            }
        }
    }
}
