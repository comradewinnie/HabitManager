using System.Text.Json.Serialization;

namespace HabitManager.Domain
{
    public record CompletionRecord  // MI ieteica taisīt kā record class, lai varētu salīdzināt objektus pēc lauku vērtībām
    {
        public Guid Id { get; init; }
        public Guid HabitId { get; init; }
        public DateTime CompletedAt { get; init; }

        [JsonIgnore]
        public Habit? Habit { get; set; }  // MI pievienoja ER Core navigācijas īpašību ārejai atslēgai

        public CompletionRecord() { }  // MI pievienoja tukšu konstruktoru priekš EF Core

        public CompletionRecord(Guid id, Guid habitId, DateTime completedAt)
        {
            Id = id;
            HabitId = habitId;
            CompletedAt = completedAt;
        }
    }
}
