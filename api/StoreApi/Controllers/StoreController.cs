using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Service.Contracts;
using Store.Service.Contracts.Store;
using Store.Service.Contracts.Store.Update;
using StoreApi.AuthPolicies;
using StoreApi.Models;
using StoreApi.Models.Store.Get;
using StoreApi.Models.Store.Update;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = RolesConst.Administrator)]
    public class StoreController : RootController
    {
        private readonly ILogger<StoreController> _logger;
        private readonly IStoreService _storeService;
        private readonly ICorrelationService _correlationService;

        public StoreController(ILogger<StoreController> logger, IStoreService storeService, ICorrelationService correlationService) : base(logger)
        {
            _logger = logger;
            _storeService = storeService;
            _correlationService = correlationService;
        }

        [HttpGet("get-info")]
        [AllowAnonymous]
        public async Task<ServerResponseDto<GetStoreResponseDto>> GetDefault()
        {
            var logPath = $"{nameof(StoreController)} {nameof(GetDefault)} | ";
            return await HandleRequest(
                _storeService.Get,
                res => new GetStoreResponseDto(res.Title, res.Logo),
                logPath
            );
        }

        [HttpPost("update")]
        [Authorize(Roles = RolesConst.Administrator)]
        public async Task<ServerResponseDto<UpdateStoreResponseDto>> Update([FromBody] UpdateStoreRequestDto request)
        {
            var logPath = $"{_correlationService.GetUserId()}.{nameof(StoreController)} {nameof(Update)} | ";
            return await HandleRequest(
                _storeService.Update,
                request,
                dto => new UpdateStoreRequest(dto.Title, dto.Logo),
                res => new UpdateStoreResponseDto(res.Title, res.Logo),
                logPath
            );
        }
    }
}
