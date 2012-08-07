using System;
using Cardio.UI.Input;
using Cardio.UI.Inventory;
using Microsoft.Xna.Framework;

namespace Cardio.UI.Scripts
{
    public class CheckForPickableObjectsScript: GameEntityScript
    {
        private readonly Game _game;
        private static Random Random = new Random();

        public float PickRadius { get; set; }

        public CheckForPickableObjectsScript(Game game)
        {
            _game = game;
            PickRadius = 150;
        }

        public override void Update(Core.GameState state, GameTime time)
        {
            for (int index = 0; index < state.Level.CustomLevelObjects.Count; index++)
            {
                var item = state.Level.CustomLevelObjects[index];
                var pickable = item as PickableGameObject;
                if (pickable != null &&
                    InputManager.IsButtonClicked(state.Camera.GetScreenRectangle(pickable.WorldCollisionRectangle)))
                {
                    state.HandledMouseOnThisUpdate = true;
                    state.AddScript(new PickObjectScript(_game)
                    {PickTarget = pickable, PickBy = state.Player.Nanobots[Random.Next(state.Player.Nanobots.Count)]});
                }
            }

            base.Update(state, time);
        }
    }
}
