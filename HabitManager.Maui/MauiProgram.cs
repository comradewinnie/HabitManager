using HabitManager.Maui.Data;
using HabitManager.Maui.ViewModels;
using HabitManager.Maui.Views;
using Microsoft.Extensions.Logging;
using System.Net.Security;

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

            var handler = new SocketsHttpHandler  // MI ieteica izmantot šo, lai labāk strādātu uz Android
            {
                SslOptions = new SslClientAuthenticationOptions
                {
                    RemoteCertificateValidationCallback = (sender, certificate, chain, errors) => true  // izslēdzam sertifikatu pārbaudi
                },
                ConnectTimeout = TimeSpan.FromSeconds(5)  // vajadzīgs, jo SocketsHttpHandler pēc noklusējuma nav taimauta
            };

            builder.Services.AddSingleton<IUserService, TestUserService>();
            builder.Services.AddSingleton<IHabitProvider, HabitApiService>();
            builder.Services.AddSingleton(new HttpClient(handler)
            {
                BaseAddress = new Uri("http://192.168.0.149:5146/"),
                Timeout = TimeSpan.FromSeconds(5)
            });

            builder.Services.AddTransient<UncompletedHabitListViewModel>();
            builder.Services.AddTransient<CompletedHabitListViewModel>();
            builder.Services.AddTransient<ArchivedHabitListViewModel>();
            builder.Services.AddTransient<HabitFormViewModel>();
            builder.Services.AddTransient<HabitDetailsViewModel>();

            builder.Services.AddTransient<UncompletedHabitListPage>();
            builder.Services.AddTransient<CompletedHabitListPage>();
            builder.Services.AddTransient<ArchivedHabitListPage>();
            builder.Services.AddTransient<HabitFormPage>();
            builder.Services.AddTransient<HabitDetailsPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
