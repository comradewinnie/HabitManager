using System;
using System.Collections.Generic;
using System.Text;

namespace HabitManager.Domain
{
    public record History  // MI ieteica record, lai varētu salīdzināt objektus pēc lauku vērtībām
    {
        public string HabitId { get; init; }
        public DateTime CompletedAt { get; init; }
    }
}
