using System;
using Microsoft.Xna.Framework;

namespace Cardio.UI.Animations
{
    public class AnimationMetadata
    {
        public String Name { get; set; }

        public String TextureName { get; set; }

        public int FramesPerRow { get; set; }

        public int Interval { get; set; }

        public Vector2 SourceOffset { get; set; }

        public float LastFrameHoldOnTime { get; set; }

        public bool IsLooped { get; set; }

        public Vector2 Size { get; set; }
    }
}
