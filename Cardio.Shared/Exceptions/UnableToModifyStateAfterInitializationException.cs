using System;

namespace Cardio.UI.Exceptions
{
    public sealed class UnableToModifyStateAfterInitializationException : Exception
    {
        public UnableToModifyStateAfterInitializationException(): this("Component state cannot be modified after it is initialized."){ }
        public UnableToModifyStateAfterInitializationException(string message) : base(message) {}
        public UnableToModifyStateAfterInitializationException(string message, Exception inner) : base(message, inner) {}
    }
}