using System;

namespace Cardio.UI.Exceptions
{
    /// <summary>
    /// Generally, is thrown when the component is not initialized and try to invoke a method that requires proper initialization (like Update or Draw)
    /// </summary>
    public sealed class ComponentIsNotInitializedException : Exception
    {
        public ComponentIsNotInitializedException() : this("Component is not initialized yet.") {}
        public ComponentIsNotInitializedException(string message) : base(message) {}
        public ComponentIsNotInitializedException(string message, Exception inner) : base(message, inner) {}
    }
}