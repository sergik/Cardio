namespace Cardio.UI.Inventory
{
    public class InventoryMapping
    {
        public InventoryItem Base { get; set; }

        public string LevelAsset { get; set; }

        public InventoryMapping(InventoryItem item, string levelAsset)
        {
            Base = item;
            LevelAsset = levelAsset;
        }
    }
}