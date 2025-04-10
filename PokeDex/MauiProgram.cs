﻿using Microsoft.Extensions.Logging;
using PokeDex.Services;
using PokeDex.ViewModels;

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

		// Registro i servizi
		builder.Services.AddHttpClient<IBaseRequest, PokemonService>();
        builder.Services.AddTransient<MainPageVM>();
        builder.Services.AddTransient<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
