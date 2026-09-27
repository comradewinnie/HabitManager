using System;
using System.Collections.Generic;
using System.Text;

namespace HabitManager.Models
{
    public record CompletionRecordDto(
        Guid Id,
        Guid HabitId,
        DateTime CompletedAt
    );
}
