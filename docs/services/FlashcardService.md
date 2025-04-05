# FlashcardService

## Index

1. [Overview](#overview)
2. [Methods](#methods)
   - [AddFlashcard](#addflashcardstring-question-string-answer)
   - [AddFlashcard](#addflashcardflashcard-flashcard)
   - [GetNextDueCard](#getnextduecard)
   - [UpdateCardProgress](#updatecardprogressguid-cardid-bool-wascorrect)
   - [GetAllFlashcards](#getallflashcardsbool-includearchived--false)
   - [GetFlashcardById](#getflashcardbyidguid-id)
   - [RemoveFlashcard](#removeflashcardguid-id)
   - [ArchiveFlashcard](#archiveflashcardguid-id-bool-archive)
   - [GetStats](#getstats)
   - [SearchFlashcards](#searchflashcardsstring-searchtext-bool-searchinanswers--true)
   - [AddTag](#addtagguid-cardid-string-tag)
   - [RemoveTag](#removetagguid-cardid-string-tag)
3. [Classes](#classes)
   - [Flashcard](#flashcard)
   - [FlashcardStats](#flashcardstats)
   - [OperationResult](#operationresult)
4. [Thread Safety](#thread-safety)
5. [Usage Example](#usage-example)

## Overview

The `FlashcardService` class is responsible for managing flashcards in a spaced repetition system. It provides functionality to add, remove, update, archive, search, and manage flashcards. The service also supports tagging of flashcards and provides stats for flashcard performance.

This class uses a `ConcurrentDictionary<Guid, Flashcard>` to store flashcards and a `ReaderWriterLockSlim` for thread-safe operations. The methods include creating, modifying, retrieving, and deleting flashcards, as well as managing their progress over time.

## Methods

### `AddFlashcard(string question, string answer)`

Adds a new flashcard with the given question and answer.

- **Parameters**:
  - `question`: The question for the flashcard.
  - `answer`: The answer to the flashcard.

- **Returns**: `OperationResult` indicating the success or failure of the operation.

### `AddFlashcard(Flashcard flashcard)`

Adds an existing flashcard object to the collection.

- **Parameters**:
  - `flashcard`: The `Flashcard` object to be added.

- **Returns**: `OperationResult` indicating the success or failure of the operation.

### `GetNextDueCard()`

Retrieves the next due flashcard that needs to be reviewed based on the spaced repetition algorithm.

- **Returns**: The `Flashcard` that is next due or `null` if none is found.

### `UpdateCardProgress(Guid cardId, bool wasCorrect)`

Updates the progress of a flashcard based on whether the user answered correctly or not. Adjusts the `EasinessFactor`, `Repetitions`, and `NextDueDate` according to the spaced repetition algorithm.

- **Parameters**:
  - `cardId`: The `Guid` of the flashcard to update.
  - `wasCorrect`: Boolean indicating if the answer was correct.

- **Returns**: `OperationResult` indicating success or failure.

### `GetAllFlashcards(bool includeArchived = false)`

Gets all flashcards, optionally including archived ones.

- **Parameters**:
  - `includeArchived`: Boolean flag to include archived flashcards in the result. Default is `false`.

- **Returns**: A list of `Flashcard` objects.

### `GetFlashcardById(Guid id)`

Gets a flashcard by its unique identifier.

- **Parameters**:
  - `id`: The `Guid` of the flashcard.

- **Returns**: The `Flashcard` with the given `Guid` or `null` if not found.

### `RemoveFlashcard(Guid id)`

Removes a flashcard from the collection.

- **Parameters**:
  - `id`: The `Guid` of the flashcard to remove.

- **Returns**: `OperationResult` indicating success or failure.

### `ArchiveFlashcard(Guid id, bool archive)`

Archives or unarchives a flashcard.

- **Parameters**:
  - `id`: The `Guid` of the flashcard.
  - `archive`: A boolean indicating whether to archive (`true`) or unarchive (`false`) the flashcard.

- **Returns**: `OperationResult` indicating success or failure.

### `GetStats()`

Gets statistics about the current flashcards, including the total number, active, archived, and due cards, as well as average easiness factor and repetitions.

- **Returns**: A `FlashcardStats` object containing the statistics.

### `SearchFlashcards(string searchText, bool searchInAnswers = true)`

Searches for flashcards based on the given text. You can specify whether to search in the answer or not.

- **Parameters**:
  - `searchText`: The text to search for in the flashcards.
  - `searchInAnswers`: Boolean flag to indicate if the answer should also be searched. Default is `true`.

- **Returns**: A list of `Flashcard` objects that match the search criteria.

### `AddTag(Guid cardId, string tag)`

Adds a tag to a flashcard.

- **Parameters**:
  - `cardId`: The `Guid` of the flashcard to add the tag to.
  - `tag`: The tag to add.

- **Returns**: `OperationResult` indicating success or failure.

### `RemoveTag(Guid cardId, string tag)`

Removes a tag from a flashcard.

- **Parameters**:
  - `cardId`: The `Guid` of the flashcard to remove the tag from.
  - `tag`: The tag to remove.

- **Returns**: `OperationResult` indicating success or failure.

## Classes

### `Flashcard`

Represents a flashcard in the system. The following properties are available:

- `Id`: The unique identifier of the flashcard.
- `Question`: The question on the flashcard.
- `Answer`: The answer to the flashcard.
- `NextDueDate`: The date when the flashcard should next be reviewed.
- `CreatedDate`: The date when the flashcard was created.
- `LastModifiedDate`: The last date the flashcard was modified.
- `EasinessFactor`: The easiness factor for spaced repetition.
- `Repetitions`: The number of repetitions for the flashcard.
- `IsArchived`: Boolean indicating if the flashcard is archived.
- `Tags`: A list of tags associated with the flashcard.

### `FlashcardStats`

Contains statistics for flashcards, including:

- `TotalCount`: The total number of flashcards.
- `ActiveCount`: The number of active flashcards.
- `ArchivedCount`: The number of archived flashcards.
- `DueCount`: The number of flashcards due for review.
- `AverageEasiness`: The average easiness factor across all flashcards.
- `AverageRepetitions`: The average number of repetitions across all flashcards.
- `TagsDistribution`: A dictionary containing the distribution of tags across flashcards.

### `OperationResult`

Represents the result of an operation, including:

- `Success`: A boolean indicating if the operation was successful.
- `Message`: A message providing additional details about the result.
- `Data`: Any additional data (such as the flashcard ID) related to the operation.

## Thread Safety

The `FlashcardService` class uses a `ReaderWriterLockSlim` to ensure thread safety when reading and writing flashcard data. The service allows multiple threads to read concurrently but ensures exclusive access during write operations (e.g., adding or modifying flashcards).

## Usage Example

```csharp
var service = new FlashcardService();

// Adding a flashcard
var result = service.AddFlashcard("What is the capital of France?", "Paris");
if (result.Success)
{
    Console.WriteLine("Flashcard added successfully!");
}
else
{
    Console.WriteLine($"Error: {result.Message}");
}

// Getting the next due card
var nextCard = service.GetNextDueCard();
if (nextCard != null)
{
    Console.WriteLine($"Next due card: {nextCard.Question}");
}

// Updating card progress
var updateResult = service.UpdateCardProgress(nextCard.Id, true);
Console.WriteLine(updateResult.Success ? "Progress updated!" : $"Error: {updateResult.Message}");
```
