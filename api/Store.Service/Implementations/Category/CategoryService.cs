using Microsoft.Extensions.Logging;
using Store.Data.Entity;
using Store.Data.Entity.Category;
using Store.Service.Common;
using Store.Service.Contracts;
using Store.Service.Contracts.Category;
using Store.Service.Contracts.Category.CreateTree;
using Store.Service.Contracts.Category.GetTree;
using Store.Service.Contracts.Category.ListTrees;
using Store.Service.Contracts.Category.UpdateTree;
using Store.Service.Exceptions;

namespace Store.Service.Implementations.Category;

public class CategoryService : ICategoryService
{
    private readonly ILogger<CategoryService> _logger;
    private readonly ICorrelationService _correlationService;
    private readonly IDataService _dataService;

    public CategoryService(ILogger<CategoryService> logger, ICorrelationService correlationService, IDataService dataService)
    {
        _logger = logger;
        _correlationService = correlationService;
        _dataService = dataService;
    }

    public async Task<ListCategoryTreesResponse> ListTrees()
    {
        var userId = _correlationService.GetUserId();
        var logPath = $"{userId} {nameof(CategoryService)} {nameof(ListTrees)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        if (userId == null)
        {
            throw new AirSoftBaseException(ErrorCodes.CategoryService.EmptyUserId, "Пустой id пользователя");
        }
        DbUser? dbUser = await _dataService.Users.GetAsync(x => x.Id == userId);
        if (dbUser == null)
        {
            throw new AirSoftBaseException(ErrorCodes.CategoryService.UserNotFound, "Пользователь не найден");
        }
        var categoryTrees = await _dataService.Categories.ListAsync(x => x.StoreId == 1);
        return new ListCategoryTreesResponse(categoryTrees
            .Select(x => new CategoryTreeData(x.Id, x.Title, x.IsDefault))
            .ToList());
    }

    public async Task<GetCategoryTreeResponse> GetTree(GetCategoryTreeRequest request)
    {
        var userId = _correlationService.GetUserId();
        var logPath = $"{userId} {nameof(CategoryService)} {nameof(ListTrees)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        if (userId == null)
        {
            throw new AirSoftBaseException(ErrorCodes.CategoryService.EmptyUserId, "Пустой id пользователя");
        }
        DbUser? dbUser = await _dataService.Users.GetAsync(x => x.Id == userId);
        if (dbUser == null)
        {
            throw new AirSoftBaseException(ErrorCodes.CategoryService.UserNotFound, "Пользователь не найден");
        }
        var categoryTree = await _dataService.Categories
            .GetAsync(x => x.Id == request.Id, "CategoryItems");
        if (categoryTree == null)
        {
            throw new AirSoftBaseException(ErrorCodes.CategoryService.CategoryNotFound, "Дерево категорий не найдено");
        }
        return new GetCategoryTreeResponse(CollectTree(categoryTree));
    }

    public Task<CreateCategoryTreeResponse> CreateTree(CreateCategoryTreeRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<UpdateCategoryTreeResponse> UpdateTree(UpdateCategoryTreeRequest request)
    {
        throw new NotImplementedException();
    }

    private CategoryTreeData CollectTree(DbCategoryTree dbCategoryTree)
    {
        var data = new List<CategoryItemData>();
        var res = new CategoryTreeData(dbCategoryTree.Id, dbCategoryTree.Title, dbCategoryTree.IsDefault, data);
        if (dbCategoryTree.CategoryItems == null)
        {
            return res;
        }
        var navTree = new Dictionary<int, List<CategoryItemData>>()
        {
            {0, data}
        };
        foreach (var dbItem in dbCategoryTree.CategoryItems)
        {
            var parentId = dbItem.ParentId ?? 0;
            if (!navTree.ContainsKey(dbItem.Id))
            {
                navTree[dbItem.Id] = new List<CategoryItemData>();
            }
            var item = new CategoryItemData(dbItem.Id, dbItem.Title, dbItem.Icon,
                dbItem.Order, dbItem.IsDisabled, navTree[dbItem.Id]);
            navTree[parentId].Add(item);
        }

        return res;
    }
}