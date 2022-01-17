namespace Store.Service.Contracts.Category.CreateTree;

public class CreateCategoryTreeResponse
{
    public CreateCategoryTreeResponse(CategoryTreeData tree)
    {
        Tree = tree;
    }

    public CategoryTreeData Tree { get; set; }
}