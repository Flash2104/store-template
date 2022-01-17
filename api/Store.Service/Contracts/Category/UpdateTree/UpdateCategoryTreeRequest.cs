namespace Store.Service.Contracts.Category.UpdateTree;

public class UpdateCategoryTreeRequest
{
    public UpdateCategoryTreeRequest(CategoryTreeData tree, List<int>? removedItemIds)
    {
        Tree = tree;
        RemovedItemIds = removedItemIds;
    }

    public CategoryTreeData Tree { get; set; }
    public List<int>? RemovedItemIds { get; }
}