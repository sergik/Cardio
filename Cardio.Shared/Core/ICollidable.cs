using Microsoft.Xna.Framework;

namespace Cardio.UI.Core
{
    public interface ICollidable: IPositioned
    {
        Rectangle Intersects(Rectangle rect);

        bool Contains(Point point);
    }
}
