using System;
using Moofin.Core.Models;

public sealed class StudySession
{
    public Guid Id { get; set; }
    public Guid? ParentSessionId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public TimeSpan? Duration { get; set; }
    public string Subject { get; set; } = "General";
    public string? Topic { get; set; } 
    public string? Notes { get; set; }
    public Guid? GoalId { get; set; }
    public SessionType SessionType { get; set; }
}