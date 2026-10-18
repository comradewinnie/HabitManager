using HabitManager.Domain;
using HabitManager.Maui.ViewModels;

namespace HabitManager.Maui.Views;

public partial class UncompletedHabitListPage : ContentPage   // nokopēts no "UzdevumuParvaldnieks"
{
    private UncompletedHabitListViewModel _vm;

    public UncompletedHabitListPage(UncompletedHabitListViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _vm.LoadHabits();
    }
}