using Cardio.Phone.Shared.Core;
using Cardio.Phone.Shared.Core.Alive;

namespace Cardio.Phone.Shared.Core.Alive
{
    /// <summary>
    /// Marks objects that could be attacked by nanobots
    /// </summary>
    public interface IPlayerAttackTarget : IAlive, IPositioned
    {
        int PlayerAttackPriority { get; }
    }
}