# SpacedRepetition

## Index

1. [Overview](#overview)
2. [Methods](#methods)
   - [Calculate(int repetitions, double easinessFactor, bool wasCorrect)](#calculateint-repetitions-double-easinessfactor-bool-wascorrect)
3. [Example Usage](#example-usage)

## Overview

The `SpacedRepetition` class provides a static method for calculating the next review interval and easiness factor in a spaced repetition algorithm. This algorithm is often used in flashcard applications to help optimize learning by adjusting review intervals based on how well the learner remembers the material.

The `Calculate` method takes in the current number of repetitions, the current easiness factor, and whether the learner's response was correct or not. Based on these inputs, it returns the next interval (in days) for the review and the updated easiness factor.

## Methods

### Calculate(int repetitions, double easinessFactor, bool wasCorrect)

Calculates the next review interval and the updated easiness factor based on the spaced repetition algorithm.

**Parameters:**

- `repetitions`: An integer representing the number of times the learner has successfully recalled the material. This is used to determine the review interval.
- `easinessFactor`: A double representing the current easiness factor, which determines how quickly the intervals between reviews increase.
- `wasCorrect`: A boolean value indicating whether the learner's response was correct. If the response was incorrect, the easiness factor will be reduced.

**Returns:**

- A tuple `(int interval, double easinessFactor)`:
  - `interval`: The next review interval (in days) calculated based on the number of repetitions and easiness factor.
  - `easinessFactor`: The updated easiness factor.

**Example Usage:**

```csharp
using Moofin.Core.Utils;

public class Example
{
    public void CalculateReviewInterval()
    {
        int repetitions = 3;  // The learner has recalled the material correctly 3 times
        double easinessFactor = 2.5;  // The current easiness factor
        bool wasCorrect = true;  // The learner recalled the material correctly

        var (interval, newEasinessFactor) = SpacedRepetition.Calculate(repetitions, easinessFactor, wasCorrect);

        Console.WriteLine($"Next review in {interval} days with easiness factor: {newEasinessFactor}");
    }
}
