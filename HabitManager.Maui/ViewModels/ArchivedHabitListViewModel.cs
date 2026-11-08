using CommunityToolkit.Mvvm.Input;
using HabitManager.Domain;
using HabitManager.Maui.Data;
using HabitManager.Shared.Mappings;
using System.Collections.ObjectModel;

namespace HabitManager.Maui.ViewModels
{
    public partial class ArchivedHabitListViewModel : HabitListViewModel
    {
        public ArchivedHabitListViewModel(IHabitProvider hp, IUserService us) : base(hp, us)
        {
        }

        [RelayCommand]
        public override async Task LoadHabitsAsync()
        {
            var userId = await _us.GetCurrentUserIdAsync();
            var allHabits = await _hp.GetUserHabitsAsync(userId);

            if (allHabits == null)  // ja nav šāda lietotāja vai nav pieejams serveris
            {
                Habits.Clear();  // MI ieteica šo, lai sarakstā nebūtu elementu no iepriekšējas ielādes
                return;
            }

            var archivedHabits = allHabits.Where(hb => hb.Status == Status.Archived).Select(hb => hb.ToDomain()).ToList();  // MI ieteica pārveidot .ToList()
            Habits = new ObservableCollection<Habit>(archivedHabits);
        }

        [RelayCommand]
        private async Task ToggleHabitStatusAsync(Habit hb)
        {
            if (hb == null)
                return;

            var success = await _hp.ToggleHabitStatusAsync(hb.Id);

            if (success)
            {
                hb.ToggleHabitStatus();
                Habits.Remove(hb);
            }
            else  // ja nav šāda ieraduma vai nav pieejams serveris
            {
                await Shell.Current.DisplayAlertAsync("Server Error", "Failed to change status of the habit.", "OK");
                return;
            }
        }
    }
}