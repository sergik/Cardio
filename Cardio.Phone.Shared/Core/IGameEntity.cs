using System.Collections.Generic;
using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Scripts;
using Cardio.Phone.Shared.Scripts;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Core
{
    public interface IGameEntity
    {
        void AddScript(GameEntityScript script);

        void Update(GameState gameState, GameTime gameTime);
    }
}