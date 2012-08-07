using System.Collections.Generic;
using Cardio.UI.Scripts;
using Microsoft.Xna.Framework;

namespace Cardio.UI.Core
{
    public interface IGameEntity
    {
        void AddScript(GameEntityScript script);

        void Update(GameState gameState, GameTime gameTime);
    }
}