using HabitManager.Maui.ViewModels;

namespace HabitManager.Maui.Views;

public partial class ArchivedHabitListPage : ContentPage
{
    private ArchivedHabitListViewModel _vm;

    public ArchivedHabitListPage(ArchivedHabitListViewModel vm)
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