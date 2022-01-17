using Store.Service.Contracts.Category;

namespace StoreApi.Models.Category.UpdateTree;

public class UpdateCategoryTreeResponseDto
{
    public UpdateCategoryTreeResponseDto(CategoryTreeData tree)
    {
        Tree = tree;
    }

    public CategoryTreeData Tree { get; set; }
}