using AirSoft.Service.Contracts;
using AirSoft.Service.Contracts.Team;
using AirSoft.Service.Contracts.Team.UpdateMainInfo;
using AirSoftApi.AuthPolicies;
using AirSoftApi.Models;
using AirSoftApi.Models.Team.GetCurrent;
using AirSoftApi.Models.TeamModify.UpdateMainInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirSoftApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = RolesConst.TeamManager)]
    public class TeamModifyController : RootController
    {
        private readonly ILogger<TeamController> _logger;
        private readonly ITeamService _teamService;
        private readonly ICorrelationService _correlationService;

        public TeamModifyController(ILogger<TeamController> logger, ITeamService teamService, ICorrelationService correlationService) : base(logger)
        {
            _logger = logger;
            _teamService = teamService;
            _correlationService = correlationService;
        }

        [HttpPut("update-main-info")]
        [Authorize]
        public async Task<ServerResponseDto<UpdateTeamMainInfoResponseDto>> UpdateMainInfo([FromBody] UpdateTeamMainInfoRequestDto requestDto)
        {
            var logPath = $"{_correlationService.GetUserId()}.{nameof(TeamModifyController)} {nameof(UpdateMainInfo)} | ";
            return await HandleRequest(
                _teamService.UpdateMainInfo,
                requestDto,
                dto => new UpdateTeamMainInfoRequest(dto.Id, dto.Title, dto.CityId, dto.FoundationDate),
                res => new UpdateTeamMainInfoResponseDto(res.TeamData),
                logPath
            );
        }
    }
}
