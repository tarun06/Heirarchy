using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Commands;
using Prism.Regions;
using PrismApp.Core.Mvvm;
using PrismApp.Modules.Users.Models;
using PrismApp.Resources;
using PrismApp.Services.Interfaces;
using Serilog;

namespace PrismApp.Modules.Users.ViewModels;

public class UsersListViewModel : RegionViewModelBase
{
    private readonly IAppConfiguration _appConfiguration;
    private readonly IUserService<SuperviserInfo> _userService;
    private ObservableCollection<SuperviserInfo> _items = new();

    public UsersListViewModel(IRegionManager regionManager, IUserService<SuperviserInfo> userService, IAppConfiguration appConfiguration)
        : base(regionManager)
    {
        _userService = userService
            ?? throw new ArgumentNullException(string.Format(Global.IsNull, nameof(IUserService<SuperviserInfo>)));
        _appConfiguration = appConfiguration
            ?? throw new ArgumentNullException(string.Format(Global.IsNull, nameof(IUserService<IAppConfiguration>)));

        LoadCommand = new DelegateCommand(OnLoad);
    }

    public ObservableCollection<SuperviserInfo> Items
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }

    public DelegateCommand LoadCommand { get; set; }

    private static ILogger Logger { get; } = Log.ForContext<UsersListViewModel>();

    public ICollection<SuperviserInfo> GetMembers(IEnumerable<SuperviserInfo> superviserInfos, int parentId)
    {
        return superviserInfos
                .Where(c => c.SupervisorId == parentId)
                .Select(c => CreateSuperViserInfo(superviserInfos, c))
                .ToList();
    }

    private SuperviserInfo CreateSuperViserInfo(IEnumerable<SuperviserInfo> superviserInfos, SuperviserInfo c)
    {
        return new SuperviserInfo
        {
            Id = c.Id,
            FirstName = c.FirstName,
            LastName = c.LastName,
            Email = c.Email,
            Title = c.Title,
            Members = GetMembers(superviserInfos, c.Id)
        };
    }

    private IEnumerable<SuperviserInfo> FlatToHierarchy(IEnumerable<SuperviserInfo> superviserInfos)
    {
        List<SuperviserInfo> output = new();
        foreach (var superviser in superviserInfos)
        {
            if (!superviser.SupervisorId.HasValue)
            {
                output.Add(CreateSuperViserInfo(superviserInfos, superviser));
            }
        }

        return output;
    }

    private async void OnLoad()
    {
        try
        {
            Items.Clear();
            var users =
               await _userService.GetUsers(_appConfiguration.ServiceUrl);
            Items.AddRange(FlatToHierarchy(users));
            Logger.Information("Supervisor data received");
        }
        catch (Exception ex)
        {
            Logger.Error($"An exception occur while getting supervisor data {ex}");
        }
    }
}