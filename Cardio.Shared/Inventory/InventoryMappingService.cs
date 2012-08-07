using System.Collections.Generic;
using Cardio.UI.Animations;
using Microsoft.Xna.Framework.Content;

namespace Cardio.UI.Inventory
{
    public class InventoryMappingService
    {
        private readonly IDictionary<string , InventoryMapping> _mappings = new Dictionary<string, InventoryMapping>
        {
            {Items.SmallMedkit.Name, new InventoryMapping(Items.SmallMedkit, @"Animations\Inventory\Medkit")},
            {Items.BloodElement.Name, new InventoryMapping(Items.BloodElement, @"Animations\Inventory\Biomaterial")},
            {Items.Oxygen.Name, new InventoryMapping(Items.Oxygen, @"Animations\Inventory\Biomaterial")},
            {Items.DnaPiece.Name, new InventoryMapping(Items.DnaPiece, @"Animations\Inventory\Biomaterial")}
        };

        public InventoryMapping GetMapping(string name)
        {
            InventoryMapping result;
            if (_mappings.TryGetValue(name, out result))
            {
                return result;
            }

            return null;
        }

        public PickableGameObject CreatePickableObject(string name, ContentManager contentManager)
        {
            var mapping = GetMapping(name);

            var result = new PickableGameObject
            {
                Content = AnimatedObject.FromMetadata(contentManager.Load<AnimatedObjectMetadata>(mapping.LevelAsset), contentManager),
                InventoryName = name,
                Count = 1
            };

            return result;
        }
    }
}
