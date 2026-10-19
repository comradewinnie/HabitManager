using HabitManager.Domain;
using System.Text.Json;

namespace HabitManager.TestData
{
    public class TestDataContainer  // konteineris JSON serializācijai, kuru saģenerēja MI
    {
        public List<User> Users { get; set; } = new();
        public List<Habit> Habits { get; set; } = new();
    }

    public static class DataGenerator
    {
        public static string FileName { get; set; }

        private static (List<User>, List<Habit>) GenerateTestData()
        {
            // Lietotāji
            User u1 = new User("demo-user-1");

            User u2 = new User("demo-user-2");

            // Izpildes ieraksti
            var h1history = new List<CompletionRecord>
            {
                new CompletionRecord(
                    id: Guid.NewGuid(),
                    habitId: Guid.Parse("1a2b3c4d-5e6f-4a7b-8c9d-0e1f2a3b4c5d"),
                    completedAt: new DateTime(2026, 09, 24, 16, 31, 24)
                )
            };

            var h2history = new List<CompletionRecord>
            {
                new CompletionRecord(
                    id: Guid.NewGuid(),
                    habitId: Guid.Parse("7f8e9d0c-1b2a-4f3e-9d8c-7b6a5f4e3d2c"),
                    completedAt: new DateTime(2026, 09, 27, 23, 46, 51)
                )
            };

            var h4history = new List<CompletionRecord>
            {
                new CompletionRecord(
                    id: Guid.NewGuid(),
                    habitId: Guid.Parse("9e8d7c6b-5a4f-4e3d-2c1b-0a9f8e7d6c5b"),
                    completedAt: new DateTime(2026, 09, 21, 22, 54, 15)
                ),

                new CompletionRecord(
                    id: Guid.NewGuid(),
                    habitId: Guid.Parse("9e8d7c6b-5a4f-4e3d-2c1b-0a9f8e7d6c5b"),
                    completedAt: new DateTime(2026, 09, 23, 07, 29, 53)
                ),

                new CompletionRecord(
                    id: Guid.NewGuid(),
                    habitId: Guid.Parse("9e8d7c6b-5a4f-4e3d-2c1b-0a9f8e7d6c5b"),
                    completedAt: new DateTime(2026, 09, 25, 10, 07, 48)
                )
            };

            // Aktīvi ieradumi
            Habit h1 = new Habit(
                id: Guid.Parse("1a2b3c4d-5e6f-4a7b-8c9d-0e1f2a3b4c5d"),  // ievadīti konkrēti GUID, lai uzreiz piesaistītu izpildes ierakstus ieradumiem
                userId: "demo-user-1",
                title: "Learn Spanish",
                description: null,
                status: Status.Active,
                periodicity: Enum.GetValues<DayOfWeek>().ToList(),  // MI piedāvāja izmantot Enum.GetValues<DayOfWeek>().ToList(), lai iegūtu visus DayOfWeek vērtības
                history: h1history,
                createdAt: new DateTime(2026, 09, 23, 12, 54, 41),
                updatedAt: new DateTime(2026, 09, 23, 12, 54, 41)
            );

            h1.MarkAsComplete(DateTime.Now);

            Habit h2 = new Habit(
                id: Guid.Parse("7f8e9d0c-1b2a-4f3e-9d8c-7b6a5f4e3d2c"),
                userId: "demo-user-2",
                title: "Play piano",
                description: "Only my favorite songs...",
                status: Status.Active,
                periodicity: new List<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday },
                history: h2history,
                createdAt: new DateTime(2026, 9, 27, 15, 46, 04),
                updatedAt: new DateTime(2026, 9, 27, 16, 10, 11)
            );

            Habit h3 = new Habit(
                id: Guid.Parse("4c3b2a1e-0f9e-4d8c-b7a6-5f4e3d2c1b0a"),
                userId: "demo-user-1",
                title: "Read books",
                description: "I created this habit right now!!",
                status: Status.Active,
                periodicity: Enum.GetValues<DayOfWeek>().ToList(),
                history: new List<CompletionRecord>(),
                createdAt: DateTime.Now,
                updatedAt: DateTime.Now
            );

            // Arhivēts ieradums
            Habit h4 = new Habit(
                id: Guid.Parse("9e8d7c6b-5a4f-4e3d-2c1b-0a9f8e7d6c5b"),
                userId: "demo-user-1",
                title: "Do some sport",
                description: "It is important!",
                status: Status.Archived,
                periodicity: new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
                history: h4history,
                createdAt: new DateTime(2026, 09, 21, 20, 58, 02),
                updatedAt: new DateTime(2026, 09, 26, 11, 05, 15)
            );

            return (new List<User> { u1, u2 }, new List<Habit> { h1, h2, h3, h4 });  // MI ieteica, kā padot konsoles lietotnē funkcijā radītus objektus
        }

        public static void SaveTestDataToJson(List<User> users, List<Habit> habits)
        {
            TestDataContainer container = new TestDataContainer
            {
                Users = users,
                Habits = habits
            };

            string jsonString = JsonSerializer.Serialize(container);
            File.WriteAllText(FileName, jsonString);
        }

        private static void CreateInitialJson()
        {
            var (users, habits) = GenerateTestData();
            SaveTestDataToJson(users, habits);
        }
        
        public static (List<User>, List<Habit>) LoadTestDataFromJson()
        {
            if (!File.Exists(FileName))
            {
                CreateInitialJson();
            }

            string jsonString = File.ReadAllText(FileName);
            TestDataContainer container = JsonSerializer.Deserialize<TestDataContainer>(jsonString);
            return (container.Users, container.Habits);
        }  
        
        public static bool AddTestHabit(Habit hb)  // adaptēts pēc "UzdevumuParvaldnieks" piemēra
        {
            var (users, habits) = LoadTestDataFromJson();
            habits.Add(hb);
            SaveTestDataToJson(users, habits);
            return true;
        }

        public static bool DeleteTestHabit(Habit hb)  // adaptēts pēc "UzdevumuParvaldnieks" piemēra
        {
            var (users, habits) = LoadTestDataFromJson();

            var itemToRemove = habits.FirstOrDefault(h => h.Id == hb.Id);  // MI pateica, kā sameklēt objektu pēc īpašības vērtības
            if (itemToRemove != null)
            {
                habits.Remove(itemToRemove);
                SaveTestDataToJson(users, habits);
                return true;
            }

            return false;
        }

        public static IEnumerable<Habit> GetUserTestHabits(string userId)  // adaptēts pēc "UzdevumuParvaldnieks" piemēra
        {
            var (users, habits) = LoadTestDataFromJson();
            var userHabits = habits.Where(h => h.UserId == userId);
            return userHabits;
        }

        public static Habit GetTestHabitById(Guid id)
        {
            var (users, habits) = LoadTestDataFromJson();
            var hb = habits.FirstOrDefault(h => h.Id == id);
            return hb;
        }

        public static bool UpdateTestHabit(Habit hb)  // adaptēts pēc "UzdevumuParvaldnieks" piemēra
        {
            var (users, habits) = LoadTestDataFromJson();

            int index = habits.FindIndex(h => h.Id == hb.Id);  // MI pateica, kā sameklēt objekta indeksu pēc īpašības vērtības
            if (index != -1)
            {
                habits[index] = hb;
                SaveTestDataToJson(users, habits);
                return true;
            }

            return false;
        }
    }
}
