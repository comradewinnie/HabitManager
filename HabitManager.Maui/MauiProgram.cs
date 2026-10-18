using HabitManager.Maui.Data;
using HabitManager.Maui.ViewModels;
using HabitManager.Maui.Views;
using Microsoft.Extensions.Logging;

namespace HabitManager.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<IUserService, TestUserService>();
            builder.Services.AddSingleton<IHabitProvider, TestHabitProvider>();

            builder.Services.AddTransient<UncompletedHabitListViewModel>();
            builder.Services.AddTransient<CompletedHabitListViewModel>();
            builder.Services.AddTransient<ArchivedHabitListViewModel>();

            builder.Services.AddTransient<UncompletedHabitListPage>();
            builder.Services.AddTransient<CompletedHabitListPage>();
            builder.Services.AddTransient<ArchivedHabitListPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
