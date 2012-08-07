using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Animations
{
    public class AnimatedObjectMetadata
    {
        public List<AnimationMetadata> Animations { get; set; }

        public Vector2 Size { get; set; }

        public Rectangle CollisionRectangle { get; set; }
    }
}