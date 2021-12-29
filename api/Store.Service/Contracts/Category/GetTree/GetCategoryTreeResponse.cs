namespace Store.Service.Contracts.Category.GetTree;

public class GetCategoryTreeResponse
{
    public GetCategoryTreeResponse(CategoryTreeData categoryTree)
    {
        CategoryTree = categoryTree;
    }

    public CategoryTreeData CategoryTree { get; set; }
}