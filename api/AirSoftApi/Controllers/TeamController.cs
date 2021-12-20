using AirSoft.Service.Contracts;
using AirSoft.Service.Contracts.Team;
using AirSoft.Service.Contracts.Team.Create;
using AirSoftApi.Models;
using AirSoftApi.Models.Team.Create;
using AirSoftApi.Models.Team.GetCurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirSoftApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeamController : RootController
    {
        private readonly ILogger<TeamController> _logger;
        private readonly ITeamService _teamService;
        private readonly ICorrelationService _correlationService;

        public TeamController(ILogger<TeamController> logger, ITeamService teamService, ICorrelationService correlationService) : base(logger)
        {
            _logger = logger;
            _teamService = teamService;
            _correlationService = correlationService;
        }

        [HttpGet("get-current")]
        [Authorize]
        public async Task<ServerResponseDto<GetCurrentTeamResponseDto>> GetCurrent()
        {
            var logPath = $"{_correlationService.GetUserId()}.{nameof(TeamController)} {nameof(GetCurrent)} | ";
            return await HandleRequest(
                _teamService.GetCurrent,
                res => new GetCurrentTeamResponseDto(res.TeamData),
                logPath
            );
        }
        
        [HttpPost("create")]
        [Authorize]
        public async Task<ServerResponseDto<CreateTeamResponseDto>> Create([FromBody] CreateTeamRequestDto requestDto)
        {
            var logPath = $"{_correlationService.GetUserId()}.{nameof(TeamController)} {nameof(Create)} | ";
            return await HandleRequest(
                _teamService.Create,
                requestDto,
                dto => new CreateTeamRequest(dto.Title, dto.CityId, dto.FoundationDate, dto.Avatar),
                res => new CreateTeamResponseDto(res.TeamData),
                logPath
            );
        }
    }
}
