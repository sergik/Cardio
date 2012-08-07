using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Core;

namespace Cardio.Phone.Shared.Backgrounds.Parallax
{
    public class ParallaxLayerMetadata: FreezableBase
    {
        private float _depth;
        public float Depth
        {
            get { return _depth; }
            set
            {
                EnsureNotFrozen();
                _depth = value;
            }
        }

        private int _velocity;
        public int Velocity
        {
            get { return _velocity; }
            set
            {
                EnsureNotFrozen();
                _velocity = value;
            }
        }

        private string _textureName;
        public string TextureName
        {
            get { return _textureName; }
            set
            {
                EnsureNotFrozen();
                _textureName = value;
            }
        }
    }
}
