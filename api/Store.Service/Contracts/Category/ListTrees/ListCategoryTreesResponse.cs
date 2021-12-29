namespace Store.Service.Contracts.Category.ListTrees;

public class ListCategoryTreesResponse
{
    public ListCategoryTreesResponse(List<CategoryTreeData>? trees)
    {
        Trees = trees ?? new List<CategoryTreeData>();
    }

    public List<CategoryTreeData> Trees { get; set; }
}