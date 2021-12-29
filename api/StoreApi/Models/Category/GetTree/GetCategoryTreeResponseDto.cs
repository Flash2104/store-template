using Store.Service.Contracts.Category;

namespace StoreApi.Models.Category.GetTree;

public class GetCategoryTreeResponseDto
{
    public GetCategoryTreeResponseDto(CategoryTreeData tree)
    {
        Tree = tree;
    }

    public CategoryTreeData Tree { get; set; }
}