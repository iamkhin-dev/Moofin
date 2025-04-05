using System;
using System.Threading.Tasks;

namespace Moofin.Core.Interfaces
{
    public interface ISyncProvider
    {
        Task SyncAsync();
        event EventHandler<SyncEventArgs> SyncCompleted;
    }

    public class SyncEventArgs : EventArgs
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}