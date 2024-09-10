using Microsoft.Extensions.Configuration;
using System.Reflection;
using DTKH2024.SbinSolution.Core;

namespace DTKH2024.SbinSolution.Mobile.MAUI
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
                });

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif
            ApplicationBootstrapper.InitializeIfNeeds<SbinSolutionMobileMAUIModule>();

            var app = builder.Build();
            return app;
        }
    }
}