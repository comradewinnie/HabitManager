using HabitManager.Domain;
using HabitManager.TestData;

namespace HabitManager.Maui.Data
{
    public class TestHabitProvider : IHabitProvider   // adaptēts pēc "UzdevumuParvaldnieks" piemēra
    {
        static TestHabitProvider()  // MI izlaboja problēmu ar "read-only file directory" Androidā
        {
            DataGenerator.FileName = Path.Combine(FileSystem.AppDataDirectory, "TestData.json");
        }

        public bool AddHabit(Habit hb)
        {
            return DataGenerator.AddTestHabit(hb);
        }

        public bool DeleteHabit(Habit hb)
        {
            return DataGenerator.DeleteTestHabit(hb);
        }

        public Habit GetHabitById(Guid id)
        {
            return DataGenerator.GetTestHabitById(id);
        }

        public IEnumerable<Habit> GetUserHabits(string userId)
        {
            return DataGenerator.GetUserTestHabits(userId);
        }

        public bool UpdateHabit(Habit hb)
        {
            return DataGenerator.UpdateTestHabit(hb);
        }
    }
}
