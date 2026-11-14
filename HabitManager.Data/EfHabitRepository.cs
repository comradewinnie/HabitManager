using HabitManager.Domain;
using Microsoft.EntityFrameworkCore;

namespace HabitManager.Data
{
    // saģenerēja MI pēc mana DataGenerator.cs piemēra
    public class EfHabitRepository : IHabitRepository
    {
        private readonly AppDbContext _context;

        public EfHabitRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Habit>?> GetUserHabitsAsync(string userId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
                return null;

            return await _context.Habits  // ieradumi kopā ar izpildes ierakstiem
                .Include(h => h.History)
                .Where(h => h.UserId == userId)
                .ToListAsync();
        }

        public async Task<Habit?> GetHabitAsync(Guid id)
        {
            return await _context.Habits
                .Include(h => h.History)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<IReadOnlyList<CompletionRecord>?> GetHabitCompletionHistoryAsync(Guid id)
        {
            var hb = await _context.Habits
                .Include(h => h.History)
                .FirstOrDefaultAsync(h => h.Id == id);

            return hb?.History.ToList();
        }

        public async Task<bool> AddHabitAsync(Habit hb)
        {
            if (hb == null)
                return false;

            await _context.Habits.AddAsync(hb);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateHabitDetailsAsync(Guid id, string title, string description, List<DayOfWeek> periodicity)
        {
            var hb = await _context.Habits.FindAsync(id);
            if (hb == null)
                return false;

            hb.UpdateDetails(title, description, periodicity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateHabitStatusAsync(Guid id)
        {
            var hb = await _context.Habits.FindAsync(id);
            if (hb == null)
                return false;

            hb.ToggleHabitStatus();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddCompletionRecordAsync(Guid habitId, DateTime completedAt)
        {
            var hb = await _context.Habits
                .Include(h => h.History)
                .FirstOrDefaultAsync(h => h.Id == habitId);

            if (hb == null)
                return false;

            hb.MarkAsComplete(completedAt);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteHabitAsync(Guid id)
        {
            var hb = await _context.Habits.FindAsync(id);
            if (hb == null)
                return false;

            _context.Habits.Remove(hb);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}