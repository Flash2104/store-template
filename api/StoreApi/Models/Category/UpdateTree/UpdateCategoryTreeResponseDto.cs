using Store.Service.Contracts.Category;
using Store.Service.Contracts.Category.UpdateTree;

namespace StoreApi.Models.Category.UpdateTree;

public class UpdateCategoryTreeResponseDto
{
    public UpdateCategoryTreeResponseDto(UpdateCategoryTreeData tree)
    {
        Tree = tree;
    }

    public UpdateCategoryTreeData Tree { get; set; }
}