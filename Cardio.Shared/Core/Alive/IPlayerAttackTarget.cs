namespace Cardio.UI.Core.Alive
{
    /// <summary>
    /// Marks objects that could be attacked by nanobots
    /// </summary>
    public interface IPlayerAttackTarget : IAlive, IPositioned
    {
        int PlayerAttackPriority { get; }
    }
}