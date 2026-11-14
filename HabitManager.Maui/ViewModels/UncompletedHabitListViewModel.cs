using CommunityToolkit.Mvvm.Input;
using HabitManager.Domain;
using HabitManager.Maui.Data;
using HabitManager.Maui.Views;
using HabitManager.Models;
using HabitManager.Shared.Mappings;
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

        [RelayCommand]
        public override async Task LoadHabitsAsync()
        {
            try
            {
                var userId = await _us.GetCurrentUserIdAsync();
                var allHabits = await _hp.GetUserHabitsAsync(userId);

                var uncompletedHabits = allHabits.Select(hb => hb.ToDomain()).Where(h => h.Status != Status.Archived && !h.WasCompletedToday()).ToList();
                Habits = new ObservableCollection<Habit>(uncompletedHabits);
            }
            catch (InvalidOperationException ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
                Habits.Clear();
            }
        }

        [RelayCommand]
        private async Task MarkHabitAsCompleteAsync(Habit hb)
        {
            if (hb == null)
                return;

            string date = await Shell.Current.DisplayPromptAsync(  // saģenerēja MI
                title: "Complete Habit",
                message: "Enter completion date:",
                accept: "OK",
                cancel: "Cancel",
                placeholder: "DD.MM.YYYY",
                initialValue: DateTime.Now.ToString("dd.MM.yyyy"));

            if (string.IsNullOrWhiteSpace(date))
                return;

            string time = await Shell.Current.DisplayPromptAsync(
                title: "Complete Habit",
                message: "Enter completion time (HH:mm):",
                accept: "OK",
                cancel: "Cancel",
                placeholder: "HH:mm",
                initialValue: DateTime.Now.ToString("HH:mm"));

            if (string.IsNullOrWhiteSpace(time))
                return;

            try
            {
                if (DateTime.TryParse(date, out DateTime selectedDate) && TimeSpan.TryParse(time, out TimeSpan selectedTime))
                {
                    DateTime selectedDateTime = selectedDate.Date.Add(selectedTime);  // MI ieteica, kā apvienot datumu ar laiku

                    var dto = new MarkHabitAsCompleteDto(
                        HabitId: hb.Id,
                        CompletedAt: selectedDateTime
                    );
                    await _hp.MarkAsCompleteAsync(dto);

                    hb.MarkAsComplete(selectedDateTime);
                    
                    if (selectedDateTime.Date == DateTime.Now.Date)
                    {
                        Habits.Remove(hb);  // MI atgādināja, ka vajag noņemt objektu no observable saraksta, lai saraksts uzreiz atjauninātos
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlertAsync("Error", "Invalid date or time format.", "OK");
                }
            }
            catch (InvalidOperationException ex)  // ja nav šāda ieraduma vai nav pieejams serveris
            {
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        }
    }
}