namespace Cardio.UI.Core
{
    /// <summary>
    /// The object that player cannot go through
    /// </summary>
    public interface IObstacle : IPositioned
    {
        /// <summary>
        /// Offset relative to WorldPosition, determines where player should stop
        /// </summary>
        float StopDistance { get; set; }
    }
}