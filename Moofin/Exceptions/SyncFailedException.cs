using System;

namespace Moofin.Core.Exceptions
{
    public class SyncFailedException : MoofinException
    {
        public SyncFailedException(string message) : base(message) { }

        public SyncFailedException(string message, Exception inner) : base(message, inner) { }
    }
}