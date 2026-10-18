using System;
using System.Collections.Generic;
using System.Text;

namespace HabitManager.Models
{
    public record MarkHabitAsCompleteDto(
        Guid HabitId
    );
}
