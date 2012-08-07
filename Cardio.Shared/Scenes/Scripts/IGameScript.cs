using Cardio.UI.Core;
using Microsoft.Xna.Framework;

namespace Cardio.UI.Scenes.Scripts
{
    public interface IGameScript
    {
        void Start(GameState state);

        void Update(GameTime time, GameState state);

        void Stop(GameState state);

        bool IsFinished { get; }
    }
}