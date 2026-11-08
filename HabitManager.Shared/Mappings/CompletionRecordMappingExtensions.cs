using HabitManager.Domain;
using HabitManager.Models;

namespace HabitManager.Shared.Mappings
{
    public static class CompletionRecordMappingExtensions
    {
        public static CompletionRecordDto ToDto(this CompletionRecord domain)  // MI ieteica pierakstu ar this, lai varētu šo izmantot kā paplašinājumu
        {
            return new CompletionRecordDto
            (
                Id: domain.Id,
                HabitId: domain.HabitId,
                CompletedAt: domain.CompletedAt
            );
        }

        public static CompletionRecord ToDomain(this CompletionRecordDto dto)
        {
            return new CompletionRecord
            (
                id: dto.Id,
                habitId: dto.HabitId,
                completedAt: dto.CompletedAt
            );
        }
    }
}
