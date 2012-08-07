using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Scripts;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.Phone.Shared.Inventory
{
    public class Tablet: InventoryItem
    {
        public int Damage { get; set; }

        public Tablet()
        {
            Use += TabletUse;
        }

        private void TabletUse(GameState state)
        {
            IsEnabled = false;

            var script = new UseTabletScript(Damage);

            state.AddScript(script);
        }

        public static Tablet FromMetadata(TabletMetadata metadata, ContentManager contentManager)
        {
            var tablet = new Tablet();
            tablet.IsReuseble = true;
            tablet.ReuseTime = metadata.ReuseTime;
            tablet.InventoryTexture = contentManager.Load<Texture2D>(metadata.AssetName);
            tablet.Damage = metadata.Damage;
            tablet.SlotIndex = -1;
            tablet.Name = "Tablet";

            return tablet;
        }
    }
}
