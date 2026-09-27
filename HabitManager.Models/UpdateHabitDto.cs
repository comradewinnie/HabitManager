using HabitManager.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace HabitManager.Models
{
    public record UpdateHabitDto(
        string HabitId,
        string Title,
        string? Description,
        Status Status,
        List<DayOfWeek> Periodicity
    );
}
