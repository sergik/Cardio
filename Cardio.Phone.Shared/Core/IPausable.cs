namespace Cardio.Phone.Shared.Core
{
    public interface IPausable
    {
        bool IsPaused { get; }

        void Pause();

        void Resume();
    }
}