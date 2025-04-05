# TimeManager 

## Index

1. [Overview](#overview)
2. [Methods](#methods)
   - [StartSession](#startsessionstring-subject-string-topic-null-string-notes-null)
   - [StopCurrentSession](#stopcurrentsession)
   - [PauseCurrentSession](#pausecurrentsession)
   - [ResumeSession](#resumesession)
   - [GetTotalStudyTimeToday](#gettotalstudytimetoday)
   - [GetStudyTimeByDate](#getstudytimebydate-datetime-date)
   - [GetTotalStudyTimeForSubject](#gettotalstudytimeforsubjectstring-subject)
   - [AddStudyTime](#addstudytime-timespan-duration-string-subject-generale-string-notes-null)
   - [GetSessionsBySubject](#getsessionsbysubjectstring-subject)
   - [GetTotalStudyTimeThisWeek](#gettotalstudytimetthisweek)
   - [IsGoalOnTrack](#isgoalontrackstudygoal-goal)
   - [GetSubjectEfficiency](#getsubjectefficiency)
   - [ResetAllSessions](#resetallsessions)
   - [MergeSessions](#mergesessions-timespan-maxgap)
   - [GetRecentSessions](#getrecentsessions-int-count)
   - [GetStudyTimeBySubject](#getstudytimebysubject)
   - [GetTodaySessions](#gettodaysessions)
   - [GetSessionsByPeriod](#getsessionsbyperiod-datetime-start-datetime-end)
   - [GetCurrentSession](#getcurrentsession)
   - [HasActiveSession](#hasactivesession)
   - [AddManualSession](#addmanualsession-datetime-start-datetime-end-string-subject-string-notes-null)
   - [UpdateSession](#updatesession-guid-sessionid-action-updatesession)
   - [DeleteSession](#deletesession-guid-sessionid)
   - [GetWeeklyStats](#getweeklystats)
   - [GetAverageDailyStudyTime](#getaveragedailystudytime-int-days-to-consider-null)
   - [GetDailyStudyTimeTrend](#getdailystudytimetrend-int-days)
   - [GetGoalProgress](#getgoalprogress-studygoal-goal)
   - [GetGoalCompletionPercentage](#getgoalcompletionpercentage-studygoal-goal)
3. [Classes](#classes)
   - [StudySession](#studysession)

## Overview

The `TimeManager` class is responsible for managing study sessions, including tracking their start, stop, pause, and resume times. It provides functionality to calculate total study time for specific days, subjects, or weeks, add manual study sessions, and merge sessions. It also allows for tracking study goals and efficiency, as well as providing trends and progress reports.

## Methods

### StartSession(string subject, string? topic = null, string? notes = null)

This method starts a new study session.

**Parameters:**

- `subject` (string): The subject of the session.
- `topic` (string?): An optional topic for the session.
- `notes` (string?): Optional notes for the session.

---

### StopCurrentSession()

This method stops the current active session.

---

### PauseCurrentSession()

This method pauses the current active session.

---

### ResumeSession()

This method resumes a paused session, starting a new session from the point where it was paused.

---

### GetTotalStudyTimeToday()

This method retrieves the total study time for today.

**Returns:**

- `TimeSpan`: The total study time for today.

---

### GetStudyTimeByDate(DateTime date)

This method calculates the total study time for a given date.

**Parameters:**

- `date` (DateTime): The date for which to calculate the total study time.

**Returns:**

- `TimeSpan`: The total study time for the specified date.

---

### GetTotalStudyTimeForSubject(string subject)

This method calculates the total study time for a given subject.

**Parameters:**

- `subject` (string): The subject for which to calculate the total study time.

**Returns:**

- `TimeSpan`: The total study time for the specified subject.

---

### AddStudyTime(TimeSpan duration, string subject = "Generale", string? notes = null)

This method allows you to manually add study time.

**Parameters:**

- `duration` (TimeSpan): The duration of the study session.
- `subject` (string): The subject for the session (default is "Generale").
- `notes` (string?): Optional notes for the session.

---

### GetSessionsBySubject(string subject)

This method retrieves all sessions for a given subject.

**Parameters:**

- `subject` (string): The subject for which to retrieve the sessions.

**Returns:**

- `IEnumerable<StudySession>`: A list of study sessions for the specified subject.

---

### GetTotalStudyTimeThisWeek()

This method retrieves the total study time for the current week.

**Returns:**

- `TimeSpan`: The total study time for the current week.

---

### IsGoalOnTrack(StudyGoal goal)

This method checks if the study goal is on track based on the time studied.

**Parameters:**

- `goal` (StudyGoal): The goal to check progress for.

**Returns:**

- `bool`: `true` if the goal is on track, otherwise `false`.

---

### GetSubjectEfficiency()

This method calculates the efficiency for each subject, based on the ratio of notes length to session duration.

**Returns:**

- `Dictionary<string, double>`: A dictionary where the key is the subject and the value is the efficiency ratio.

---

### ResetAllSessions()

This method resets all study sessions.

---

### MergeSessions(TimeSpan maxGap)

This method merges sessions that have a gap between them smaller than the specified maximum gap.

**Parameters:**

- `maxGap` (TimeSpan): The maximum gap between sessions for them to be merged.

---

### GetRecentSessions(int count)

This method retrieves the most recent sessions.

**Parameters:**

- `count` (int): The number of recent sessions to retrieve.

**Returns:**

- `IEnumerable<StudySession>`: A list of the most recent sessions.

---

### GetStudyTimeBySubject()

This method calculates the total study time for each subject.

**Returns:**

- `Dictionary<string, TimeSpan>`: A dictionary where each key is a subject and the value is the total study time for that subject.

---

### GetTodaySessions()

This method retrieves all sessions for today.

**Returns:**

- `IEnumerable<StudySession>`: A list of all sessions for today.

---

### GetSessionsByPeriod(DateTime start, DateTime end)

This method retrieves all sessions within a specific time period.

**Parameters:**

- `start` (DateTime): The start of the time period.
- `end` (DateTime): The end of the time period.

**Returns:**

- `IEnumerable<StudySession>`: A list of sessions within the specified period.

---

### GetCurrentSession()

This method retrieves the current active session, if any.

**Returns:**

- `StudySession?`: The current session, or `null` if no active session exists.

---

### HasActiveSession()

This method checks if there is currently an active session.

**Returns:**

- `bool`: `true` if there is an active session, otherwise `false`.

---

### AddManualSession(DateTime start, DateTime end, string subject, string? notes = null)

This method allows you to manually add a study session by specifying the start and end times.

**Parameters:**

- `start` (DateTime): The start time of the session.
- `end` (DateTime): The end time of the session.
- `subject` (string): The subject of the session.
- `notes` (string?): Optional notes for the session.

---

### UpdateSession(Guid sessionId, Action<StudySession> updateAction)

This method allows you to update an existing session.

**Parameters:**

- `sessionId` (Guid): The ID of the session to update.
- `updateAction` (Action<StudySession>): An action that performs the update on the session.

---

### DeleteSession(Guid sessionId)

This method deletes a session by its ID.

**Parameters:**

- `sessionId` (Guid): The ID of the session to delete.

**Returns:**

- `bool`: `true` if the session was deleted, otherwise `false`.

---

### GetWeeklyStats()

This method retrieves the total study time and the number of sessions for the current week.

**Returns:**

- `(TimeSpan totalTime, int sessionCount)`: A tuple containing the total time and the session count.

---

### GetAverageDailyStudyTime(int? daysToConsider = null)

This method calculates the average daily study time, considering a specific number of days.

**Parameters:**

- `daysToConsider` (int?): The number of days to consider (optional).

**Returns:**

- `double`: The average daily study time in seconds.

---

### GetDailyStudyTimeTrend(int days)

This method retrieves the daily study time trend for the specified number of days.

**Parameters:**

- `days` (int): The number of days to consider for the trend.

**Returns:**

- `IEnumerable<(DateTime, TimeSpan)>`: A list of tuples containing the date and the study time for that day.

---

### GetGoalProgress(StudyGoal goal)

This method calculates the progress of a given study goal.

**Parameters:**

- `goal` (StudyGoal): The goal to check progress for.

**Returns:**

- `(TimeSpan completed, TimeSpan remaining)`: A tuple containing the completed and remaining time for the goal.

---

### GetGoalCompletionPercentage(StudyGoal goal)

This method calculates the percentage of completion for a given study goal.

**Parameters:**

- `goal` (StudyGoal): The goal to check the completion percentage for.

**Returns:**

- `double`: The completion percentage of the goal.

---

## Classes

### StudySession

A class that represents a study session, containing details such as the start time, end time, duration, subject, and notes.
