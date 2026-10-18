using HabitManager.Maui.Views;

namespace HabitManager.Maui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(HabitFormPage), typeof(HabitFormPage));
        }
    }
}
