# ISyncProvider

## Index

1. [Overview](#overview)
2. [Methods](#methods)
   - [SyncAsync](#syncasync)
3. [Events](#events)
   - [SyncCompleted](#syncccompleted)
4. [Example Implementations](#example-implementations)

## Overview

The `ISyncProvider` interface defines a contract for classes that provide synchronization capabilities. This interface includes an asynchronous method for performing the synchronization process (`SyncAsync`) and an event (`SyncCompleted`) that signals the completion of the synchronization.

## Methods

### SyncAsync()

Asynchronously performs the synchronization process.

**Returns:**

- `Task`: An asynchronous operation that represents the synchronization process. The synchronization logic will be executed when this method is invoked.

---

## Events

### SyncCompleted

An event that is triggered when the synchronization process is completed.

**Event Arguments:**

- `SyncEventArgs`: Contains information about the synchronization process, including whether it was successful (`Success`) and an optional message (`Message`).

---

## Example Implementations

### SyncProvider Implementation

```csharp
using Moofin.Core.Interfaces;
using System;
using System.Threading.Tasks;

public class SyncProvider : ISyncProvider
{
    public event EventHandler<SyncEventArgs> SyncCompleted;

    public async Task SyncAsync()
    {
        // Simulate a sync process
        await Task.Delay(2000); // Simulating a 2-second sync operation

        // Raise the event to indicate that the sync has completed
        OnSyncCompleted(new SyncEventArgs
        {
            Success = true,
            Message = "Synchronization completed successfully!"
        });
    }

    protected virtual void OnSyncCompleted(SyncEventArgs e)
    {
        SyncCompleted?.Invoke(this, e);
    }
}

class Program
{
    static async Task Main()
    {
        ISyncProvider syncProvider = new SyncProvider();

        // Subscribe to the SyncCompleted event
        syncProvider.SyncCompleted += (sender, e) =>
        {
            if (e.Success)
            {
                Console.WriteLine($"Sync completed: {e.Message}");
            }
            else
            {
                Console.WriteLine($"Sync failed: {e.Message}");
            }
        };

        // Start the sync process
        await syncProvider.SyncAsync();
    }
}
