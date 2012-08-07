using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Weapons
{
    public class WeaponMetadata
    {
        public String Name { get; set; }

        public string AssetName { get; set; }

        public int Damage { get; set; }

        public Vector2 TargetOffset
        {
            get; set;
        }

        public String BulletAssetName { get; set; }

        public int AtackRate { get; set; }

        public int BulletSpeed { get; set; }

        public Vector2 BulletGenerationPosition { get; set; }
    }
}
