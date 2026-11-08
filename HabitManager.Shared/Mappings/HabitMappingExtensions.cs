using HabitManager.Domain;
using HabitManager.Models;

namespace HabitManager.Shared.Mappings
{
    public static class HabitMappingExtensions
    {
        public static HabitDto ToDto(this Habit domain)
        {
            List<CompletionRecordDto> historyToDto = new List<CompletionRecordDto>();
            foreach (CompletionRecord record in domain.History)
            {
                historyToDto.Add(record.ToDto());
            }

            return new HabitDto
            (
                Id: domain.Id,
                UserId: domain.UserId,
                Title: domain.Title,
                Description: domain.Description,
                Status: domain.Status,
                Periodicity: domain.Periodicity,
                History: historyToDto,
                CreatedAt: domain.CreatedAt,
                UpdatedAt: domain.UpdatedAt
            );
        }

        public static Habit ToDomain(this HabitDto dto)
        {
            List<CompletionRecord> historyToDomain = new List<CompletionRecord>();
            foreach (CompletionRecordDto record in dto.History)
            {
                historyToDomain.Add(record.ToDomain());
            }

            return new Habit
            (
                id: dto.Id,
                userId: dto.UserId,
                title: dto.Title,
                description: dto.Description,
                status: dto.Status,
                periodicity: dto.Periodicity,
                history: historyToDomain,
                createdAt: dto.CreatedAt,
                updatedAt: dto.UpdatedAt
            );
        }

        public static Habit ToDomain(this CreateHabitDto dto)
        {
            return new Habit
            (
                id: Guid.NewGuid(),
                userId: dto.UserId,
                title: dto.Title,
                description: dto.Description,
                status: Status.Active,
                periodicity: dto.Periodicity,
                history: new List<CompletionRecord>(),
                createdAt: DateTime.Now,
                updatedAt: DateTime.Now
            );
        }
    }
}