using Moofin.Core.Models;
using System;
using System.Collections.Generic;

namespace Moofin.Core.Services
{
    public sealed class TimeManager
    {
        private readonly List<StudySession> _sessions = new();
        private StudySession? _currentSession;

        public void StartSession(string subject, string? topic = null, string? notes = null)
        {
            if (_currentSession != null && !_currentSession.EndTime.HasValue)
            {
                StopCurrentSession();
            }

            _currentSession = new StudySession
            {
                Id = Guid.NewGuid(),
                StartTime = DateTime.UtcNow,
                Subject = subject,
                Topic = topic,
                Notes = notes,
                SessionType = SessionType.Active
            };

            _sessions.Add(_currentSession);
        }

        public void StopCurrentSession()
        {
            if (_currentSession != null)
            {
                _currentSession.EndTime = DateTime.UtcNow;
                _currentSession.Duration = _currentSession.EndTime - _currentSession.StartTime;
                _currentSession = null;
            }
        }

        public void PauseCurrentSession()
        {
            if (_currentSession != null)
            {
                _currentSession.EndTime = DateTime.UtcNow;
                _currentSession.Duration = _currentSession.EndTime - _currentSession.StartTime;
                _currentSession.SessionType = SessionType.Paused;
            }
        }

        public void ResumeSession()
        {
            if (_currentSession != null && _currentSession.SessionType == SessionType.Paused)
            {
                var resumedSession = new StudySession
                {
                    Id = Guid.NewGuid(),
                    StartTime = DateTime.UtcNow,
                    Subject = _currentSession.Subject,
                    Topic = _currentSession.Topic,
                    Notes = _currentSession.Notes,
                    SessionType = SessionType.Active,
                    ParentSessionId = _currentSession.Id
                };

                _currentSession = resumedSession;
                _sessions.Add(_currentSession);
            }
        }

        public TimeSpan GetTotalStudyTimeToday()
        {
            return GetStudyTimeByDate(DateTime.UtcNow.Date);
        }

        public TimeSpan GetStudyTimeByDate(DateTime date)
        {
            double totalSeconds = 0;

            foreach (var s in _sessions)
            {
                if (s.StartTime.Date == date && s.SessionType == SessionType.Active)
                {
                    var endTime = s.EndTime ?? DateTime.UtcNow;
                    totalSeconds += (endTime - s.StartTime).TotalSeconds;
                }
            }

            return TimeSpan.FromSeconds(totalSeconds);
        }

        public TimeSpan GetTotalStudyTimeForSubject(string subject)
        {
            double totalSeconds = 0;

            foreach (var s in _sessions)
            {
                if (string.Equals(s.Subject, subject, StringComparison.OrdinalIgnoreCase) &&
                    s.SessionType == SessionType.Active)
                {
                    var endTime = s.EndTime ?? DateTime.UtcNow;
                    totalSeconds += (endTime - s.StartTime).TotalSeconds;
                }
            }

            return TimeSpan.FromSeconds(totalSeconds);
        }

        public void AddStudyTime(TimeSpan duration, string subject = "Generale", string? notes = null)
        {
            if (duration <= TimeSpan.Zero)
                throw new ArgumentException("La durata deve essere maggiore di zero", nameof(duration));

            _sessions.Add(new StudySession
            {
                Id = Guid.NewGuid(),
                StartTime = DateTime.UtcNow - duration,
                EndTime = DateTime.UtcNow,
                Duration = duration,
                Subject = subject ?? "Generale",
                Notes = notes,
                SessionType = SessionType.Manual
            });
        }

        public IEnumerable<StudySession> GetSessionsBySubject(string subject)
        {
            var result = new List<StudySession>();

            foreach (var s in _sessions)
            {
                if (string.Equals(s.Subject, subject, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(s);
                }
            }

            result.Sort((a, b) => b.StartTime.CompareTo(a.StartTime));
            return result;
        }

        public TimeSpan GetTotalStudyTimeThisWeek()
        {
            var startOfWeek = DateTime.UtcNow.Date.AddDays(-(int)DateTime.UtcNow.DayOfWeek);
            double totalSeconds = 0;

            foreach (var s in _sessions)
            {
                if (s.StartTime >= startOfWeek && s.SessionType == SessionType.Active)
                {
                    var endTime = s.EndTime ?? DateTime.UtcNow;
                    totalSeconds += (endTime - s.StartTime).TotalSeconds;
                }
            }

            return TimeSpan.FromSeconds(totalSeconds);
        }

        public bool IsGoalOnTrack(StudyGoal goal)
        {
            var timeStudied = GetTotalStudyTimeForSubject(goal.Title);
            var totalDays = (goal.Deadline - DateTime.UtcNow.Date).TotalDays;
            var timeRequiredPerDay = TimeSpan.FromHours(goal.TargetHours) / totalDays;

            return timeStudied >= timeRequiredPerDay * DateTime.UtcNow.TimeOfDay.TotalHours / 24;
        }

        public Dictionary<string, double> GetSubjectEfficiency()
        {
            var map = new Dictionary<string, List<double>>();

            foreach (var s in _sessions)
            {
                if (s.SessionType == SessionType.Active && s.Duration.HasValue)
                {
                    double value = s.Notes?.Length / s.Duration.Value.TotalHours ?? 0;

                    if (!map.ContainsKey(s.Subject))
                    {
                        map[s.Subject] = new List<double>();
                    }

                    map[s.Subject].Add(value);
                }
            }

            var result = new Dictionary<string, double>();
            foreach (var pair in map)
            {
                double sum = 0;
                foreach (var v in pair.Value) sum += v;
                result[pair.Key] = pair.Value.Count > 0 ? sum / pair.Value.Count : 0;
            }

            return result;
        }

        public void ResetAllSessions()
        {
            _sessions.Clear();
            _currentSession = null;
        }

        public void MergeSessions(TimeSpan maxGap)
        {
            var sessionsToMerge = new List<StudySession>();

            foreach (var s in _sessions)
            {
                if (s.SessionType == SessionType.Active)
                {
                    sessionsToMerge.Add(s);
                }
            }

            sessionsToMerge.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));

            for (int i = 0; i < sessionsToMerge.Count - 1; i++)
            {
                var current = sessionsToMerge[i];
                var next = sessionsToMerge[i + 1];

                if ((next.StartTime - (current.EndTime ?? current.StartTime)) <= maxGap)
                {
                    current.EndTime = next.EndTime;
                    current.Duration = (current.EndTime ?? DateTime.UtcNow) - current.StartTime;
                    sessionsToMerge.RemoveAt(i + 1);
                    i--;
                }
            }
        }

        public IEnumerable<StudySession> GetRecentSessions(int count)
        {
            var sorted = new List<StudySession>(_sessions);
            sorted.Sort((a, b) => b.StartTime.CompareTo(a.StartTime));

            return sorted.GetRange(0, Math.Min(count, sorted.Count));
        }

        public Dictionary<string, TimeSpan> GetStudyTimeBySubject()
        {
            var map = new Dictionary<string, double>();

            foreach (var s in _sessions)
            {
                if (s.SessionType == SessionType.Active)
                {
                    var endTime = s.EndTime ?? DateTime.UtcNow;
                    var seconds = (endTime - s.StartTime).TotalSeconds;

                    if (!map.ContainsKey(s.Subject))
                        map[s.Subject] = 0;

                    map[s.Subject] += seconds;
                }
            }

            var result = new Dictionary<string, TimeSpan>();
            foreach (var pair in map)
            {
                result[pair.Key] = TimeSpan.FromSeconds(pair.Value);
            }

            return result;
        }

        public IEnumerable<StudySession> GetTodaySessions()
        {
            var today = DateTime.UtcNow.Date;
            var result = new List<StudySession>();

            foreach (var s in _sessions)
            {
                if (s.StartTime.Date == today)
                    result.Add(s);
            }

            result.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));
            return result;
        }

        public IEnumerable<StudySession> GetSessionsByPeriod(DateTime start, DateTime end)
        {
            var result = new List<StudySession>();

            foreach (var s in _sessions)
            {
                if (s.StartTime >= start && s.StartTime <= end)
                    result.Add(s);
            }

            result.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));
            return result;
        }

        public StudySession? GetCurrentSession()
        {
            return _currentSession;
        }

        public bool HasActiveSession()
        {
            return _currentSession != null;
        }

        public void AddManualSession(DateTime start, DateTime end, string subject, string? notes = null)
        {
            if (start >= end)
                throw new ArgumentException("End time must be after start time");

            _sessions.Add(new StudySession
            {
                Id = Guid.NewGuid(),
                StartTime = start,
                EndTime = end,
                Subject = subject,
                Notes = notes,
                Duration = end - start,
                SessionType = SessionType.Manual
            });
        }

        public void UpdateSession(Guid sessionId, Action<StudySession> updateAction)
        {
            for (int i = 0; i < _sessions.Count; i++)
            {
                if (_sessions[i].Id == sessionId)
                {
                    updateAction(_sessions[i]);

                    if (_sessions[i].EndTime.HasValue)
                    {
                        _sessions[i].Duration = _sessions[i].EndTime - _sessions[i].StartTime;
                    }

                    break;
                }
            }
        }

        public bool DeleteSession(Guid sessionId)
        {
            for (int i = 0; i < _sessions.Count; i++)
            {
                if (_sessions[i].Id == sessionId)
                {
                    if (_currentSession != null && _currentSession.Id == sessionId)
                    {
                        _currentSession = null;
                    }

                    _sessions.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public (TimeSpan totalTime, int sessionCount) GetWeeklyStats()
        {
            var startOfWeek = DateTime.UtcNow.Date.AddDays(-(int)DateTime.UtcNow.DayOfWeek);
            var end = DateTime.UtcNow;

            var sessionList = new List<StudySession>();
            foreach (var s in _sessions)
            {
                if (s.StartTime >= startOfWeek && s.StartTime <= end && s.SessionType == SessionType.Active)
                    sessionList.Add(s);
            }

            double totalSeconds = 0;
            foreach (var s in sessionList)
            {
                var endTime = s.EndTime ?? DateTime.UtcNow;
                totalSeconds += (endTime - s.StartTime).TotalSeconds;
            }

            return (TimeSpan.FromSeconds(totalSeconds), sessionList.Count);
        }

        public double GetAverageDailyStudyTime(int? daysToConsider = null)
        {
            var relevantSessions = new List<StudySession>();
            var minDate = daysToConsider.HasValue ? DateTime.UtcNow.Date.AddDays(-daysToConsider.Value) : DateTime.MinValue;

            foreach (var s in _sessions)
            {
                if (s.SessionType == SessionType.Active && s.StartTime >= minDate)
                {
                    relevantSessions.Add(s);
                }
            }

            var activeDays = new HashSet<DateTime>();
            foreach (var s in relevantSessions)
            {
                activeDays.Add(s.StartTime.Date);
            }

            if (activeDays.Count == 0) return 0;

            double totalSeconds = 0;
            foreach (var s in relevantSessions)
            {
                var endTime = s.EndTime ?? DateTime.UtcNow;
                totalSeconds += (endTime - s.StartTime).TotalSeconds;
            }

            return totalSeconds / activeDays.Count;
        }

        public IEnumerable<(DateTime Date, TimeSpan StudyTime)> GetDailyStudyTimeTrend(int days)
        {
            var trend = new List<(DateTime, TimeSpan)>();
            var today = DateTime.UtcNow.Date;

            for (int i = 0; i <= days; i++)
            {
                var date = today.AddDays(-i);
                var time = GetStudyTimeByDate(date);
                trend.Add((date, time));
            }

            trend.Reverse();
            return trend;
        }

        public (TimeSpan completed, TimeSpan remaining) GetGoalProgress(StudyGoal goal)
        {
            var completed = GetTotalStudyTimeForSubject(goal.Title);
            var target = TimeSpan.FromHours(goal.TargetHours);
            var remaining = completed >= target ? TimeSpan.Zero : target - completed;

            return (completed, remaining);
        }

        public double GetGoalCompletionPercentage(StudyGoal goal)
        {
            if (goal.TargetHours <= 0) return 0;

            var studied = GetTotalStudyTimeForSubject(goal.Title).TotalHours;
            return Math.Min(100, studied / goal.TargetHours * 100);
        }
    }
}