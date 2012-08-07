using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Inventory;
using Cardio.Phone.Shared.Sounds;
using Microsoft.Xna.Framework;
using Cardio.Phone.Shared.Extensions;

namespace Cardio.Phone.Shared.Scripts
{
    public class PickObjectScript: GameEntityScript
    {
        private readonly Game _game;

        /// <summary>
        /// Item that has to be picked.
        /// </summary>
        public PickableGameObject PickTarget { get; set; }

        /// <summary>
        /// The one who will pick the item.
        /// </summary>
        public DrawableGameObject PickBy { get; set; }

        /// <summary>
        /// Amount of time, in milliseconds, during which the item has to be picked.
        /// </summary>
        public float PickingTime { get; set; }

        /// <summary>
        /// How long has the item already been moving.
        /// </summary>
        private float _currentTime;

        private InventoryMappingService _inventoryMappingService;

        public PickObjectScript(Game game)
        {
            _game = game;
            PickingTime = 1000;
        }

        protected override void OnStart(GameState state)
        {
            _currentTime = 0;
            _inventoryMappingService = _game.Services.GetService<InventoryMappingService>();
            PickTarget.IsBeingPicked = true;
            base.OnStart(state);
        }

        protected override void OnStop(GameState state)
        {
            PickTarget.IsBeingPicked = false;

            base.OnStop(state);
        }

        public override void Update(GameState state, GameTime time)
        {
            var elapsedTime = (float) time.ElapsedGameTime.TotalMilliseconds;

            var remainingDistance = new Vector2(PickBy.WorldCollisionRectangle.Center.X - PickTarget.WorldCollisionRectangle.Center.X,
                    PickBy.WorldCollisionRectangle.Center.Y - PickTarget.WorldCollisionRectangle.Center.Y);
            var remainingTime = PickingTime - _currentTime;
            var requiredSpeed = remainingDistance / remainingTime;
            var offset = requiredSpeed * elapsedTime;
            PickTarget.WorldPosition += offset;
            
            if (PickTarget.Intersects(PickBy.WorldCollisionRectangle) != Rectangle.Empty || _currentTime >= PickingTime)
            {
                AddToInventory(state);
            }

            _currentTime += elapsedTime;

            base.Update(state, time);
        }

        private void AddToInventory(GameState state)
        {
            state.Inventory.Add(_inventoryMappingService.GetMapping(PickTarget.InventoryName).Base, PickTarget.Count);

            state.Level.RemoveLevelObject(PickTarget);
            Stop(state);

            SoundManager.Pick.Play();
        }
    }
}
