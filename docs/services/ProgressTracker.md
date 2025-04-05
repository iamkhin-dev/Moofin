# ProgressTracker 

## Index

1. [Overview](#overview)
2. [Methods](#methods)
   - [GetWeeklyStudyTime](#getweeklystudytime)
   - [GetMonthlyStudyTime](#getmonthlystudytime)
   - [GetStudyTimeBySubject](#getstudytimebysubjectstring-subject)
   - [GetStudyTimeByAllSubjects](#getstudytimebyallsubjects)
   - [CalculateTotalTime](#calculatetotaltimeienumerablestudysession-sessions)
   - [CalculateFlashcardMastery](#calculateflashcardmastery)
   - [GetFlashcardRetentionRate](#getflashcardretentionrate)
   - [GetFlashcardPerformanceByCategory](#getflashcardperformancebycategory)
   - [GenerateProgressReport](#generateprogressreport)
   - [GetStudyTrends](#getstudytrendsint-weeks-4)
   - [CheckGoalProgress](#checkgoalprogressstudygoal-goal)
3. [Classes](#classes)
   - [ProgressReport](#progressreport)
   - [StudyTrends](#studytrends)
   - [WeeklyAverage](#weeklyaverage)
   - [GoalProgress](#goalprogress)

## Overview

The `ProgressTracker` class is used to track study progress, including study sessions and flashcard mastery. It provides various methods to calculate study time, track flashcard performance, and generate reports on study trends over time. The class works with lists of `StudySession` and `Flashcard` objects to analyze study habits and achievements.

## Methods

### GetWeeklyStudyTime()

This method calculates the total study time from the past week.

**Returns:**

- `TimeSpan`: The total time spent studying over the last 7 days.

---

### GetMonthlyStudyTime()

This method calculates the total study time from the past month.

**Returns:**

- `TimeSpan`: The total time spent studying over the last 30 days.

---

### GetStudyTimeBySubject(string subject)

This method calculates the total study time for a specific subject.

**Parameters:**

- `subject` (string): The subject for which to calculate the total study time.

**Returns:**

- `TimeSpan`: The total time spent studying the specified subject.

---

### GetStudyTimeByAllSubjects()

This method calculates the total study time for each subject.

**Returns:**

- `Dictionary<string, TimeSpan>`: A dictionary where each key is a subject name, and the value is the total study time for that subject.

---

### CalculateTotalTime(IEnumerable<StudySession> sessions)

This method calculates the total time for a given set of study sessions.

**Parameters:**

- `sessions` (IEnumerable<StudySession>): The study sessions to calculate the total time for.

**Returns:**

- `TimeSpan`: The total time for the specified sessions.

---

### CalculateFlashcardMastery()

This method calculates the flashcard mastery percentage, which is the percentage of flashcards that have been mastered (3 or more repetitions).

**Returns:**

- `double`: The flashcard mastery as a percentage.

---

### GetFlashcardRetentionRate()

This method calculates the flashcard retention rate, which is the percentage of flashcards that have been successfully recalled.

**Returns:**

- `double`: The flashcard retention rate as a percentage.

---

### GetFlashcardPerformanceByCategory()

This method calculates the flashcard performance by category, providing a percentage of successful recalls per category.

**Returns:**

- `Dictionary<string, double>`: A dictionary where each key is a flashcard category, and the value is the recall success percentage for that category.

---

### GenerateProgressReport()

This method generates a detailed progress report that includes study time statistics, flashcard mastery, retention rate, and performance by subject/category.

**Returns:**

- `ProgressReport`: A report containing the various progress metrics.

---

### GetStudyTrends(int weeks = 4)

This method calculates study trends for the last specified number of weeks, including daily averages for study time.

**Parameters:**

- `weeks` (int): The number of weeks to analyze (default is 4 weeks).

**Returns:**

- `StudyTrends`: An object containing the daily averages of study time for the specified number of weeks.

---

### CheckGoalProgress(StudyGoal goal)

This method checks the progress of a given study goal, including the time studied, completion percentage, and daily target required to reach the goal by the deadline.

**Parameters:**

- `goal` (StudyGoal): The goal to check progress against.

**Returns:**

- `GoalProgress`: An object containing the progress details of the study goal.

## Classes

### ProgressReport

The `ProgressReport` class contains details about the user's study progress, including weekly and monthly study times, flashcard mastery, retention rates, and subject breakdowns.

### StudyTrends

The `StudyTrends` class contains study trends over a specified period, such as weekly averages.

### WeeklyAverage

The `WeeklyAverage` class contains the average study time for a specific week.

### GoalProgress

The `GoalProgress` class contains the progress details for a specific study goal, including the time studied, completion percentage, and daily target required.

