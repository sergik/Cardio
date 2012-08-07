using Microsoft.Xna.Framework;

namespace Cardio.UI.Characters.Behaviors
{
    public interface ICharacterBehavior
    {
        void Update(Character character, GameTime gameTime);
    }
}