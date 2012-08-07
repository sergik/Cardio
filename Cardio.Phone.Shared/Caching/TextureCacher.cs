using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Cardio.Phone.Shared.Exceptions;

namespace Cardio.Phone.Shared.Caching
{
    public class TextureCacher
    {
        private static Dictionary<string, Texture2D> Cache = new Dictionary<string, Texture2D>();


        public static bool UploadTexture(Game game, string textureName)
        {
            if (Cache.ContainsKey(textureName))
            {
                return false;
            }

            var newTexture = game.Content.Load<Texture2D>(textureName);

            if (newTexture == null)
            {
                throw new AssetLoadingException(textureName);
            }

            Cache.Add(textureName, newTexture);

            return true;
        }

        public static Texture2D GetTexture(Game game, string textureName)
        {
            if (!Cache.ContainsKey(textureName))
            {
                if (!UploadTexture(game, textureName))
                {
                    return null;
                }
            }

            return Cache[textureName];
        }

        public static void ClearCatche()
        {
            Cache.Clear();
        }

    }
}
