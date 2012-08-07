using System;
using System.Collections.Generic;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Core.Alive;
using Cardio.Phone.Shared.Effects;
using Cardio.Phone.Shared.Input;
using Cardio.Phone.Shared.Levels;
using Cardio.Phone.Shared.Projectiles;
using Cardio.Phone.Shared.Scripts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Cardio.Phone.Shared.Extensions;

namespace Cardio.Phone.Shared.Characters.Player
{
    public class Player : GameEntity, IPlayer
    {
        private const int ToolbarHeight = 100;
        private const int MoveSpeed = 8;

        private Game _game;

        private Vector2 _positionMoveTo;

        public Nanobot Nanobot { get; set; }

        public bool Handled { get; set; }

        public IList<DrawableGameObject> Effects { get; set; }

        public float MaxHealth
        {
            get { return Nanobot.MaxHealth; }
            set { Nanobot.MaxHealth = value; }
        }

        public float AttackDamage
        {
            get { return Nanobot.Gun.Damage; }
            set
            {
            }
        }

        public float AttackRange { get; set; }

        public float Health
        {
            get
            {
                return Nanobot.Health;
            }
            set { Nanobot.Health = value; }
        }

        public Vector2 WorldPosition { get; set; }

        public Vector2 BoundingSize { get; private set; }

        public bool IsMoving
        {
            get { return Nanobot.IsMoving; }
            set { Nanobot.IsMoving = value; }
        }

        public bool IsShooting
        {
            get { return Nanobot.IsShooting; }
            set { Nanobot.IsShooting = value; }
        }

        public bool CanEvadeAttack { get; set; }

        public bool CanSwitch { get; set; }

        public Player()
        {
            Effects = new List<DrawableGameObject>();
            CreateNanobot();
        }

        private void CreateNanobot()
        {
            Nanobot = new Nanobot();
            Nanobot.GroupPosition = new Vector2(0, 0);
        }

        public void Initialize(Game game, SpriteBatch spriteBatch)
        {
            var service = game.Services.GetService(typeof(GameState)) as GameState;
            CanSwitch = true;
            AddScript(new MovePlayerScript());
            Nanobot.Initialize(game, spriteBatch, service.Camera);
            _game = game;
        }

        public override void Update(GameState gameState, GameTime gameTime)
        {
            base.Update(gameState, gameTime);
            for (int i = 0; i < Effects.Count; i++)
            {
                Effects[i].Update(gameState, gameTime);
            }

            Nanobot.WorldPosition = WorldPosition + Nanobot.GroupPosition;
            Nanobot.Update(gameState, gameTime);
            
            //if(IsMoving)
            //{
            //    _positionMoveTo = Nanobot.GroupPosition;
            //}

            BoundingSize = GetBoundingSize();
            HanldleTouch(gameState);
            HandleIntersection(gameState, gameTime);
        }

        public void HandleIntersection(GameState state, GameTime time)
        {
            var botPosition = Nanobot.WorldPosition;
            var gameState = _game.Services.GetService<GameState>();
            var level = gameState.Level;
            var size = Nanobot.Size ?? Nanobot.AutoSize;
            var rect = new Rectangle((int)(botPosition.X),
                                     (int)(botPosition.Y), (int)size.X, (int)size.Y);

            var enemies = level.IntersectedObstacle(rect);
            foreach (var enemy in enemies)
            {
                if (enemy != null)
                {
                    if (Nanobot.Spikes == null || !Nanobot.Spikes.Activated)
                    {
                        Nanobot.Damage(new DamageTakenEventArgs(enemy.Health,
                                                                botPosition));
                    }
                    enemy.Damage(
                            new DamageTakenEventArgs(enemy.Health,
                                                     enemy.WorldPosition));

                }
            }
        }

        private void HanldleTouch(GameState gameState)
        {
            var heightDelta = gameState.Camera.ViewportHeight/2;
            if(InputManager.IsButtonClicked(new Rectangle(0, 0, (int)gameState.Camera.ViewportWidth, (int)gameState.Camera.ViewportHeight)) || Handled)
            {
                if (InputManager.CurrentMouseState.Y > ToolbarHeight)
                {
                    //float clickX = gameState.Camera.Position.X;
                    _positionMoveTo =
                        new Vector2(InputManager.CurrentMouseState.X, InputManager.CurrentMouseState.Y -
                                    (Nanobot.Content.BoundingRectangle.Height + Nanobot.Content.BoundingRectangle.Y)/2);
                    
                        //AddClickEffect(gameState,
                        //               new Vector2(
                        //                   InputManager.CurrentMouseState.X - _game.GraphicsDevice.Viewport.Width/2 +
                        //                   clickX,
                        //                   InputManager.CurrentMouseState.Y - _game.GraphicsDevice.Viewport.Height/2));
                    
                }
                Handled = true;
            }
            if (Handled && InputManager.IsMouseUp())
            {
                Handled = false;
            }
            if(Nanobot.GroupPosition == _positionMoveTo)
            {
                return;
            }

            float deltaX;
            
           
            deltaX = MoveSpeed - MovePlayerScript.PlayerSpeed;
            if (Nanobot.GroupPosition.X > _positionMoveTo.X)
            {
                deltaX = -MoveSpeed + MovePlayerScript.PlayerSpeed;
                if (Nanobot.GroupPosition.X + deltaX < _positionMoveTo.X)
                {
                    deltaX = _positionMoveTo.X - Nanobot.GroupPosition.X;
                }
            }
            else
            {
                if (Nanobot.GroupPosition.X + deltaX > _positionMoveTo.X)
                {
                    deltaX = _positionMoveTo.X - Nanobot.GroupPosition.X;
                }
            }

            float deltaY = MoveSpeed;
            if (Nanobot.GroupPosition.Y > _positionMoveTo.Y - heightDelta)
            {
                deltaY = -MoveSpeed;
                if (Nanobot.GroupPosition.Y + deltaY < _positionMoveTo.Y - heightDelta)
                {
                    deltaY = (_positionMoveTo.Y - heightDelta) - Nanobot.GroupPosition.Y;
                }
            } 
            else
            {
                if (Nanobot.GroupPosition.Y + deltaY > _positionMoveTo.Y - heightDelta)
                {
                    deltaY = (_positionMoveTo.Y) - heightDelta - Nanobot.GroupPosition.Y;
                }
            }

            Nanobot.GroupPosition = new Vector2(Nanobot.GroupPosition.X + deltaX, Nanobot.GroupPosition.Y + deltaY);
        }

        private void AddClickEffect(GameState gameState, Vector2 position)
        {
            if (
                Effects.Any(
                    e =>
                    e is ClickEfect && Math.Abs(e.WorldPosition.X - position.X) < 30 &&
                    Math.Abs(e.WorldPosition.Y - position.Y) < 30))
            {
                return;
            }
            var click = ClickEfect.FromMetadata(_game.Content.Load<ClickEffectMetadata>(@"Effects\Cursor"),
                                                _game.Content);
            click.WorldPosition = position;
            Effects.Add(click);
            click.Ended += () => Effects.Remove(click);
            click.Initialize(_game, _game.Services.GetService<SpriteBatch>(), gameState.Camera);
        }

        private Vector2 GetBoundingSize()
        {
            var minX = float.MaxValue;
            var minY = float.MaxValue;
            var maxX = float.MinValue;
            var maxY = float.MinValue;

            if (Nanobot.WorldPosition.X < minX)
            {
                minX = Nanobot.WorldPosition.X;
            }

            if (Nanobot.WorldPosition.Y < minY)
            {
                minY = Nanobot.WorldPosition.Y;
            }

            var right = Nanobot.WorldPosition.X + Nanobot.CollisionRectangle.Width;
            var bottom = Nanobot.WorldPosition.Y + Nanobot.CollisionRectangle.Height;
            if (right > maxX)
            {
                maxX = right;
            }
            if (bottom > maxY)
            {
                maxY = bottom;
            }

            return new Vector2(maxX - minX, maxY - minY);
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var effect in Effects)
            {
                effect.Draw(gameTime);
            }
            Nanobot.Draw(gameTime);
        }
    }
}
