using System;

namespace Moofin.Core.Exceptions
{
    public class MoofinException : Exception
    {
        public MoofinException(string message) : base(message) { }

        public MoofinException(string message, Exception inner) : base(message, inner) { }
    }
}