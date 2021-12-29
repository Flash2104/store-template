using Microsoft.Extensions.Logging;
using Store.Data.Entity;
using Store.Service.Common;
using Store.Service.Contracts;
using Store.Service.Contracts.Store;
using Store.Service.Contracts.Store.Get;
using Store.Service.Contracts.Store.Update;
using Store.Service.Exceptions;

namespace Store.Service.Implementations.Store;

public class StoreService : IStoreService
{
    private readonly ILogger<StoreService> _logger;
    private readonly ICorrelationService _correlationService;
    private readonly IDataService _dataService;

    public StoreService(ILogger<StoreService> logger, ICorrelationService correlationService, IDataService dataService)
    {
        _logger = logger;
        _correlationService = correlationService;
        _dataService = dataService;
    }

    public async Task<GetStoreResponse> Get()
    {
        var userId = _correlationService.GetUserId();
        var logPath = $"{userId} {nameof(StoreService)} {nameof(Get)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");

        var res = await _dataService.Store.GetAsync();
        if (res == null)
        {
            throw new AirSoftBaseException(ErrorCodes.StoreService.StoreNotFound, "Магазин не найден");
        }

        return new GetStoreResponse(res.Title, res.Logo);
    }

    public async Task<UpdateStoreResponse> Update(UpdateStoreRequest request)
    {
        var userId = _correlationService.GetUserId();
        var logPath = $"{userId} {nameof(StoreService)} {nameof(Update)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        if (userId == null)
        {
            throw new AirSoftBaseException(ErrorCodes.StoreService.EmptyUserId, "Пустой id пользователя");
        }
        DbUser? dbUser = await _dataService.Users.GetAsync(x => x.Id == userId);

        if (dbUser == null)
        {
            throw new AirSoftBaseException(ErrorCodes.StoreService.UserNotFound, "Пользователь не найден");
        }
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new AirSoftBaseException(ErrorCodes.StoreService.EmptyTitle, "Пустое название магазина");
        }
        var res = await _dataService.Store.GetAsync();
        if (res == null)
        {
            throw new AirSoftBaseException(ErrorCodes.StoreService.StoreNotFound, "Магазин не найден");
        }

        res.Title = request.Title;
        res.Logo = request.Logo;
        _dataService.Store.Update(res);
        await _dataService.SaveAsync();
        return new UpdateStoreResponse(res.Title, res.Logo);
    }
}