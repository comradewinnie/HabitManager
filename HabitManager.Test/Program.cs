using HabitManager.Test;
using HabitManager.Domain;

class Program
{
    public static void Main()
    {
        try
        {
            var (users, habits) = DataGenerator.GenerateTestData();  // MI ieteica, kā iegūt funkcija radītus objektus konsoles lietotnē

            int habitCount = 0;

            List<string> idsOfUsersWithHabits = new List<string>();
            foreach (Habit habit in habits)
            {
                habitCount++;
                idsOfUsersWithHabits.Add(habit.UserId);
            }
            idsOfUsersWithHabits = idsOfUsersWithHabits.Distinct().ToList();  // MI pateica, kā atstāt listā tikai unikālas vērtības

            Console.WriteLine($"Total number of registered habits: {habitCount}\n");

            Console.WriteLine($"IDs of users who have habits: {string.Join(", ", idsOfUsersWithHabits)}\n");

            Console.WriteLine("List of each user's habits:\n");
            foreach (User user in users)
            {
                Console.WriteLine($"USER: {user.Id}\n");

                foreach (Habit habit in habits)
                {
                    if (habit.UserId == user.Id)
                    {
                        Console.WriteLine($"Title: {habit.Title}");

                        if (!string.IsNullOrWhiteSpace(habit.Description))
                            Console.WriteLine($"Description: {habit.Description}");

                        Console.WriteLine($"Periodicity: {string.Join(", ", habit.Periodicity)}");
                        Console.WriteLine($"Status: {habit.Status}");
                        Console.WriteLine($"Created at: {habit.CreatedAt}");

                        string wasCompletedToday = (habit.WasCompletedToday()) ? "YES" : "no";
                        Console.WriteLine($"Was completed today: {wasCompletedToday}\n");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR!\n{ex}");
        }
    }
}