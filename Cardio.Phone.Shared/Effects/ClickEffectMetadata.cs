using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cardio.Phone.Shared.Core;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Effects
{
    public class ClickEffectMetadata  
    {
        public String ContentPath { get; set; }

        public Vector2 StartSize { get; set; }

        public Vector2 EndSize { get; set; }

        public float TimeAlive { get; set; }
    }
}
