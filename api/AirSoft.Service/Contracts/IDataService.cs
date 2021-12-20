using AirSoft.Data.Entity;
using AirSoft.Service.Repositories;

namespace AirSoft.Service.Contracts;

public interface IDataService
{
    UserRepository Users { get; }
    MemberRepository Member { get; }
    TeamRepository Team { get; }

    GenericRepository<DbUserRole> UserRoles { get; }

    GenericRepository<DbUserNavigation> UserNavigations { get; }

    GenericRepository<DbNavigationItem> NavigationItems { get; }

    public CitiesRepository Cities { get; }

    Task<bool> SaveAsync();
}