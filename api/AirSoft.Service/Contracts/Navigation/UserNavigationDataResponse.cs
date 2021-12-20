
namespace AirSoft.Service.Contracts.Navigation;

public class UserNavigationDataResponse
{
    public List<UserNavigationData> Navigations { get; }

    public UserNavigationDataResponse(List<UserNavigationData>? navigations)
    {
        Navigations = navigations ?? new List<UserNavigationData>();
    }
}

public class UserNavigationData
{
    public Guid Id { get; }

    public string Title { get; }

    public bool IsDefault { get; }

    public List<NavigationItem>? NavItems { get; }

    public UserNavigationData(Guid id, string title, List<NavigationItem>? navItems, bool isDefault)
    {
        Id = id;
        Title = title;
        NavItems = navItems;
        IsDefault = isDefault;
    }
}

public class NavigationItem
{
    public int Id { get; }
    public string Path { get; }
    public string Title { get; }
    public string? Icon { get; }

    public int Order { get; }

    public bool? Disabled { get; }
    public List<NavigationItem> Children { get; }

    public NavigationItem(int id, string path, string title, string? icon, int order, List<NavigationItem> children, bool? disabled)
    {
        Id = id;
        Path = path;
        Title = title;
        Icon = icon;
        Children = children;
        Disabled = disabled;
        Order = order;
    }
}