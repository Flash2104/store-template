using Store.Service.Contracts.Category;

namespace StoreApi.Models.Category.UpdateTree;

public class UpdateCategoryTreeRequestDto
{
    public UpdateCategoryTreeRequestDto(CategoryTreeData tree, List<int>? removedItemIds)
    {
        Tree = tree;
        RemovedItemIds = removedItemIds;
    }

    public CategoryTreeData Tree { get; set; }
    public List<int>? RemovedItemIds { get; }
}