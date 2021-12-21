using System.Collections.ObjectModel;

namespace Store.Data.InitialData;

public static class RoleNavigationItemsConst
{
    public static readonly ReadOnlyCollection<int> PlayerNavItemIds = new ReadOnlyCollection<int>(new[]
    {
        1, 2, 3, 6
    });
    public static readonly ReadOnlyCollection<int> TeamManagerNavItemIds = new ReadOnlyCollection<int>(new[]
    {
        1, 2, 3, 4, 5, 6
    });
}