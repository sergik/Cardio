using System;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Characters.Player.Spikes
{
    public class SpikeMetadata
    {
        public String AssetName { get; set; }

        public Vector2 Position { get; set; }

        public int ActiveTime { get; set; }

        public int ReuseTime { get; set; }
    }
}
