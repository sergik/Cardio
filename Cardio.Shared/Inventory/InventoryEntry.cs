namespace Cardio.UI.Inventory
{
    public class InventoryEntry
    {
        private int _count;
        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                CountText = _count.ToString();
            }
        }

        public string CountText { get; set; }

        public InventoryItem Base { get; set; }
    }
}