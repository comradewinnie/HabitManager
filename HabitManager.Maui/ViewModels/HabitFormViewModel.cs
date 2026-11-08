using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabitManager.Domain;
using HabitManager.Maui.Data;
using HabitManager.Maui.Helpers;
using HabitManager.Models;

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
            try
            {
                var periodicity = PeriodicityHelper.ToEnums(Periodicity);

                if (Hb is null)
                {
                    var dto = new CreateHabitDto(
                        UserId: await _us.GetCurrentUserIdAsync(),
                        Title: Title,
                        Description: Description,
                        Periodicity: periodicity
                    );

                    var success = await _hp.CreateHabitAsync(dto);
                    if (!success)  // MI pievienoja šo
                    {
                        await Shell.Current.DisplayAlertAsync("Server Error", "Failed to create habit.", "OK");
                        return;
                    }
                }
                else
                {
                    var dto = new UpdateHabitDto
                    (
                        HabitId: Hb.Id,
                        Title: Title,
                        Description: Description,
                        Periodicity: periodicity
                    );

                    var success = await _hp.UpdateHabitAsync(dto);
                    if (!success)
                    {
                        await Shell.Current.DisplayAlertAsync("Server Error", "Failed to update habit.", "OK");
                        return;
                    }
                }

                await Shell.Current.GoToAsync("..");
            }
            catch (ArgumentException ex)
            {
                await Shell.Current.DisplayAlertAsync("Validation Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
