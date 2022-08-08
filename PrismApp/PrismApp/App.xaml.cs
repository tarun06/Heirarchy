using System.Windows;
using Microsoft.Extensions.Configuration;
using Prism.Ioc;
using Prism.Modularity;
using PrismApp.Modules.Users;
using PrismApp.Modules.Users.Models;
using PrismApp.Services;
using PrismApp.Services.Interfaces;
using PrismApp.Views;

namespace PrismApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override Window CreateShell()
    {
        AppUnhandledException.HandlingUnhandledExceptions();
        return Container.Resolve<MainWindow>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<IMessageService, MessageService>();
        containerRegistry.RegisterSingleton<ISystemFolders, SystemFolders>();
        containerRegistry.Register<HttpClientBase>();
        containerRegistry.RegisterSingleton<IUserService<SuperviserInfo>, UserService<SuperviserInfo>>();
        containerRegistry.RegisterSingleton<IAppConfiguration, AppConfiguration>();
        containerRegistry.RegisterSingleton<IConfiguration>(() => ApplicationBootstrapper.BuildConfiguration(Container.Resolve<ISystemFolders>()));
        ApplicationBootstrapper.InitializeLogging(Container.Resolve<IConfiguration>(), Container.Resolve<ISystemFolders>());
    }

    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        moduleCatalog.AddModule<UsersModule>();
    }
}