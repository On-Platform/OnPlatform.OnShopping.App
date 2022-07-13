using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnPlatform.OnShopping.App
{
    internal class PrismStartup
    {
        public static void Configure(PrismAppBuilder builder)
        {
            builder.RegisterTypes(RegisterTypes)
                    .OnAppStart("NavigationPage/MainPage");
        }

        private static void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainPage>()
                         .RegisterInstance(SemanticScreenReader.Default);
        }
    }
}
