using CommunityToolkit.Mvvm.Input;
using HabitManager.Domain;
using HabitManager.Maui.Data;
using HabitManager.Maui.Views;
using System.Collections.ObjectModel;

namespace HabitManager.Maui.ViewModels
{
    public partial class UncompletedHabitListViewModel : HabitListViewModel
    {
        public UncompletedHabitListViewModel(IHabitProvider hp, IUserService us) : base(hp, us)
        {
        }

        [RelayCommand]
        public async Task AddHabitAsync()
        {
            await Shell.Current.GoToAsync(nameof(HabitFormPage));
        }

        public override void LoadHabits()
        {
            var allHabits = _hp.GetUserHabits(_us.GetCurrentUserId());
            var uncompletedHabits = allHabits.Where(h => h.Status != Status.Archived && !h.WasCompletedToday());
            Habits = new ObservableCollection<Habit>(uncompletedHabits);
        }

        [RelayCommand]
        private async Task MarkHabitAsCompleteAsync(Habit hb)
        {
            if (hb == null)
                return;

            string result = await Shell.Current.DisplayPromptAsync(  // saģenerēja MI
                title: "Complete Habit",
                message: "Enter completion date:",
                accept: "OK",
                cancel: "Cancel",
                placeholder: "DD.MM.YYYY",
                initialValue: DateTime.Now.ToString("dd.MM.yyyy"));

            if (string.IsNullOrWhiteSpace(result))
                return;

            if (DateTime.TryParse(result, out DateTime selectedDate))
            {
                hb.MarkAsComplete(selectedDate);
                _hp.UpdateHabit(hb);

                if (selectedDate == DateTime.Now.Date && Habits.Contains(hb))
                {
                    Habits.Remove(hb);  // MI atgādināja, ka vajag noņemt objektu no observable saraksta, lai saraksts uzreiz atjauninātos
                }
            }
        }
    }
}