using Moofin.Core.Models;
using Moofin.Core.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Moofin.Core.Services
{
    public sealed class FlashcardService
    {
        private readonly ConcurrentDictionary<Guid, Flashcard> _flashcards = new();
        private readonly ReaderWriterLockSlim _lock = new();

        public OperationResult AddFlashcard(string question, string answer)
        {
            question = question?.Trim() ?? string.Empty;
            answer = answer?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(question)) return OperationResult.Failure("Question cannot be empty");
            if (string.IsNullOrWhiteSpace(answer)) return OperationResult.Failure("Answer cannot be empty");

            var flashcard = new Flashcard
            {
                Question = question,
                Answer = answer,
                NextDueDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow
            };

            return AddFlashcard(flashcard);
        }

        public OperationResult AddFlashcard(Flashcard flashcard)
        {
            if (flashcard == null) return OperationResult.Failure("Flashcard cannot be null");

            flashcard.Question = flashcard.Question?.Trim() ?? string.Empty;
            flashcard.Answer = flashcard.Answer?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(flashcard.Question)) return OperationResult.Failure("Question cannot be empty");
            if (string.IsNullOrWhiteSpace(flashcard.Answer)) return OperationResult.Failure("Answer cannot be empty");

            flashcard.Id = flashcard.Id == Guid.Empty ? Guid.NewGuid() : flashcard.Id;
            flashcard.NextDueDate = flashcard.NextDueDate == default ? DateTime.UtcNow : flashcard.NextDueDate;
            flashcard.CreatedDate = flashcard.CreatedDate == default ? DateTime.UtcNow : flashcard.CreatedDate;
            flashcard.LastModifiedDate = DateTime.UtcNow;
            flashcard.EasinessFactor = flashcard.EasinessFactor < 1.3 ? 1.3 : (flashcard.EasinessFactor > 2.5 ? 2.5 : flashcard.EasinessFactor);
            flashcard.Repetitions = flashcard.Repetitions < 0 ? 0 : flashcard.Repetitions;

            List<string> tags = new List<string>();
            if (flashcard.Tags != null)
            {
                foreach (var tag in flashcard.Tags)
                {
                    bool exists = false;
                    foreach (var t in tags)
                    {
                        if (string.Equals(tag, t, StringComparison.OrdinalIgnoreCase))
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists)
                    {
                        tags.Add(tag);
                    }
                }
            }
            flashcard.Tags = tags;

            try
            {
                _lock.EnterWriteLock();

                if (_flashcards.TryAdd(flashcard.Id, flashcard))
                {
                    return OperationResult.SuccessResult(flashcard.Id);
                }

                return OperationResult.Failure("Flashcard already exists");
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public Flashcard? GetNextDueCard()
        {
            try
            {
                _lock.EnterReadLock();
                Flashcard? nextCard = null;

                foreach (var card in _flashcards.Values)
                {
                    if (!card.IsArchived && card.NextDueDate <= DateTime.UtcNow)
                    {
                        if (nextCard == null || card.NextDueDate < nextCard.NextDueDate)
                        {
                            nextCard = card;
                        }
                    }
                }

                return nextCard;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public OperationResult UpdateCardProgress(Guid cardId, bool wasCorrect)
        {
            try
            {
                _lock.EnterUpgradeableReadLock();

                if (!_flashcards.TryGetValue(cardId, out var card)) return OperationResult.Failure("Flashcard not found");

                var (interval, ef) = SpacedRepetition.Calculate(card.Repetitions, card.EasinessFactor, wasCorrect);

                try
                {
                    _lock.EnterWriteLock();

                    card.Repetitions++;
                    card.EasinessFactor = ef;
                    card.NextDueDate = DateTime.UtcNow.AddDays(interval);
                    card.LastModifiedDate = DateTime.UtcNow;

                    return OperationResult.SuccessResult(cardId);
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
            finally
            {
                _lock.ExitUpgradeableReadLock();
            }
        }

        public List<Flashcard> GetAllFlashcards(bool includeArchived = false)
        {
            try
            {
                _lock.EnterReadLock();
                List<Flashcard> result = new List<Flashcard>();

                foreach (var card in _flashcards.Values)
                {
                    if (includeArchived || !card.IsArchived)
                    {
                        result.Add(card);
                    }
                }

                result.Sort((a, b) => b.CreatedDate.CompareTo(a.CreatedDate));
                return result;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public Flashcard? GetFlashcardById(Guid id)
        {
            try
            {
                _lock.EnterReadLock();
                return _flashcards.TryGetValue(id, out var card) ? card : null;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public OperationResult RemoveFlashcard(Guid id)
        {
            try
            {
                _lock.EnterWriteLock();

                if (_flashcards.TryRemove(id, out _))
                {
                    return OperationResult.SuccessResult(id);
                }

                return OperationResult.Failure("Flashcard not found");
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public OperationResult ArchiveFlashcard(Guid id, bool archive)
        {
            try
            {
                _lock.EnterUpgradeableReadLock();

                if (!_flashcards.TryGetValue(id, out var card)) return OperationResult.Failure("Flashcard not found");

                try
                {
                    _lock.EnterWriteLock();
                    card.IsArchived = archive;
                    card.LastModifiedDate = DateTime.UtcNow;
                    return OperationResult.SuccessResult(id);
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
            finally
            {
                _lock.ExitUpgradeableReadLock();
            }
        }

        public FlashcardStats GetStats()
        {
            try
            {
                _lock.EnterReadLock();

                int total = 0, active = 0, archived = 0, due = 0;
                double efSum = 0, repSum = 0;
                Dictionary<string, int> tags = new(StringComparer.OrdinalIgnoreCase);

                foreach (var kvp in _flashcards)
                {
                    var card = kvp.Value;
                    total++;
                    if (card.IsArchived) archived++;
                    else active++;

                    if (!card.IsArchived && card.NextDueDate <= DateTime.UtcNow)
                        due++;

                    efSum += card.EasinessFactor;
                    repSum += card.Repetitions;

                    if (card.Tags != null)
                    {
                        foreach (var tag in card.Tags)
                        {
                            if (!string.IsNullOrWhiteSpace(tag))
                            {
                                if (tags.ContainsKey(tag))
                                    tags[tag]++;
                                else
                                    tags[tag] = 1;
                            }
                        }
                    }
                }

                double averageEf = total > 0 ? efSum / total : 0;
                double averageRep = total > 0 ? repSum / total : 0;

                return new FlashcardStats
                {
                    TotalCount = total,
                    ActiveCount = active,
                    ArchivedCount = archived,
                    DueCount = due,
                    AverageEasiness = averageEf,
                    AverageRepetitions = averageRep,
                    TagsDistribution = tags
                };
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public List<Flashcard> SearchFlashcards(string searchText, bool searchInAnswers = true)
        {
            searchText = searchText?.Trim() ?? string.Empty;

            List<Flashcard> results = new List<Flashcard>();

            if (string.IsNullOrWhiteSpace(searchText)) return results;

            try
            {
                _lock.EnterReadLock();

                foreach (var card in _flashcards.Values)
                {
                    if (!card.IsArchived &&
                        (card.Question != null && card.Question.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (searchInAnswers && card.Answer != null && card.Answer.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)))
                    {
                        results.Add(card);
                    }
                }

                results.Sort((a, b) => b.LastModifiedDate.CompareTo(a.LastModifiedDate));
                return results;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public OperationResult AddTag(Guid cardId, string tag)
        {
            tag = tag?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(tag)) return OperationResult.Failure("Tag cannot be empty");

            try
            {
                _lock.EnterUpgradeableReadLock();

                if (!_flashcards.TryGetValue(cardId, out var card)) return OperationResult.Failure("Flashcard not found");

                try
                {
                    _lock.EnterWriteLock();

                    card.Tags ??= new List<string>();
                    foreach (var existing in card.Tags)
                    {
                        if (string.Equals(existing, tag, StringComparison.OrdinalIgnoreCase))
                        {
                            return OperationResult.Failure("Tag already exists");
                        }
                    }

                    card.Tags.Add(tag);
                    card.LastModifiedDate = DateTime.UtcNow;

                    return OperationResult.SuccessResult(cardId);
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
            finally
            {
                _lock.ExitUpgradeableReadLock();
            }
        }

        public OperationResult RemoveTag(Guid cardId, string tag)
        {
            tag = tag?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(tag)) return OperationResult.Failure("Tag cannot be empty");

            try
            {
                _lock.EnterUpgradeableReadLock();

                if (!_flashcards.TryGetValue(cardId, out var card)) return OperationResult.Failure("Flashcard not found");

                try
                {
                    _lock.EnterWriteLock();

                    bool removed = false;
                    if (card.Tags != null)
                    {
                        for (int i = card.Tags.Count - 1; i >= 0; i--)
                        {
                            if (string.Equals(card.Tags[i], tag, StringComparison.OrdinalIgnoreCase))
                            {
                                card.Tags.RemoveAt(i);
                                removed = true;
                            }
                        }
                    }

                    if (removed)
                    {
                        card.LastModifiedDate = DateTime.UtcNow;
                        return OperationResult.SuccessResult(cardId);
                    }

                    return OperationResult.Failure("Tag not found");
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
            finally
            {
                _lock.ExitUpgradeableReadLock();
            }
        }
    }
}