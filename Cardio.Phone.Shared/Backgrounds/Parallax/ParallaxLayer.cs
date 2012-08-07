using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.Phone.Shared.Backgrounds.Parallax
{
    public class ParallaxLayer : ScrollingBackground
    {
        private float _depth = 400;
        public float Depth
        {
            get { return _depth; }
            set
            {
                _depth = value;
            }
        }

        private float _visibilityAngle = MathHelper.PiOver2;
        public float VisibilityAngle
        {
            get { return _visibilityAngle; }
            set
            {
                _visibilityAngle = value;
            }
        }

        public int Velocity { get; set; }

        private double _incomingDelta;

        private int _realPosition;
        private int _position;

        public override int Position
        {
            get
            {
                return _position;
            }
            set
            {
                _incomingDelta += (value - _realPosition + Velocity) * Game.GraphicsDevice.Viewport.Width / (2 * _depth * Math.Tan(_visibilityAngle / 2));

                _realPosition = value;


                if (Math.Abs(_incomingDelta) > 1)
                {
                    _position += (int)_incomingDelta;
                    _incomingDelta = _incomingDelta - (int)_incomingDelta;
                }
            }
        }

        private ParallaxLayer() { }

        public static ParallaxLayer FromMetadata(ParallaxLayerMetadata metadata, ContentManager content)
        {
            var layer = new ParallaxLayer
            {
                Depth = metadata.Depth,
                Texture = content.Load<Texture2D>(metadata.TextureName),
                Velocity = metadata.Velocity,
                VisibilityAngle = MathHelper.PiOver2
            };
            return layer;
        }
    }
}
