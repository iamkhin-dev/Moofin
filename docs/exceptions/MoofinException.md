# MoofinException 

## Index

1. [Overview](#overview)
2. [Constructors](#constructors)
   - [MoofinException(string message)](#moofinexceptionstring-message)
   - [MoofinException(string message, Exception inner)](#moofinexceptionstring-message-exception-inner)
3. [Example Usage](#example-usage)

## Overview

`MoofinException` is a custom exception class in the `Moofin.Core.Exceptions` namespace. It extends the base `Exception` class and provides constructors for initializing the exception with a message and optionally an inner exception.

This exception class is typically used for error handling within the Moofin application.

## Constructors

### MoofinException(string message)

Initializes a new instance of the `MoofinException` class with a specified error message.

**Parameters:**

- `message`: A string that describes the error.

**Example Usage:**

```csharp
throw new MoofinException("An error occurred during the operation.");
