using Store.Service.Contracts.Category.UpdateTree;

namespace StoreApi.Models.Category.CreateTree;

public class CreateCategoryTreeRequestDto
{
    public CreateCategoryTreeRequestDto(UpdateCategoryTreeData tree)
    {
        Tree = tree;
    }

    public UpdateCategoryTreeData Tree { get; set; }
}