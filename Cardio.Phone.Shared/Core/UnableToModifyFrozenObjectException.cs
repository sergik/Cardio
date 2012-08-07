using System;

namespace Cardio.Phone.Shared.Core
{
    //[Serializable]
    public class UnableToModifyFrozenObjectException : Exception
    {
        public UnableToModifyFrozenObjectException() {}
        public UnableToModifyFrozenObjectException(string message) : base(message) {}
        public UnableToModifyFrozenObjectException(string message, Exception inner) : base(message, inner) {}
    }
}