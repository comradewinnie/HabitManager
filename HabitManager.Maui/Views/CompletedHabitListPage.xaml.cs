using HabitManager.Maui.ViewModels;

namespace HabitManager.Maui.Views;

public partial class CompletedHabitListPage : ContentPage
{
    private CompletedHabitListViewModel _vm;

    public CompletedHabitListPage(CompletedHabitListViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is CompletedHabitListViewModel vm)
            await vm.LoadHabitsAsync();
    }
}