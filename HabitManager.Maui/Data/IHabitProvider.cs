using HabitManager.Domain;

namespace HabitManager.Maui.Data
{
    public interface IHabitProvider   // adaptēts pēc "UzdevumuParvaldnieks" piemēra
    {
        IEnumerable<Habit> GetUserHabits(string userId);
        Habit GetHabitById(Guid id);
        bool AddHabit(Habit hb);
        bool UpdateHabit(Habit hb);
        bool DeleteHabit(Habit hb);
    }
}
