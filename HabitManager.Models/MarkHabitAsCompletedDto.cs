using System;
using System.Collections.Generic;
using System.Text;

namespace HabitManager.Models
{
    public record MarkHabitAsCompletedDto(
        Guid HabitId
    );
}
