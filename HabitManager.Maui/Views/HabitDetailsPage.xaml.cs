using HabitManager.Maui.Data;
using HabitManager.Maui.ViewModels;

namespace HabitManager.Maui.Views;

public partial class HabitDetailsPage : ContentPage
{
	public HabitDetailsPage(IHabitProvider hp)
	{
		InitializeComponent();
        BindingContext = new HabitDetailsViewModel(hp);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is HabitDetailsViewModel vm)  // MI ieteica šo pierakstu, jo mēs izveidojām jaunu viewmodeli uzreiz konstruktorā
        {
            await vm.ReloadHabitAsync();
        }
    }
}