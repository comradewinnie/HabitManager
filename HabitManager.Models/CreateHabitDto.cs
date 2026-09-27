using HabitManager.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace HabitManager.Models
{
    public record CreateHabitDto(
        string UserId,  // tikai kamēr nav autentifikācijas
        string Title,
        string? Description = null,
        Status? Status = null,
        List<DayOfWeek>? Periodicity = null
    );
}
