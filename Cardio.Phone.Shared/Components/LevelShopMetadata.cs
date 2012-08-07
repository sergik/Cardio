using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cardio.Phone.Shared.Components
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

        public List<ShopItemMetadata> Medcits { get; set; }

        public List<ShopItemMetadata> Tablets { get; set; }

        public List<ShopItemMetadata> Spikes { get; set; }

        public List<ShopItemMetadata> FrontWeapons { get; set; }


        public LevelShopMetadata()
        {
            Guns = new List<ShopItemMetadata>();
            Shields = new List<ShopItemMetadata>();
            Bodies = new List<ShopItemMetadata>();
            Medcits = new List<ShopItemMetadata>();
            Spikes = new List<ShopItemMetadata>();
        }
    }
}
