using AirSoft.Data.Entity;
using AirSoft.Service.Common;
using AirSoft.Service.Contracts;
using AirSoft.Service.Contracts.Models;
using AirSoft.Service.Contracts.Navigation;
using AirSoft.Service.Contracts.Team;
using AirSoft.Service.Contracts.Team.GetCurrent;
using AirSoft.Service.Exceptions;
using Microsoft.Extensions.Logging;

namespace AirSoft.Service.Implementations;

public class NavigationService : INavigationService
{
    private readonly ILogger<NavigationService> _logger;
    private readonly ICorrelationService _correlationService;
    private readonly IDataService _dataService;

    public NavigationService(
        ILogger<NavigationService> logger,
        ICorrelationService correlationService,
        IDataService dataService)
    {
        _logger = logger;
        _correlationService = correlationService;
        _dataService = dataService;
    }

    public async Task<UserNavigationDataResponse> GetUserNavigations()
    {
        var userId = _correlationService.GetUserId();
        var logPath = $"{userId} {nameof(NavigationService)} {nameof(GetUserNavigations)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        if (!userId.HasValue)
        {
            throw new AirSoftBaseException(ErrorCodes.NavigationService.EmptyUserId, "Пустой идентификатор пользователя");
        }
        DbUser? user = await _dataService.Users.GetAsync(x => userId.GetValueOrDefault() == x.Id);
        if (user == null)
        {
            throw new AirSoftBaseException(ErrorCodes.NavigationService.UserNotFound, "Пользователь не найден");
        }
        if (user.UserRoles == null || user.UserRoles.Count == 0)
        {
            throw new AirSoftBaseException(ErrorCodes.NavigationService.UserRolesNotFound, "Роли пользователя не найдены");
        }

        List<int> roleIds = user.UserRoles.Select(x => x.Id).ToList();
        List<DbUserNavigation> dbNavigations = await _dataService.UserNavigations.ListAsync(x => userId.GetValueOrDefault() == x.UserId)!;
        List<DbNavigationItem> dbAvailableItems =
            await _dataService.NavigationItems
                .ListAsync(x => x.Roles!.Any(r => roleIds.Contains(r.Id)), null, "Roles");
        if (dbNavigations.Count == 0)
        {
            throw new AirSoftBaseException(ErrorCodes.NavigationService.NavigationNotFound, "Не найдены навигации пользователя");
        }

        return MapTreeToResponse(dbNavigations, dbAvailableItems.Select(x => x.Id).ToList());
    }

    private UserNavigationDataResponse MapTreeToResponse(List<DbUserNavigation> dbNavigations, List<int> dbAvailableItemIds)
    {
        var data = new List<UserNavigationData>();
        foreach (var dbNavigation in dbNavigations)
        {
            if (dbNavigation?.NavigationItems == null || dbNavigation?.NavigationItems.Count == 0)
            {
                continue; // ToDO: throw new AirSoftBaseException(ErrorCodes.NavigationService.NavigationNotFound, "Навигация не содержит элементов");
            }
            var navTree = new Dictionary<int, List<NavigationItem>>()
            {
                {0, new List<NavigationItem>()}
            };

            foreach (var dbNavItem in dbNavigation!.NavigationItems)
            {
                if (!dbAvailableItemIds.Contains(dbNavItem.Id))
                {
                    continue;
                }
                var parentId = dbNavItem.ParentId ?? 0;
                if (!navTree.ContainsKey(dbNavItem.Id))
                {
                    navTree[dbNavItem.Id] = new List<NavigationItem>();
                }
                var item = new NavigationItem(dbNavItem.Id, dbNavItem.Path, dbNavItem.Title, dbNavItem.Icon,
                    dbNavItem.Order, navTree[dbNavItem.Id], dbNavItem.Disabled);
                navTree[parentId].Add(item);
            }
            data.Add(new UserNavigationData(dbNavigation.Id, dbNavigation.Title, navTree[0], dbNavigation.IsDefault));
        }

        return new UserNavigationDataResponse(data);
    }
}