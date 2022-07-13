using OnPlatform.OnShopping.App.ViewModels;
using OnPlatform.OnShopping.App.Views;
using OnPlatform.Net.Auth.Configs;
using OnPlatform.Net.Auth.Abstractions;
using OnPlatform.OnShopping.App.Controls;
using OnPlatform.Net.Auth.Services;

namespace OnPlatform.OnShopping.App;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UsePrismApp<App>(prism =>
			{
				prism.RegisterTypes(container =>
				{
					container.RegisterForNavigation<MainPage, MainPage>();
					container.RegisterInstance<IOpenIdClientConfigs>(new DefaultOpenIdClientConfigs("onShoppingAuth", App.CallbackScheme, App.SignoutCallbackScheme, "openid onShoppingApi.read", "http://10.0.2.2:5079", new WebAuthenticatorBrowser(), "OnPlatform"));
                    container.RegisterSingleton<IIdentityService, IdentityService>();
                })
				.OnAppStart(navigationService => navigationService.CreateBuilder().AddNavigationPage().AddNavigationSegment<MainPage>().Navigate());
			})
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		return builder.Build();
	}
}