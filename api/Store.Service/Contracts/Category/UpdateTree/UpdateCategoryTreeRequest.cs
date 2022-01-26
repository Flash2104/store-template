namespace Store.Service.Contracts.Category.UpdateTree;

public class UpdateCategoryTreeRequest
{
    public UpdateCategoryTreeRequest(UpdateCategoryTreeData tree, List<int>? removedItemIds)
    {
        Tree = tree;
        RemovedItemIds = removedItemIds;
    }

    public UpdateCategoryTreeData Tree { get; set; }
    public List<int>? RemovedItemIds { get; }
}

public class UpdateCategoryTreeData
{
    public UpdateCategoryTreeData(int id, string title, bool isDefault, List<UpdateCategoryItemData>? items = null)
    {
        Id = id;
        Title = title;
        IsDefault = isDefault;
        Items = items ?? new List<UpdateCategoryItemData>();
    }

    public int Id { get; }

    public string Title { get; }

    public bool IsDefault { get; }

    public List<UpdateCategoryItemData> Items { get; }
}

public class UpdateCategoryItemData
{
    public UpdateCategoryItemData(int id, string title, string? icon, int order, bool? isDisabled, bool? isExpanded, List<CategoryItemData>? children)
    {
        Id = id;
        Title = title;
        Icon = icon;
        Order = order;
        IsDisabled = isDisabled;
        IsExpanded = isExpanded;
        Children = children ?? new List<CategoryItemData>();
    }

    public int Id { get; }

    public string Title { get; }

    public string? Icon { get; }

    public int Order { get; }

    public bool? IsDisabled { get; }

    public bool? IsExpanded { get; }

    public List<CategoryItemData> Children { get; }
}