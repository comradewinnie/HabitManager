using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabitManager.Domain;
using HabitManager.Maui.Data;
using HabitManager.Maui.Helpers;

namespace HabitManager.Maui.ViewModels
{
    [QueryProperty(nameof(Hb), "HabitToEdit")]
    public partial class HabitFormViewModel : ObservableObject  // adaptēts pēc "UzdevumuParvaldnieks" piemēra
    {
        private IHabitProvider _hp;
        private IUserService _us;

        [ObservableProperty]
        private List<PeriodicityHelper.SelectionItem> _periodicity = PeriodicityHelper.ToSelectionItems();

        [ObservableProperty]
        private Habit _hb;

        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private string _description = string.Empty;

        public HabitFormViewModel(IHabitProvider hp, IUserService us)
        {
            _hp = hp;
            _us = us;
        }

        partial void OnHbChanged(Habit hb)
        {
            if (hb is not null)
            {
                Title = hb.Title;
                Description = hb.Description;
                Periodicity = PeriodicityHelper.ToSelectionItems(hb.Periodicity);
            }
            else
            {
                Title = string.Empty;
                Description = string.Empty;
                Periodicity = PeriodicityHelper.ToSelectionItems();
            }
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            var periodicity = PeriodicityHelper.ToEnums(Periodicity);

            if (Hb is null)
            {
                Habit hb = new Habit(
                    id: Guid.NewGuid(),
                    userId: _us.GetCurrentUserId(),
                    title: Title,
                    description: Description,
                    status: Status.Active,
                    periodicity: periodicity,
                    history: new List<CompletionRecord>(),
                    createdAt: DateTime.Now,
                    updatedAt: DateTime.Now
                );
                _hp.AddHabit(hb);
            }
            else
            {
                Hb.Title = Title;
                Hb.Description = Description;
                Hb.Periodicity = periodicity;
                Hb.UpdatedAt = DateTime.Now;
                _hp.UpdateHabit(Hb);
            }

            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
