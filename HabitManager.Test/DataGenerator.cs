using HabitManager.Domain;

namespace HabitManager.Test
{
    public static class DataGenerator
    {
        public static (List<User>, List<Habit>) GenerateTestData()
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
                history: h1history,
                createdAt: new DateTime(2026, 09, 23, 12, 54, 41),
                updatedAt: new DateTime(2026, 09, 23, 12, 54, 41)
            );

            h1.MarkAsCompleted();

            Habit h2 = new Habit(
                id: Guid.Parse("7f8e9d0c-1b2a-4f3e-9d8c-7b6a5f4e3d2c"),
                userId: "demo-user-2",
                title: "Play piano",
                description: "Only my favorite songs...",
                periodicity: new List<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday },
                history: h2history,
                createdAt: new DateTime(2026, 9, 27, 15, 46, 04),
                updatedAt: new DateTime(2026, 9, 27, 16, 10, 11)
            );

            Habit h3 = new Habit(
                id: Guid.Parse("4c3b2a1e-0f9e-4d8c-b7a6-5f4e3d2c1b0a"),
                userId: "demo-user-1",
                title: "Read books",
                description: "I created this habit right now!!"
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
    }

}
