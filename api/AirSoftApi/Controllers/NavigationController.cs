using Store.Service.Contracts;
using Store.Service.Contracts.Navigation;
using Store.Service.Contracts.Team;
using StoreApi.Models.Team.GetCurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Models;
using StoreApi.Models.Navigation;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NavigationController : RootController
    {
        private readonly ILogger<NavigationController> _logger;
        private readonly INavigationService _navigationService;
        private readonly ICorrelationService _correlationService;

        public NavigationController(ILogger<NavigationController> logger, INavigationService navigationService, ICorrelationService correlationService) : base(logger)
        {
            _logger = logger;
            _navigationService = navigationService;
            _correlationService = correlationService;
        }

        [HttpGet("get")]
        [Authorize]
        public async Task<ServerResponseDto<UserNavigationResponseDto>> GetUserNavigations()
        {
            var logPath = $"{_correlationService.GetUserId()}.{nameof(NavigationController)} {nameof(GetUserNavigations)} | ";
            return await HandleRequest(
                _navigationService.GetUserNavigations,
                res => new UserNavigationResponseDto(res.Navigations),
                logPath
            );
        }
    }
}
