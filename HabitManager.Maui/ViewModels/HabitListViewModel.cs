using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabitManager.Domain;
using HabitManager.Maui.Data;
using HabitManager.Maui.Views;
using System.Collections.ObjectModel;

namespace HabitManager.Maui.ViewModels
{
    public abstract partial class HabitListViewModel : ObservableObject   // adaptēts pēc "UzdevumuParvaldnieks" piemēra
    {
        protected IHabitProvider _hp;
        protected IUserService _us;

        [ObservableProperty]
        protected ObservableCollection<Habit> _habits;

        public HabitListViewModel(IHabitProvider hp, IUserService us)
        {
            _hp = hp;
            _us = us;
            LoadHabits();  // lai rādītu sarakstu, kad pirmo reizi atvēram lapu
        }

        public abstract void LoadHabits();  // lai atjaunotu katru reizi, kad atgriežamies lapā

        [RelayCommand]
        public async Task ShowHabitDetailsAsync(Habit hb)
        {
            if (hb is null)
                return;

            await Shell.Current.GoToAsync($"{nameof(HabitDetailsPage)}", new Dictionary<string, object>
            {
                { "HabitToShow", hb }
            });
        }
    }
}
