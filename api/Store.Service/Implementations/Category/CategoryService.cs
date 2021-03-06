using System.Security.Cryptography.X509Certificates;
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
        var categoryTrees = await _dataService.CategoryTrees.ListAsync(x => x.StoreId == 1);
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
        var categoryTree = await _dataService.CategoryTrees
            .GetIncludeAsync(x => x.Id == request.Id, y => y.CategoryItems!.Where(x => x.Deleted == null));
        if (categoryTree == null)
        {
            throw new AirSoftBaseException(ErrorCodes.CategoryService.CategoryTreeNotFound, "Дерево категорий не найдено");
        }
        return new GetCategoryTreeResponse(CollectTree(categoryTree));
    }

    public async Task<CreateCategoryTreeResponse> CreateTree(CreateCategoryTreeRequest request)
    {
        var userId = _correlationService.GetUserId();
        var logPath = $"{userId} {nameof(CategoryService)} {nameof(ListTrees)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        if (userId == null)
        {
            throw new AirSoftBaseException(ErrorCodes.CategoryService.EmptyUserId, "Пустой id пользователя");
        }
        if (request.Tree.Id != 0)
        {
            throw new AirSoftBaseException(ErrorCodes.CategoryService.CategoryTreeIdIsNotEmpty, "id дерева не пустой");
        }
        DbUser? dbUser = await _dataService.Users.GetAsync(x => x.Id == userId);
        if (dbUser == null)
        {
            throw new AirSoftBaseException(ErrorCodes.CategoryService.UserNotFound, "Пользователь не найден");
        }
        var categoryTree = await _dataService.CategoryTrees.GetAsync(x => x.Id == request.Tree.Id);
        if (categoryTree == null)
        {
            throw new AirSoftBaseException(ErrorCodes.CategoryService.CategoryTreeNotFound, "Дерево категорий не найдено");
        }

        var updatedItems = await UpdateCategoryTreeItems(request.Tree.Id, null, request.Tree.Items);
        return new CreateCategoryTreeResponse(new UpdateCategoryTreeData(request.Tree.Id, request.Tree.Title, request.Tree.IsDefault, updatedItems));
    }

    public async Task<UpdateCategoryTreeResponse> UpdateTree(UpdateCategoryTreeRequest request)
    {
        var userId = _correlationService.GetUserId();
        var logPath = $"{userId} {nameof(CategoryService)} {nameof(ListTrees)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        if (userId == null)
        {
            throw new AirSoftBaseException(ErrorCodes.CategoryService.EmptyUserId, "Пустой id пользователя");
        }
        if (request.Tree == null || request.Tree.Id == 0)
        {
            throw new AirSoftBaseException(ErrorCodes.CategoryService.CategoryTreeIdIsEmpty, "Пустой id дерева");
        }
        DbUser? dbUser = await _dataService.Users.GetAsync(x => x.Id == userId);
        if (dbUser == null)
        {
            throw new AirSoftBaseException(ErrorCodes.CategoryService.UserNotFound, "Пользователь не найден");
        }
        var categoryTree = await _dataService.CategoryTrees.GetAsync(x => x.Id == request.Tree.Id);
        if (categoryTree == null)
        {
            throw new AirSoftBaseException(ErrorCodes.CategoryService.CategoryTreeNotFound, "Дерево категорий не найдено");
        }

        if (request.RemovedItemIds != null && request.RemovedItemIds.Count > 0)
        {
            foreach (var removedItemId in request.RemovedItemIds)
            {
                _dataService.CategoryTreeItems.Delete(removedItemId);
            }

            await _dataService.SaveAsync();
        }

        var updatedItems = await UpdateCategoryTreeItems(request.Tree.Id, null, request.Tree.Items);
        return new UpdateCategoryTreeResponse(new UpdateCategoryTreeData(request.Tree.Id, request.Tree.Title, request.Tree.IsDefault, updatedItems));
    }

    private async Task<List<UpdateCategoryItemData>> UpdateCategoryTreeItems(int treeId, int? parentId, List<UpdateCategoryItemData> treeItems)
    {
        var result = new List<UpdateCategoryItemData>();
        foreach (var item in treeItems)
        {
            DbCategoryItem? dbItem = null;
            if (item.Id > 0)
            {
                dbItem = await _dataService.CategoryTreeItems.GetAsync(x => x.Id == item.Id) ?? new DbCategoryItem();
                FillDbItem(dbItem, item, parentId, treeId);
                _dataService.CategoryTreeItems.Update(dbItem);
            }
            else
            {
                dbItem = new DbCategoryItem() { CategoryTreeId = treeId };
                FillDbItem(dbItem, item, parentId, treeId);
                _dataService.CategoryTreeItems.Insert(dbItem);
            }
            await _dataService.SaveAsync();

            var children = item.Children.Count > 0
                ? await UpdateCategoryTreeItems(treeId, dbItem.Id, item.Children)
                : new List<UpdateCategoryItemData>();
            var resItem = new UpdateCategoryItemData(dbItem.Id, dbItem.Title, dbItem.Icon, dbItem.Order, dbItem.IsDisabled, item.TempId, children);
            result.Add(resItem);
        }

        return result;
    }

    private void FillDbItem(DbCategoryItem dbItem, UpdateCategoryItemData item, int? parentId, int treeId)
    {
        dbItem.Icon = item.Icon;
        dbItem.ParentId = parentId;
        dbItem.CategoryTreeId = treeId;
        dbItem.IsDisabled = item.IsDisabled ?? false;
        dbItem.Order = item.Order;
        dbItem.Title = item.Title;
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