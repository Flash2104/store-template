using AirSoft.Service.Contracts;
using AirSoft.Service.Contracts.Navigation;
using AirSoft.Service.Contracts.Team;
using AirSoftApi.Models;
using AirSoftApi.Models.Navigation;
using AirSoftApi.Models.Team.GetCurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirSoftApi.Controllers
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
