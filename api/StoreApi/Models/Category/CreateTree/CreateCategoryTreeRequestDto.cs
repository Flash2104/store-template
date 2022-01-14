using Store.Service.Contracts.Category;

namespace StoreApi.Models.Category.CreateTree;

public class CreateCategoryTreeRequestDto
{
    public CreateCategoryTreeRequestDto(CategoryTreeData tree)
    {
        Tree = tree;
    }

    public CategoryTreeData Tree { get; set; }
}