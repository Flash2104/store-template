namespace Store.Service.Contracts.Category.CreateTree;

public class CreateCategoryTreeRequest
{
    public CreateCategoryTreeRequest(CategoryTreeData tree)
    {
        Tree = tree;
    }

    public CategoryTreeData Tree { get; set; }
}