using System;
using System.Collections.Generic;
using System.Text;

namespace HabitManager.Domain
{
    public class Habit
    {
        public Guid Id { get; init; }  // MI ieteica izmantot auto-implemented properties, ja nav papildu loģikas, un ar init, ja nevajadzēs tos mainīt
        public string UserId { get; init; }
        private string _title;  // izmantoju private field, jo ir papildu loģika
        public string? Description { get; set; }
        public Status Status { get; set; }
        public List<DayOfWeek> Periodicity { get; set; }
        private List<CompletionRecord> _history;
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; set; }

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
        public IReadOnlyList<CompletionRecord> History => _history;  // MI piedāvāja darīt šādi, lai ārpus klases neviens nevarētu izmainīt vēsturi

        public Habit(Guid id, string userId, string title, string? description = null, Status? status = null, List<DayOfWeek>? periodicity = null, List<CompletionRecord>? history = null, DateTime? createdAt = null, DateTime? updatedAt = null)
          // MI pievienoja null pie pēdējiem 6 parametriem, lai varētu nepadod tos konstruktorā
        {
            Id = id;
            UserId = userId;
            Title = title;
            Description = description;
            Status = status ?? Status.Active;
            Periodicity = periodicity ?? Enum.GetValues<DayOfWeek>().ToList();  // MI piedāvāja izmantot Enum.GetValues<DayOfWeek>().ToList(), lai iegūtu visus DayOfWeek vērtības
            _history = history ?? new List<CompletionRecord>();
            CreatedAt = createdAt ?? DateTime.Now;
            UpdatedAt = updatedAt ?? DateTime.Now;
        }

        public void MarkAsCompleted()
        {
            if (Status == Status.Archived)
                throw new InvalidOperationException("Cannot mark an archived habit as completed.");  // MI piedāvāja šādu izņēmuma tipu

            foreach (var record in History)
            {
                if (record.CompletedAt.Date == DateTime.Now.Date)
                    throw new InvalidOperationException("You've already marked this habit as completed.");
            }

            _history.Add(new CompletionRecord(id: Guid.NewGuid(), habitId: Id, completedAt: DateTime.Now));
        }

        public bool WasCompletedToday()
        {
            if (this.History.Any(record => record.CompletedAt.Date == DateTime.Today))
            {
                return true;
            }
            return false;
        }
    }
}
