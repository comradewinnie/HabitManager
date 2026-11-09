using HabitManager.Models;

namespace HabitManager.Maui.Data
{
    public interface IHabitProvider   // adaptēts pēc "UzdevumuParvaldnieks" piemēra
    {
        Task<IEnumerable<HabitDto>> GetUserHabitsAsync(string userId);
        Task<HabitDto> GetHabitAsync(Guid id);
        Task<IReadOnlyList<CompletionRecordDto>> GetHabitCompletionHistoryAsync(Guid id);
        Task<bool> CreateHabitAsync(CreateHabitDto dto);
        Task<bool> MarkAsCompleteAsync(MarkHabitAsCompleteDto dto);
        Task<bool> UpdateHabitAsync(Guid id, UpdateHabitDto dto);
        Task<bool> ToggleHabitStatusAsync(Guid id);
        Task<bool> DeleteHabitAsync(Guid id);
    }
}
