using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Scripts;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.Phone.Shared.Inventory
{
    public class Medcit : InventoryItem 
    {
        public int HealingValue { get; set; }

        public Medcit()
        {
            Use += UseMedkit;
        }

        public static Medcit FromMetadata(MedcitMetadata metadata, ContentManager contentManager)
        {
            var medcit = new Medcit();
            medcit.IsReuseble = true;
            medcit.ReuseTime = metadata.ReuseTime;
            medcit.InventoryTexture = contentManager.Load<Texture2D>(metadata.AssetName);
            medcit.HealingValue = metadata.HealingValue;
            medcit.SlotIndex = 2;

            return medcit;
        }

        private void UseMedkit(GameState state)
        {
            IsEnabled = false;

            var script = new HealPlayerScript(HealingValue);

            state.AddScript(script);
        }
    }
}
