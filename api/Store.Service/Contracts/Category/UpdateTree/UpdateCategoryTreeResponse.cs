namespace Store.Service.Contracts.Category.UpdateTree;

public class UpdateCategoryTreeResponse
{
    public UpdateCategoryTreeResponse(UpdateCategoryTreeData tree)
    {
        Tree = tree;
    }

    public UpdateCategoryTreeData Tree { get; set; }
}