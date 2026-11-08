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
            var userId = await _us.GetCurrentUserIdAsync();
            var allHabits = await _hp.GetUserHabitsAsync(userId);

            if (allHabits == null)
            {
                Habits.Clear();
                return;
            }

            var uncompletedHabits = allHabits.Select(hb => hb.ToDomain()).Where(h => h.Status != Status.Archived && !h.WasCompletedToday()).ToList();
            Habits = new ObservableCollection<Habit>(uncompletedHabits);
        }

        [RelayCommand]
        private async Task MarkHabitAsCompleteAsync(Habit hb)
        {
            try
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
                    var dto = new MarkHabitAsCompleteDto(
                        HabitId: hb.Id,
                        CompletedAt: selectedDate
                    );
                    var success = await _hp.MarkAsCompleteAsync(dto);
                    if (success)
                    {
                        hb.MarkAsComplete(selectedDate);
                        
                        if (selectedDate.Date == DateTime.Now.Date)
                        {
                            Habits.Remove(hb);  // MI atgādināja, ka vajag noņemt objektu no observable saraksta, lai saraksts uzreiz atjauninātos
                        }
                    }
                    else  // ja nav šāda ieraduma vai nav pieejams serveris
                    {
                        await Shell.Current.DisplayAlertAsync("Server Error", "Failed to mark habit as complete.", "OK");
                        return;
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                await Shell.Current.DisplayAlertAsync("Invalid Operation", ex.Message, "OK");
            }
        }
    }
}