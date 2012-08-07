using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Cardio.UI.Weapons
{
    public class WeaponMetadata
    {
        public string AssetName { get; set; }

        public int Damage { get; set; }

        public Vector2 TargetOffset
        {
            get; set;
        }
    }
}
