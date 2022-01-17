namespace Store.Service.Contracts.Category.UpdateTree;

public class UpdateCategoryTreeResponse
{
    public UpdateCategoryTreeResponse(CategoryTreeData tree)
    {
        Tree = tree;
    }

    public CategoryTreeData Tree { get; set; }
}