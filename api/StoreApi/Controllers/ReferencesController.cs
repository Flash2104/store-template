using Store.Service.Contracts;
using Store.Service.Contracts.References;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Models;
using StoreApi.Models.References.Cities;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReferencesController : RootController
    {
        private readonly ILogger<ReferencesController> _logger;
        private readonly IReferenceService _referenceService;
        private readonly ICorrelationService _correlationService;

        public ReferencesController(ILogger<ReferencesController> logger, IReferenceService referenceService, ICorrelationService correlationService) : base(logger)
        {
            _logger = logger;
            _referenceService = referenceService;
            _correlationService = correlationService;
        }

        [HttpGet("cities")]
        public async Task<ServerResponseDto<GetCityReferencesResponseDto>> GetCities()
        {
            var logPath = $"{_correlationService.GetUserId()}.{nameof(ReferencesController)} {nameof(GetCities)} | ";
            await Task.Delay(1000);
            return await HandleRequest(
                _referenceService.GetCities,
                res => new GetCityReferencesResponseDto(res.Regions),
                logPath
            );
        }
    }
}
