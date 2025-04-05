# IStorage

## Index

1. [Overview](#overview)
2. [Methods](#methods)
   - [SaveAsync](#saveasynctstring-key-t-data)
   - [LoadAsync](#loadasynctstring-key)
   - [DeleteAsync](#deleteasynctstring-key)
3. [Example Implementations](#example-implementations)

## Overview

The `IStorage` interface provides a contract for classes that handle asynchronous data storage. This interface allows storing, loading, and deleting data associated with a given key. It is designed for scenarios where data needs to be persisted in a storage medium such as local storage, cloud storage, or a database.

## Methods

### SaveAsync<T>(string key, T data)

Asynchronously saves the specified data associated with the given key.

**Parameters:**

- `key` (string): A unique identifier for the data being stored.
- `data` (T): The data to be stored, which can be of any type (`T`).

**Returns:**

- `Task`: An asynchronous operation representing the save action.

---

### LoadAsync<T>(string key)

Asynchronously loads the data associated with the specified key.

**Parameters:**

- `key` (string): The key used to look up the data.

**Returns:**

- `Task<T>`: An asynchronous operation that returns the data associated with the specified key. If the data does not exist, the result will be the default value of the type `T`.

---

### DeleteAsync(string key)

Asynchronously deletes the data associated with the specified key.

**Parameters:**

- `key` (string): The key identifying the data to delete.

**Returns:**

- `Task`: An asynchronous operation representing the delete action.

---

## Example Implementations

### In-Memory Storage Implementation

```csharp
using Moofin.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

public class InMemoryStorage : IStorage
{
    private readonly Dictionary<string, object> _storage = new();

    public Task SaveAsync<T>(string key, T data)
    {
        _storage[key] = data;
        return Task.CompletedTask;
    }

    public Task<T> LoadAsync<T>(string key)
    {
        if (_storage.TryGetValue(key, out var value))
        {
            return Task.FromResult((T)value);
        }
        return Task.FromResult(default(T));
    }

    public Task DeleteAsync(string key)
    {
        _storage.Remove(key);
        return Task.CompletedTask;
    }
}

class Program
{
    static async Task Main()
    {
        IStorage storage = new InMemoryStorage();

        // Save data
        await storage.SaveAsync("user_name", "John Doe");

        // Load data
        var userName = await storage.LoadAsync<string>("user_name");
        Console.WriteLine(userName); // Output: John Doe

        // Delete data
        await storage.DeleteAsync("user_name");

        // Verify data deletion
        var deletedUserName = await storage.LoadAsync<string>("user_name");
        Console.WriteLine(deletedUserName); // Output: (null)
    }
}
