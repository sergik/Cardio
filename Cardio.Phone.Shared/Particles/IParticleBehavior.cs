using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Particles
{
    public interface IParticleBehavior
    {
        Vector2 GetNewPosition(Vector2 oldPosition, float elapsedMilliseconds);
    }
}
