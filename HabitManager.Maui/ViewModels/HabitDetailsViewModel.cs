using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabitManager.Domain;
using HabitManager.Maui.Data;
using HabitManager.Maui.Views;
using HabitManager.Shared.Mappings;

[QueryProperty(nameof(Hb), "HabitToShow")]
public partial class HabitDetailsViewModel : ObservableObject  // adaptēts pēc "UzdevumuParvaldnieks" piemēra
{
    private IHabitProvider _hp;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ArchiveButtonText))]
    private Habit? _hb;
    public string ArchiveButtonText => Hb?.Status == Status.Archived ? "Back from Archive" : "To Archive";  // MI ieteica pierakstu ar =>

    public HabitDetailsViewModel(IHabitProvider hp)
    {
        _hp = hp;
    }

    [RelayCommand]
    private async Task EditHabitAsync()
    {
        if (Hb is null)
            return;

        await Shell.Current.GoToAsync($"{nameof(HabitFormPage)}", new Dictionary<string, object>
        {
            { "HabitToEdit", Hb }
        });
    }

    [RelayCommand]
    private async Task ToggleHabitStatusAsync()
    {
        if (Hb == null)
            return;

        try
        {
            await _hp.ToggleHabitStatusAsync(Hb.Id);

            Hb.ToggleHabitStatus(); // MI pateica, ka drīkst mainīt statusu lokāli tikai pēc success no servera
            OnPropertyChanged(nameof(Hb));
            OnPropertyChanged(nameof(ArchiveButtonText));
        }
        catch (InvalidOperationException ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    [RelayCommand]
    private async Task DeleteHabitAsync()
    {
        if (Hb is null)
            return;

        bool isConfirmed = await Shell.Current.DisplayAlertAsync(
            "Delete Habit",
            $"Are you sure you want to delete \"{Hb.Title}\"?",
            "Yes",
            "No");

        if (!isConfirmed)
            return;

        try
        {
            await _hp.DeleteHabitAsync(Hb.Id);

            await Shell.Current.GoToAsync("..");
        }
        catch (InvalidOperationException ex)  // ja nav šāda ieraduma vai nav pieejams serveris
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    public async Task ReloadHabitAsync()  // MI atgādināja, ka vajag atjaunot ObservableProperty pēc objekta rediģēšanas
    {
        if (Hb == null)
            return;

        try
        {
            var reloadedDto = await _hp.GetHabitAsync(Hb.Id);

            Hb = reloadedDto.ToDomain();
        }
        catch (InvalidOperationException ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }
}