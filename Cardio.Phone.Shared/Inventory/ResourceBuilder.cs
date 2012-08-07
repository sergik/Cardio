using Cardio.Phone.Shared;
using Microsoft.Xna.Framework.Content;

namespace Cardio.Phone.Shared.Inventory
{
    public class ResourceBuilder
    {
        private readonly InventoryMappingService _mappingService;

        public ResourceBuilder(InventoryMappingService mappingService)
        {
            _mappingService = mappingService;
        }

        public PickableGameObject BuildBio(ContentManager contentManager, int countFrom, int countTo)
        {
            var pickable = _mappingService.CreatePickableObject(Items.BloodElement.Name, contentManager);
            pickable.Count = RandomHelper.RandomFrom(countFrom, countTo);
            return pickable;
        }
    }
}
