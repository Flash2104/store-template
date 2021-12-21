using Store.Service.Contracts;
using Store.Service.Contracts.Team;
using Store.Service.Contracts.Team.UpdateMainInfo;
using StoreApi.Models.Team.GetCurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApi.AuthPolicies;
using StoreApi.Models;
using StoreApi.Models.TeamModify.UpdateMainInfo;

namespace StoreApi.Controllers
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
