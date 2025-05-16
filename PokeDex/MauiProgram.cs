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
				fonts.AddFont("MauiMaterialAssets.ttf", "MaterialAssets");
			});

		// Service Register
		builder.Services.AddSingleton<IPokemonService, PokemonService>();
		builder.Services.AddSingleton<ILoginService, LoginService>();
        builder.Services.AddSingleton<MainPageVM>();
        builder.Services.AddSingleton<LoginPageVm>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<LoadingPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
