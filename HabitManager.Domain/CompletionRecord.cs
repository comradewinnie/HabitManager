using System.Text.Json.Serialization;

namespace HabitManager.Domain
{
    public record CompletionRecord  // MI ieteica taisīt kā record class, lai varētu salīdzināt objektus pēc lauku vērtībām
    {
        private Guid _id;
        private Guid _habitId;
        private DateTime _completedAt;

        public Guid Id
        {
            get
            {
                return _id;
            }
            init
            {
                if (value == Guid.Empty)
                    throw new ArgumentException("ID cannot be empty.");

                _id = value;
            }
        }
        public Guid HabitId
        {
            get
            {
                return _habitId;
            }
            init
            {
                if (value == Guid.Empty)
                    throw new ArgumentException("Habit ID cannot be empty.");

                _habitId = value;
            }
        }

        public DateTime CompletedAt
        {
            get
            {
                return _completedAt;
            }
            init
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Completion date cannot be in the future.");

                _completedAt = value;
            }
        }
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
