using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Core
{
    public interface ICollidable: IPositioned
    {
        Rectangle Intersects(Rectangle rect);

        bool Contains(Point point);
    }
}
