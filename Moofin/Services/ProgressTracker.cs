using Moofin.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Moofin.Core.Services
{
    public sealed class ProgressTracker
    {
        private readonly List<StudySession> _sessions;
        private readonly List<Flashcard> _flashcards;

        public ProgressTracker(List<StudySession> sessions, List<Flashcard> flashcards)
        {
            _sessions = sessions ?? new List<StudySession>();
            _flashcards = flashcards ?? new List<Flashcard>();
        }

        public TimeSpan GetWeeklyStudyTime()
        {
            var oneWeekAgo = DateTime.UtcNow.AddDays(-7);
            return CalculateTotalTime(_sessions.Where(s => s.StartTime >= oneWeekAgo));
        }

        public TimeSpan GetMonthlyStudyTime()
        {
            var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);
            return CalculateTotalTime(_sessions.Where(s => s.StartTime >= oneMonthAgo));
        }

        public TimeSpan GetStudyTimeBySubject(string subject)
        {
            return CalculateTotalTime(_sessions.Where(s =>
                s.Subject.Equals(subject, StringComparison.OrdinalIgnoreCase)));
        }

        public Dictionary<string, TimeSpan> GetStudyTimeByAllSubjects()
        {
            return _sessions
                .GroupBy(s => s.Subject)
                .ToDictionary(
                    g => g.Key,
                    g => CalculateTotalTime(g.AsEnumerable())
                );
        }

        private TimeSpan CalculateTotalTime(IEnumerable<StudySession> sessions)
        {
            return sessions.Aggregate(TimeSpan.Zero, (total, session) =>
                total + ((session.EndTime ?? DateTime.UtcNow) - session.StartTime));
        }

        public double CalculateFlashcardMastery()
        {
            if (!_flashcards.Any()) return 0;

            int masteredCards = _flashcards.Count(c => c.Repetitions >= 3);
            return (masteredCards / (double)_flashcards.Count) * 100;
        }

        public double GetFlashcardRetentionRate()
        {
            if (!_flashcards.Any()) return 0;

            int rememberedCards = _flashcards.Count(c => c.LastRecallSuccessful);
            return (rememberedCards / (double)_flashcards.Count) * 100;
        }

        public Dictionary<string, double> GetFlashcardPerformanceByCategory()
        {
            return _flashcards
                .GroupBy(f => f.Category)
                .ToDictionary(
                    g => g.Key ?? "Uncategorized",
                    g => g.Count(c => c.LastRecallSuccessful) / (double)g.Count() * 100
                );
        }

        public ProgressReport GenerateProgressReport()
        {
            return new ProgressReport
            {
                WeeklyStudyTime = GetWeeklyStudyTime(),
                MonthlyStudyTime = GetMonthlyStudyTime(),
                FlashcardMastery = CalculateFlashcardMastery(),
                RetentionRate = GetFlashcardRetentionRate(),
                SubjectBreakdown = GetStudyTimeByAllSubjects(),
                FlashcardPerformance = GetFlashcardPerformanceByCategory(),
                GeneratedAt = DateTime.UtcNow
            };
        }

        public StudyTrends GetStudyTrends(int weeks = 4)
        {
            var trends = new StudyTrends();
            var endDate = DateTime.UtcNow.Date;
            var startDate = endDate.AddDays(-7 * weeks);

            trends.DailyAverages = Enumerable.Range(0, weeks * 7)
                .GroupBy(d => d / 7)
                .Select(g => new WeeklyAverage
                {
                    WeekNumber = g.Key + 1,
                    AverageStudyTime = TimeSpan.FromHours(
                        g.Select(d => _sessions
                            .Where(s => s.StartTime.Date == endDate.AddDays(-d).Date)
                            .Sum(s => ((s.EndTime ?? DateTime.UtcNow) - s.StartTime).TotalHours)
                        ).Average()
                    )
                }).ToList();

            return trends;
        }

        public GoalProgress CheckGoalProgress(StudyGoal goal)
        {
            var timeStudied = GetStudyTimeBySubject(goal.Subject ?? string.Empty);
            var targetTime = TimeSpan.FromHours(goal.TargetHours);
            var remainingDays = (goal.Deadline - DateTime.UtcNow).TotalDays;

            return new GoalProgress
            {
                Goal = goal,
                TimeStudied = timeStudied,
                CompletionPercentage = Math.Min(100, timeStudied.TotalHours / goal.TargetHours * 100),
                DailyTargetRequired = remainingDays > 0
                    ? TimeSpan.FromHours((goal.TargetHours - timeStudied.TotalHours) / remainingDays)
                    : TimeSpan.Zero
            };
        }
    }

    public sealed class ProgressReport
    {
        public TimeSpan WeeklyStudyTime { get; set; }
        public TimeSpan MonthlyStudyTime { get; set; }
        public double FlashcardMastery { get; set; }
        public double RetentionRate { get; set; }
        public Dictionary<string, TimeSpan> SubjectBreakdown { get; set; } = new();
        public Dictionary<string, double> FlashcardPerformance { get; set; } = new();
        public DateTime GeneratedAt { get; set; }
    }

    public sealed class StudyTrends
    {
        public List<WeeklyAverage> DailyAverages { get; set; } = new();
    }

    public sealed class WeeklyAverage
    {
        public int WeekNumber { get; set; }
        public TimeSpan AverageStudyTime { get; set; }
    }

    public sealed class GoalProgress
    {
        public StudyGoal? Goal { get; set; }
        public TimeSpan TimeStudied { get; set; }
        public double CompletionPercentage { get; set; }
        public TimeSpan DailyTargetRequired { get; set; }
    }
}