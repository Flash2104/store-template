using Store.Service.Contracts.Category;

namespace StoreApi.Models.Category.UpdateTree;

public class UpdateCategoryTreeRequestDto
{
    public UpdateCategoryTreeRequestDto(CategoryTreeData tree)
    {
        Tree = tree;
    }

    public CategoryTreeData Tree { get; set; }
}