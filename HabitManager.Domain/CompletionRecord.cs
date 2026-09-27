using System;
using System.Collections.Generic;
using System.Text;

namespace HabitManager.Domain
{
    public record CompletionRecord  // MI ieteica taisīt kā record class, lai varētu salīdzināt objektus pēc lauku vērtībām
    {
        public Guid Id { get; init; }
        public Guid HabitId { get; init; }
        public DateTime CompletedAt { get; init; }

        public CompletionRecord(Guid id, Guid habitId, DateTime completedAt)
        {
            Id = id;
            HabitId = habitId;
            CompletedAt = completedAt;
        }
    }
}
