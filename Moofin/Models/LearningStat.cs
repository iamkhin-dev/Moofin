using System;

namespace Moofin.Core.Models
{
    public sealed class LearningStat
    {
        public TimeSpan TotalTime { get; set; }

        public double Accuracy { get; set; }

        public double MasteryPercentage { get; set; }

        public DateTime PeriodStart { get; set; }

        public DateTime PeriodEnd { get; set; }

        public int CompletedSessions { get; set; }
    }
}