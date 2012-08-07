using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Core
{
    public interface IPositioned
    {
        Vector2 WorldPosition { get; set; }
    }
}