using System.Text.Json.Serialization;

namespace HabitManager.Domain
{
    public class Habit
    {
        private Guid _id;  // izmantoju private field, jo ir papildu loģika
        private string _userId;
        private string _title;
        private string? _description;
        private Status _status;
        private List<DayOfWeek> _periodicity = new();
        private List<CompletionRecord> _history = new();
        private DateTime _createdAt;
        private DateTime _updatedAt;

        public Guid Id
        {
            get
            {
                return _id;
            }
            init  // MI ieteica izmantot init, ja nevajadzēs mainīt īpašības
            {
                if (value == Guid.Empty)
                    throw new ArgumentException("ID cannot be empty.");

                _id = value;
            }
        }
        public string UserId
        {
            get
            {
                return _userId;
            }
            init
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("User ID cannot be empty.");

                _userId = value;
            }
        }
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 3 || value.Length > 30)  // MI pievienoja IsNullOrWhiteSpace, jo nevar izmērīt Length pie null
                    throw new ArgumentException("Title must be between 3 and 30 characters long.");  // MI pievienoja izņēmuma izmešanu, jo bez tā, ja title neietilpst 3-30 simbolu robežās, _title paliks null, kas nav pieļaujams

                _title = value;
            }
        }
        public string? Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (value != null && value.Length > 250)
                    throw new ArgumentException("Description cannot exceed 250 characters.");

                _description = value;
            }
        }
        public Status Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (!Enum.IsDefined(typeof(Status), value))
                    throw new ArgumentOutOfRangeException("Invalid status value.");

                _status = value;
            }
        }
        public List<DayOfWeek> Periodicity
        {
            get
            {
                return _periodicity;
            }
            private set  // lai ārpus klases neviens nevarētu izmainīt grafiku
            {
                if (value == null)
                    throw new ArgumentNullException("Periodicity cannot be null.");

                _periodicity = value;
            }
        }
        public IReadOnlyList<CompletionRecord> History => _history;  // MI piedāvāja IReadOnlyList pie public, lai ārpus klases neviens nevarētu izmainīt vēsturi
        public DateTime CreatedAt
        {
            get
            {
                return _createdAt;
            }
            init
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Creation date cannot be in the future.");

                _createdAt = value;
            }
        }
        public DateTime UpdatedAt
        {
            get
            {
                return _updatedAt;
            }
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Update date cannot be in the future.");
                _updatedAt = value;
            }
        }
        [JsonIgnore]
        public User? User { get; set; }

        public Habit() { }

        public Habit(Guid id, string userId, string title, string? description, Status status, List<DayOfWeek> periodicity, IReadOnlyList<CompletionRecord> history, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            UserId = userId;
            Title = title;
            Description = description;
            Status = status;
            Periodicity = periodicity;
            _history = history.ToList();
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public void MarkAsComplete(DateTime completionDate)
        {
            if (Status == Status.Archived)
                throw new InvalidOperationException("Cannot mark an archived habit as completed.");  // MI piedāvāja šādu izņēmuma tipu

            if (completionDate > DateTime.Now)
                throw new InvalidOperationException("Completion date cannot be in the future.");

            if (completionDate < CreatedAt)
                throw new InvalidOperationException("Cannot complete a habit before its creation date.");

            foreach (var record in History)
            {
                if (record.CompletedAt.Date == completionDate.Date)
                    throw new InvalidOperationException("You've already marked this habit as completed for this day.");
            }

            _history.Add(new CompletionRecord(id: Guid.NewGuid(), habitId: Id, completedAt: completionDate));
        }

        public bool WasCompletedToday()
        {
            if (History.Any(record => record.CompletedAt.Date == DateTime.Today))
            {
                return true;
            }
            return false;
        }

        public void ToggleHabitStatus()
        {
            if (Status == Status.Active)
            {
                Status = Status.Archived;
            }
            else
            {
                Status = Status.Active;
            }
        }

        public void UpdateDetails(string title, string? description, List<DayOfWeek> periodicity)
        {
            Title = title;
            Description = description;
            Periodicity = periodicity;
            UpdatedAt = DateTime.Now;
        }
    }
}
