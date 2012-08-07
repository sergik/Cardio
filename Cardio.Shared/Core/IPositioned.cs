using Microsoft.Xna.Framework;

namespace Cardio.UI.Core
{
    public interface IPositioned
    {
        Vector2 WorldPosition { get; set; }
    }
}