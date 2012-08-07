using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Core;
using Microsoft.Xna.Framework;

namespace Cardio.Phone.Shared.Scenes.Scripts
{
    public interface IGameScript
    {
        void Start(GameState state);

        void Update(GameTime time, GameState state);

        void Stop(GameState state);

        bool IsFinished { get; }
    }
}