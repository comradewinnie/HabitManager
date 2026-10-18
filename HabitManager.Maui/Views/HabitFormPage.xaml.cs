using HabitManager.Domain;
using HabitManager.Maui.Data;
using HabitManager.Maui.ViewModels;

namespace HabitManager.Maui.Views;

public partial class HabitFormPage : ContentPage  // nokopēts no "UzdevumuParvaldnieks"
{
	public HabitFormPage(IHabitProvider hp, IUserService us)
	{
		InitializeComponent();
		BindingContext = new HabitFormViewModel(hp, us);
	}
}