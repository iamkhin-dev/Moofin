using Moofin.Core.Models;
using System;
using System.Collections.Generic;

namespace Moofin.Core.Services
{
    public sealed class GoalService
    {
        private readonly List<StudyGoal> _goals = new();

        public void AddGoal(StudyGoal goal)
        {
            ArgumentNullException.ThrowIfNull(goal, nameof(goal));

            goal.Id = goal.Id == Guid.Empty ? Guid.NewGuid() : goal.Id;
            _goals.Add(goal);
        }

        public bool RemoveGoal(Guid goalId)
        {
            for (int i = 0; i < _goals.Count; i++)
            {
                if (_goals[i].Id == goalId)
                {
                    _goals.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public StudyGoal? GetGoal(Guid goalId)
        {
            for (int i = 0; i < _goals.Count; i++)
            {
                if (_goals[i].Id == goalId)
                {
                    return _goals[i];
                }
            }
            return null;
        }

        public IReadOnlyCollection<StudyGoal> GetAllGoals()
        {
            return _goals.AsReadOnly();
        }

        public bool UpdateGoal(StudyGoal updatedGoal)
        {
            ArgumentNullException.ThrowIfNull(updatedGoal, nameof(updatedGoal));

            for (int i = 0; i < _goals.Count; i++)
            {
                if (_goals[i].Id == updatedGoal.Id)
                {
                    _goals[i] = updatedGoal;
                    return true;
                }
            }
            return false;
        }

        public void CheckGoalProgress(TimeManager timeManager)
        {
            ArgumentNullException.ThrowIfNull(timeManager, nameof(timeManager));

            var now = DateTime.UtcNow;
            var totalTime = timeManager.GetTotalStudyTimeToday();

            for (int i = 0; i < _goals.Count; i++)
            {
                var goal = _goals[i];
                if (!goal.IsAchieved && totalTime.TotalHours >= goal.TargetHours && now <= goal.Deadline)
                {
                    goal.MarkAsAchieved();
                }
            }
        }

        public IReadOnlyCollection<StudyGoal> GetAchievedGoals()
        {
            var achieved = new List<StudyGoal>();
            for (int i = 0; i < _goals.Count; i++)
            {
                if (_goals[i].IsAchieved)
                {
                    achieved.Add(_goals[i]);
                }
            }
            return achieved.AsReadOnly();
        }

        public IReadOnlyCollection<StudyGoal> GetPendingGoals()
        {
            var pending = new List<StudyGoal>();
            for (int i = 0; i < _goals.Count; i++)
            {
                if (!_goals[i].IsAchieved)
                {
                    pending.Add(_goals[i]);
                }
            }
            return pending.AsReadOnly();
        }

        public void ClearAllGoals(){ _goals.Clear(); }

        public bool HasGoal(Guid goalId)
        {
            for (int i = 0; i < _goals.Count; i++)
            {
                if (_goals[i].Id == goalId)
                {
                    return true;
                }
            }
            return false;
        }

        public int GetGoalCount(){ return _goals.Count;}

        public IReadOnlyCollection<StudyGoal> GetExpiredGoals()
        {
            var now = DateTime.UtcNow;
            var expired = new List<StudyGoal>();

            for (int i = 0; i < _goals.Count; i++)
            {
                if (!_goals[i].IsAchieved && _goals[i].Deadline < now)
                {
                    expired.Add(_goals[i]);
                }
            }
            return expired.AsReadOnly();
        }

        public IReadOnlyCollection<StudyGoal> GetActiveGoals()
        {
            var now = DateTime.UtcNow;
            var active = new List<StudyGoal>();

            for (int i = 0; i < _goals.Count; i++)
            {
                if (!_goals[i].IsAchieved && _goals[i].Deadline >= now)
                {
                    active.Add(_goals[i]);
                }
            }
            return active.AsReadOnly();
        }

        public (IReadOnlyCollection<StudyGoal> active, IReadOnlyCollection<StudyGoal> expired) GetActiveAndExpiredGoals()
        {
            var now = DateTime.UtcNow;
            var active = new List<StudyGoal>();
            var expired = new List<StudyGoal>();

            for (int i = 0; i < _goals.Count; i++)
            {
                if (!_goals[i].IsAchieved)
                {
                    if (_goals[i].Deadline >= now)
                    {
                        active.Add(_goals[i]);
                    }
                    else
                    {
                        expired.Add(_goals[i]);
                    }
                }
            }
            return (active.AsReadOnly(), expired.AsReadOnly());
        }
    }
}