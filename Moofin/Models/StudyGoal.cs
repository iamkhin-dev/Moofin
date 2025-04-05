using System;

namespace Moofin.Core.Models
{
    public sealed class StudyGoal
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double TargetHours { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsAchieved { get; private set; }
        public string? Subject { get; set; }
        public DateTime? AchievedDate { get; private set; }

        public void MarkAsAchieved()
        {
            if (!IsAchieved)
            {
                IsAchieved = true;
                AchievedDate = DateTime.UtcNow;
            }
        } 
    }
}