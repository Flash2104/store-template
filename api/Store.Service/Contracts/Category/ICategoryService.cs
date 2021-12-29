using Store.Service.Contracts.Category.CreateTree;
using Store.Service.Contracts.Category.GetTree;
using Store.Service.Contracts.Category.ListTrees;
using Store.Service.Contracts.Category.UpdateTree;

namespace Store.Service.Contracts.Category;

public interface ICategoryService
{
    Task<ListCategoryTreesResponse> ListTrees();

    Task<GetCategoryTreeResponse> GetTree(GetCategoryTreeRequest request);

    Task<CreateCategoryTreeResponse> CreateTree(CreateCategoryTreeRequest request);

    Task<UpdateCategoryTreeResponse> UpdateTree(UpdateCategoryTreeRequest request);
}