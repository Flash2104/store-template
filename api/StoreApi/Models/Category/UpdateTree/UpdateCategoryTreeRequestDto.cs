using Store.Service.Contracts.Category;
using Store.Service.Contracts.Category.UpdateTree;

namespace StoreApi.Models.Category.UpdateTree;

public class UpdateCategoryTreeRequestDto
{
    public UpdateCategoryTreeRequestDto(UpdateCategoryTreeData tree, List<int>? removedItemIds)
    {
        Tree = tree;
        RemovedItemIds = removedItemIds;
    }

    public UpdateCategoryTreeData Tree { get; set; }
    public List<int>? RemovedItemIds { get; }
}