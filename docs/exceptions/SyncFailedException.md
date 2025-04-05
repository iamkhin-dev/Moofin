# SyncFailedException

## Index

1. [Overview](#overview)
2. [Constructors](#constructors)
   - [SyncFailedException(string message)](#syncfailedexceptionstring-message)
   - [SyncFailedException(string message, Exception inner)](#syncfailedexceptionstring-message-exception-inner)
3. [Example Usage](#example-usage)

## Overview

`SyncFailedException` is a custom exception class that inherits from the `MoofinException` class in the `Moofin.Core.Exceptions` namespace. This exception is used to indicate a failure during the synchronization process within the Moofin application.

It provides constructors for initializing the exception with a message and optionally an inner exception that causes the sync failure.

## Constructors

### SyncFailedException(string message)

Initializes a new instance of the `SyncFailedException` class with a specified error message that describes the failure.

**Parameters:**

- `message`: A string that describes the sync failure error.

**Example Usage:**

```csharp
throw new SyncFailedException("Sync operation failed due to network issues.");
