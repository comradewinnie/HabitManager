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
    }
}
