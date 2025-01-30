using Microsoft.Maui.LifecycleEvents;
using Microsoft.Identity.Client;
using Microsoft.Extensions.DependencyInjection;
using XGOMobile.Services.Models;
using XGOMobile.ViewModels;
using XGOMobile.Views;

namespace XGOMobile
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
            builder.Services.AddSingleton<AuthenticationService>();
            builder.Services.AddSingleton<HttpUriBuilder>();
            builder.Services.AddSingleton<HttpClientService>();
            builder.Services.AddSingleton<MessagingService>();
            builder.Services.AddSingleton<HttpClientServiceHelper>();
            builder.Services.AddScoped<CategoriesViewModel>();
            builder.Services.AddScoped<Categories>();
            builder.Services.AddScoped<ProductsViewModel>();
            builder.Services.AddScoped<Products>();

            return builder.Build();
        }
    }
}