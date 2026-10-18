using CommunityToolkit.Mvvm.Input;
using HabitManager.Domain;
using HabitManager.Maui.Data;
using System.Collections.ObjectModel;

namespace HabitManager.Maui.ViewModels
{
    public partial class CompletedHabitListViewModel : HabitListViewModel
    {
        public CompletedHabitListViewModel(IHabitProvider hp, IUserService us) : base(hp, us)
        {
        }

        public override void LoadHabits()
        {
            var allHabits = _hp.GetUserHabits(_us.GetCurrentUserId());
            var completedHabits = allHabits.Where(h => h.Status != Status.Archived && h.WasCompletedToday());
            Habits = new ObservableCollection<Habit>(completedHabits);
        }

        [RelayCommand]
        private async Task ToggleHabitStatusAsync(Habit hb)
        {
            if (hb == null)
                return;

            if (hb.Status == Status.Active)
            {
                hb.Status = Status.Archived;
                _hp.UpdateHabit(hb);
            }
            else
            {
                hb.Status = Status.Active;
                _hp.UpdateHabit(hb);
            }

            if (Habits.Contains(hb))
                Habits.Remove(hb);
        }
    }
}