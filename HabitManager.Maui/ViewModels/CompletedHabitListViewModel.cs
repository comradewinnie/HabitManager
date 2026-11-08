using CommunityToolkit.Mvvm.Input;
using HabitManager.Domain;
using HabitManager.Maui.Data;
using HabitManager.Shared.Mappings;
using System.Collections.ObjectModel;

namespace HabitManager.Maui.ViewModels
{
    public partial class CompletedHabitListViewModel : HabitListViewModel
    {
        public CompletedHabitListViewModel(IHabitProvider hp, IUserService us) : base(hp, us)
        {
        }

        [RelayCommand]
        public override async Task LoadHabitsAsync()
        {
            var userId = await _us.GetCurrentUserIdAsync();
            var allHabits = await _hp.GetUserHabitsAsync(userId);

            if (allHabits == null)
            {
                Habits.Clear();
                return;
            }

            var completedHabits = allHabits.Select(hb => hb.ToDomain()).Where(hb => hb.Status != Status.Archived && hb.WasCompletedToday()).ToList();
            Habits = new ObservableCollection<Habit>(completedHabits);
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
            else
            {
                await Shell.Current.DisplayAlertAsync("Server Error", "Failed to change status of the habit.", "OK");
                return;
            }
        }
    }
}