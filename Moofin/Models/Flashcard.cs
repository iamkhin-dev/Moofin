using System;
using System.Collections.Generic;

namespace Moofin.Core.Models
{
    public sealed class Flashcard
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // Aggiunto per supportare GetFlashcardPerformanceByCategory
        public int Repetitions { get; set; } = 0;
        public double EasinessFactor { get; set; } = 2.5;
        public DateTime NextDueDate { get; set; } = DateTime.UtcNow;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;
        public DateTime LastReviewedDate { get; set; } = DateTime.UtcNow; // Aggiunto per tracciare l'ultima revisione
        public bool LastRecallSuccessful { get; set; } = false; // Aggiunto per supportare GetFlashcardRetentionRate
        public List<string> Tags { get; set; } = new List<string>();
        public bool IsArchived { get; set; } = false;
        public int Interval { get; set; } = 1; // Aggiunto per l'algoritmo di ripetizione spaziata
        public double PerformanceScore { get; set; } = 0; // Aggiunto per tracciare le prestazioni

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Question) &&
                   !string.IsNullOrWhiteSpace(Answer) &&
                   EasinessFactor >= 1.3 &&
                   EasinessFactor <= 2.5 &&
                   NextDueDate >= CreatedDate;
        }

        public void UpdateRecallPerformance(bool recallSuccessful)
        {
            LastRecallSuccessful = recallSuccessful;
            LastReviewedDate = DateTime.UtcNow;

            if (recallSuccessful)
            {
                Repetitions++;
                PerformanceScore = Math.Min(100, PerformanceScore + 15);
            }
            else
            {
                PerformanceScore = Math.Max(0, PerformanceScore - 20);
            }
        }
    }

    public sealed class FlashcardStats
    {
        public int TotalCount { get; set; }
        public int ActiveCount { get; set; }
        public int DueCount { get; set; }
        public int ArchivedCount { get; set; }
        public int NewCount { get; set; }
        public int LearningCount { get; set; }
        public int ReviewCount { get; set; }
        public double AverageEasiness { get; set; }
        public double AverageRepetitions { get; set; }
        public double AveragePerformance { get; set; }
        public double RetentionRate { get; set; }
        public Dictionary<string, int> TagsDistribution { get; set; } = new();
        public Dictionary<string, int> CategoriesDistribution { get; set; } = new();
    }

    public sealed class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? EntityId { get; set; }
        public Flashcard? Flashcard { get; set; }

        public static OperationResult SuccessResult(Guid? id = null, string message = "", Flashcard? flashcard = null)
            => new() { Success = true, EntityId = id, Message = message, Flashcard = flashcard };

        public static OperationResult Failure(string message)
            => new() { Success = false, Message = message };
    }
}