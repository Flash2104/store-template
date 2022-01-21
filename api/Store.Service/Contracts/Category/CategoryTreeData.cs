namespace Store.Service.Contracts.Category;

public class CategoryTreeData
{
    public CategoryTreeData(int id, string title, bool isDefault, List<CategoryItemData>? items = null)
    {
        Id = id;
        Title = title;
        IsDefault = isDefault;
        Items = items ?? new List<CategoryItemData>();
    }

    public int Id { get; }

    public string Title { get; }

    public bool IsDefault { get; }

    public List<CategoryItemData> Items { get; }
}

public class CategoryItemData
{
    public CategoryItemData(int id, string title, string? icon, int order, bool? isDisabled, List<CategoryItemData>? children)
    {
        Id = id;
        Title = title;
        Icon = icon;
        Order = order;
        IsDisabled = isDisabled;
        Children = children ?? new List<CategoryItemData>();
    }

    public int Id { get; }

    public string Title { get; }

    public string? Icon { get; }

    public int Order { get; }

    public bool? IsDisabled { get; }

    public List<CategoryItemData> Children { get; }
}