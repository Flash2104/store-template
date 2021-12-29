using Store.Service.Contracts.Category;

namespace StoreApi.Models.Category.ListTrees;

public class ListCategoryTreesResponseDto
{
    public ListCategoryTreesResponseDto(List<CategoryTreeData>? trees)
    {
        Trees = trees ?? new List<CategoryTreeData>();
    }

    public List<CategoryTreeData> Trees { get; set; }
}