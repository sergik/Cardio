using Cardio.UI.Characters;
using Cardio.UI.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cardio.UI.Components
{
    public interface ICamera2D
    {
        /// <summary>
        /// Gets or sets the position of the camera
        /// </summary>
        /// <value>The position.</value>
        Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the move speed of the camera.
        /// The camera will tween to its destination.
        /// </summary>
        /// <value>The move speed.</value>
        float MoveSpeed { get; set; }

        /// <summary>
        /// Gets or sets the rotation of the camera.
        /// </summary>
        /// <value>The rotation.</value>
        float Rotation { get; set; }

        /// <summary>
        /// Gets the origin of the viewport (accounts for Scale)
        /// </summary>        
        /// <value>The origin.</value>
        Vector2 Origin { get; }

        /// <summary>
        /// Gets or sets the scale of the Camera
        /// </summary>
        /// <value>The scale.</value>
        float Scale { get; set; }

        /// <summary>
        /// Gets the screen center (does not account for Scale)
        /// </summary>
        /// <value>The screen center.</value>
        Vector2 ScreenCenter { get; }

        /// <summary>
        /// Gets the transform that can be applied to 
        /// the SpriteBatch Class.
        /// </summary>
        /// <see cref="SpriteBatch"/>
        /// <value>The transform.</value>
        Matrix Transform { get; }

        /// <summary>
        /// Gets or sets the focus of the Camera.
        /// </summary>
        /// <seealso cref="IPositioned"/>
        /// <value>The focus.</value>
        IPositioned FocusedAt { get; set; }

        /// <summary>
        /// Lets you provide the offset from the camera focused object.
        /// The camera will follow <see cref="FocusedAt"/> target, but this target will not be in the camera's viewport center.
        /// </summary>
        Vector2 FocusedAtOffset { get; set; }

        /// <summary>
        /// Camera viewport width.
        /// Usually this value depends on the current camera scale.
        /// </summary>
        float ViewportWidth { get; }

        /// <summary>
        /// Camera viewport width.
        /// Usually this value depends on the current camera scale.
        /// </summary>
        float ViewportHeight { get; }

        /// <summary>
        /// The maximum value of this camera scale. Max scale means the lowest zoom value.
        /// </summary>
        float MaxScale { get; }

        /// <summary>
        /// The minimum value of this camera scale. Min scale means the highest zoom value.
        /// </summary>
        float MinScale { get; }

        /// <summary>
        /// Determines whether the target is in view given the specified position.
        /// This can be used to increase performance by not drawing objects
        /// directly in the viewport
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="texture">The texture.</param>
        /// <returns>
        ///     <c>true</c> if the target is in view at the specified position; otherwise, <c>false</c>.
        /// </returns>
        bool IsInView(Vector2 position, Texture2D texture);

        void Initialize(Game game);

        void Update(GameTime gameTime);

        Point GetScreenPosition(Vector2 worldPosition);

        Point GetScreenPosition(Vector2 worldPosition, float atScale);

        Rectangle GetScreenRectangle(Rectangle worldRectangle);
    }
}