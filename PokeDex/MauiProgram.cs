using Microsoft.Extensions.Logging;
using PokeDex.Services;
using PokeDex.ViewModels;
using PokeDex.Views;

namespace PokeDex;

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

		// Service Register
		builder.Services.AddSingleton<IPokemonService, PokemonService>();
        builder.Services.AddTransient<MainPageVM>();
        builder.Services.AddTransient<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
