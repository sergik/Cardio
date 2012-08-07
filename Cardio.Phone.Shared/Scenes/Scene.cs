using System;
using System.Collections.Generic;
using Cardio.UI.Components;
using Cardio.UI.Core;
using Cardio.UI.Input.Touch;
using Cardio.UI.Scenes.Actions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.Scenes
{
    public class Scene
    {
        /// <summary>
        /// Describes the level position when this sceen should begin.
        /// </summary>
        public float StartPosition { get; set; }

        public float ActivationThreshold { get; set; }

        /// <summary>
        /// True, if the scene has finished its playback/logic actions.
        /// </summary>
        public bool IsFinished { get; protected set; }

        /// <summary>
        /// The length of this scene (in level coordinates).
        /// </summary>
        public float Length { get; set; }

        /// <summary>
        /// Collection of reactions for this scene.
        /// </summary>
        public IDictionary<PlayerAction, Action<GameState>> Reactions { get; private set; }

        /// <summary>
        /// True if the scene has finished its enter action.
        /// </summary>
        public bool HasEntered { get; set; }

        /// <summary>
        /// True if the scene has finished its exit action.
        /// </summary>
        public bool HasExited { get; set; }

        public Scene()
        {
            Reactions = new Dictionary<PlayerAction, Action<GameState>>();
            ActivationThreshold = 60;
        }

        public virtual void Initialize(Game game, SpriteBatch spriteBatch, ICamera2D camera)
        {
            // blank
        }

        public virtual bool CanBeActivated(GameState gameState)
        {
            return gameState.Player.WorldPosition.X + gameState.Camera.ViewportWidth >= StartPosition + ActivationThreshold;
        }

        // returns true if any action has been invoked
        public virtual bool ProcessTouchSequence(GameState state, IList<ButtonType> sequence)
        {
            var match = ActionCatalog.CheckSequence(sequence);
            if (match != null)
            {
                Action<GameState> handler;
                if (Reactions.TryGetValue(match.Value, out handler))
                {
                    handler(state);
                    return true;
                }
            }

            return false;
        }

        public virtual void OnEnter(GameTime gameTime, GameState gameState)
        {
            HasEntered = true;
        }

        public virtual void OnExit(GameTime gameTime, GameState gameState)
        {
            gameState.Player.IsMoving = false;
            gameState.Player.IsShooting = false;
            HasExited = true;
        }

        public virtual void Update(GameTime gameTime, GameState gameState)
        {
            
        }

        public virtual void Draw(GameTime gameTime) {}

        public virtual bool CanMovePlayer(GameState state)
        {
            return state.Player.WorldPosition.X + state.Player.BoundingSize.X <= StartPosition;
        }
    }
}