using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cardio.UI.Components
{
    public class LevelShopMetadata
    {
        public List<ShopItemMetadata> Guns
        {
            get; set;
        }

        public List<ShopItemMetadata> Shields
        {
            get;
            set;
        }

        public List<ShopItemMetadata> Bodies
        {
            get;
            set;
        }

        public LevelShopMetadata()
        {
            Guns = new List<ShopItemMetadata>();
            Shields = new List<ShopItemMetadata>();
            Bodies = new List<ShopItemMetadata>();
        }
    }
}
