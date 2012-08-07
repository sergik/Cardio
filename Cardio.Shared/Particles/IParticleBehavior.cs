using Microsoft.Xna.Framework;

namespace Cardio.UI.Particles
{
    public interface IParticleBehavior
    {
        Vector2 GetNewPosition(Vector2 oldPosition, float elapsedMilliseconds);
    }
}
