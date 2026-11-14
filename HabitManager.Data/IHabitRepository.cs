using HabitManager.Domain;

namespace HabitManager.Data
{
    public interface IHabitRepository
    {
        Task<IEnumerable<Habit>?> GetUserHabitsAsync(string userId);
        Task<Habit?> GetHabitAsync(Guid id);
        Task<IReadOnlyList<CompletionRecord>?> GetHabitCompletionHistoryAsync(Guid id);
        Task<bool> AddHabitAsync(Habit hb);
        Task<bool> UpdateHabitDetailsAsync(Guid id, string title, string description, List<DayOfWeek> periodicity);
        Task<bool> UpdateHabitStatusAsync(Guid id);
        Task<bool> AddCompletionRecordAsync(Guid habitId, DateTime completedAt);
        Task<bool> DeleteHabitAsync(Guid id);
    }
}