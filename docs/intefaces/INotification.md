# INotification 

## Index

1. [Overview](#overview)
2. [Methods](#methods)
   - [Show](#show-string-title-string-message)

## Overview

The `INotification` interface provides a contract for classes that are responsible for displaying notifications. It defines a method for showing a notification with a title and a message. This can be implemented in different ways depending on the platform, such as showing system notifications, pop-ups, or toast messages.

## Methods

### Show(string title, string message)

Displays a notification with a specified title and message.

**Parameters:**

- `title` (string): The title of the notification.
- `message` (string): The message or content to display in the notification.

**Returns:**

- `void`: This method does not return any value.

---

## Example

### A Simple Console Notification

```csharp
using Moofin.Core.Interfaces;
using System;

public class ConsoleNotification : INotification
{
    public void Show(string title, string message)
    {
        Console.WriteLine($"[Notification] {title}: {message}");
    }
}

class Program
{
    static void Main()
    {
        INotification notification = new ConsoleNotification();
        notification.Show("Reminder", "You have a study session starting soon!");
    }
}
