using Store.Service.Contracts.Category.UpdateTree;

namespace Store.Service.Contracts.Category.CreateTree;

public class CreateCategoryTreeRequest
{
    public CreateCategoryTreeRequest(UpdateCategoryTreeData tree)
    {
        Tree = tree;
    }

    public UpdateCategoryTreeData Tree { get; set; }
}