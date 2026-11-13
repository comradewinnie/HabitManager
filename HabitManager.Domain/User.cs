using System.Text.Json.Serialization;

namespace HabitManager.Domain
{
    public class User
    {
        private string _id;
        private List<Habit> _habits = new();  // MI pievienoja īpašību 1:M saitei EF Core
        [JsonIgnore]
        public IReadOnlyList<Habit> Habits => _habits;

        public string Id
        {
            get
            {
                return _id;
            }
            init
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("ID cannot be empty.");

                _id = value;
            }
        }

        public User() { }

        public User(string id)
        {
            Id = id;
        }

        public void AddHabit(Habit hb)
        {
            if (hb == null)
                throw new ArgumentNullException("Habit cannot be null.");

            if (hb.UserId != Id)
                throw new InvalidOperationException("Cannot add a habit that belongs to another user.");

            if (_habits.Any(h => h.Id == hb.Id))
                throw new InvalidOperationException("Habit with the same ID already linked to this user.");

            _habits.Add(hb);
        }

    }
}
