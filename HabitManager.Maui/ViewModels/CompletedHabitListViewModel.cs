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
            try
            {
                var userId = await _us.GetCurrentUserIdAsync();
                var allHabits = await _hp.GetUserHabitsAsync(userId);

                var completedHabits = allHabits.Select(hb => hb.ToDomain()).Where(hb => hb.Status != Status.Archived && hb.WasCompletedToday()).ToList();
                Habits = new ObservableCollection<Habit>(completedHabits);
            }
            catch (InvalidOperationException ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
                Habits.Clear();
            }
        }

        [RelayCommand]
        private async Task ToggleHabitStatusAsync(Habit hb)
        {
            if (hb == null)
                return;

            try
            {
                await _hp.ToggleHabitStatusAsync(hb.Id);

                hb.ToggleHabitStatus();
                Habits.Remove(hb);
            }
            catch (InvalidOperationException ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        }
    }
}