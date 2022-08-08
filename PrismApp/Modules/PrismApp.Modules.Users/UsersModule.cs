using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using PrismApp.Core;

namespace PrismApp.Modules.Users;

public class UsersModule : IModule
{
    private readonly IRegionManager _regionManager;

    public UsersModule(IRegionManager regionManager)
    {
        _regionManager = regionManager;
    }

    public void OnInitialized(IContainerProvider containerProvider)
    {
        _regionManager.RequestNavigate(RegionNames.ContentRegion, "UsersList");
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<Views.UsersList>();
    }
}