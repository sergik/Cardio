using System.Collections.Generic;

namespace Cardio.Phone.Shared.Input.Touch
{
    public interface ITouchService
    {
        void Push(ButtonTouch touch);

        void Reset();

        List<ButtonTouch> Touches { get; }

        bool ContainsValidSequence();
    }
}