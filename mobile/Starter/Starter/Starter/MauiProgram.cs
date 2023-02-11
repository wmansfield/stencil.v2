using Microsoft.Maui.Hosting;
using Stencil.Maui;

namespace Starter;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<MainApp>(app => new MainApp())
			.ConfigureFonts(fonts =>
			{
                fonts.AddFont("LibreFranklin-Regular.ttf", "LibreFranklin-Regular"); // included in stencil
                fonts.AddFont("LibreFranklin-Italic.ttf", "LibreFranklin-Italic"); // included in stencil
                fonts.AddFont("LibreFranklin-Bold.ttf", "LibreFranklin-Bold"); // included in stencil
                fonts.AddFont("LibreFranklin-BoldItalic.ttf", "LibreFranklin-BoldItalic"); // included in stencil

            })
			.ConfigureStencil();

		return builder.Build();
	}
}
