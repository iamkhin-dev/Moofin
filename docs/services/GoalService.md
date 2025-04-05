# GoalService

## Index

1. [Overview](#overview)
2. [Methods](#methods)
   - [AddGoal](#addgoalstudygoal-goal)
   - [RemoveGoal](#removegoalguid-goalid)
   - [GetGoal](#getgoalguid-goalid)
   - [GetAllGoals](#getallgoals)
   - [UpdateGoal](#updategoalstudygoal-updatedgoal)
   - [CheckGoalProgress](#checkgoalprogresstimemanager-timemanager)
   - [GetAchievedGoals](#getachievedgoals)
   - [GetPendingGoals](#getpendinggoals)
   - [ClearAllGoals](#clearallgoals)
   - [HasGoal](#hasgoalguid-goalid)
   - [GetGoalCount](#getgoalcount)
   - [GetExpiredGoals](#getexpiredgoals)
   - [GetActiveGoals](#getactivegoals)
   - [GetActiveAndExpiredGoals](#getactiveandexpiredgoals)
3. [Classes](#classes)
   - [StudyGoal](#studygoal)
   - [TimeManager](#timemanager)

## Overview

The `GoalService` class is responsible for managing study goals. It provides functionality to add, remove, update, retrieve, and check the progress of goals. The service allows users to track whether their study goals have been achieved, pending, expired, or active.

This class uses an in-memory list (`List<StudyGoal>`) to store the goals and offers various methods to manipulate and query them.

## Methods

### AddGoal(StudyGoal goal)

This method adds a new study goal to the service. If the goal already has a `Guid.Empty` ID, a new ID will be generated.

**Parameters:**

- `goal` (StudyGoal): The study goal to be added.

**Returns:**

- `void`: No return value.

---

### RemoveGoal(Guid goalId)

This method removes a study goal based on the specified `goalId`.

**Parameters:**

- `goalId` (Guid): The unique identifier of the goal to be removed.

**Returns:**

- `bool`: `true` if the goal was successfully removed; otherwise, `false`.

---

### GetGoal(Guid goalId)

This method retrieves a study goal by its unique identifier.

**Parameters:**

- `goalId` (Guid): The unique identifier of the goal to retrieve.

**Returns:**

- `StudyGoal?`: The study goal if found, or `null` if no goal with the specified ID exists.

---

### GetAllGoals()

This method retrieves all study goals currently stored in the service.

**Returns:**

- `IReadOnlyCollection<StudyGoal>`: A collection of all study goals.

---

### UpdateGoal(StudyGoal updatedGoal)

This method updates an existing study goal. It replaces the existing goal with the specified `updatedGoal`.

**Parameters:**

- `updatedGoal` (StudyGoal): The updated study goal.

**Returns:**

- `bool`: `true` if the goal was successfully updated; otherwise, `false`.

---

### CheckGoalProgress(TimeManager timeManager)

This method checks the progress of each goal and marks them as achieved if the total study time for the day has met the target hours.

**Parameters:**

- `timeManager` (TimeManager): The `TimeManager` instance used to calculate the total study time for today.

**Returns:**

- `void`: No return value.

---

### GetAchievedGoals()

This method retrieves all goals that have been marked as achieved.

**Returns:**

- `IReadOnlyCollection<StudyGoal>`: A collection of achieved goals.

---

### GetPendingGoals()

This method retrieves all goals that are not yet achieved.

**Returns:**

- `IReadOnlyCollection<StudyGoal>`: A collection of pending goals.

---

### ClearAllGoals()

This method clears all goals stored in the service.

**Returns:**

- `void`: No return value.

---

### HasGoal(Guid goalId)

This method checks if a goal with the specified `goalId` exists.

**Parameters:**

- `goalId` (Guid): The unique identifier of the goal to check.

**Returns:**

- `bool`: `true` if the goal exists; otherwise, `false`.

---

### GetGoalCount()

This method retrieves the total number of study goals stored in the service.

**Returns:**

- `int`: The count of study goals.

---

### GetExpiredGoals()

This method retrieves all goals that have expired (i.e., the deadline has passed and the goal is not achieved).

**Returns:**

- `IReadOnlyCollection<StudyGoal>`: A collection of expired goals.

---

### GetActiveGoals()

This method retrieves all goals that are still active (i.e., the goal is not achieved and the deadline has not passed).

**Returns:**

- `IReadOnlyCollection<StudyGoal>`: A collection of active goals.

---

### GetActiveAndExpiredGoals()

This method retrieves a tuple containing both active and expired goals.

**Returns:**

- `(IReadOnlyCollection<StudyGoal> active, IReadOnlyCollection<StudyGoal> expired)`: A tuple of active and expired goals.

## Classes

### StudyGoal

The `StudyGoal` class represents a study goal, which includes details like the target study hours, deadline, and achievement status.

### TimeManager

The `TimeManager` class helps track the total study time, typically used in conjunction with the goal service to check whether study goals have been achieved.
