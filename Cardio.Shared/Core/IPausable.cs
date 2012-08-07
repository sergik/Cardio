namespace Cardio.UI.Core
{
    public interface IPausable
    {
        bool IsPaused { get; }

        void Pause();

        void Resume();
    }
}