using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.Phone.Shared.Inventory
{
    public class InventoryContentBuilder
    {
        private readonly ContentManager _content;

        public InventoryContentBuilder(ContentManager content)
        {
            _content = content;
        }

        public InventoryItem BuildBio()
        {
            //return new InventoryItem
            //{
            //    Name = "Biomaterial",
            //    Texture = _content.Load<Texture2D>(@"Textures\Inventory\BiomaterialInventory")
            //};
            throw new NotImplementedException();
        }
    }
}