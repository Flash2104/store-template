using AirSoft.Service.Contracts;
using AirSoft.Service.Contracts.Member;
using AirSoft.Service.Contracts.Member.Get;
using AirSoft.Service.Contracts.Member.Update;
using AirSoftApi.AuthPolicies;
using AirSoftApi.Models;
using AirSoftApi.Models.Member;
using AirSoftApi.Models.Member.GetCurrent;
using AirSoftApi.Models.Member.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirSoftApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MemberController : RootController
    {
        private readonly ILogger<MemberController> _logger;
        private readonly IMemberService _memberService;
        private readonly ICorrelationService _correlationService;

        public MemberController(ILogger<MemberController> logger, IMemberService memberService, ICorrelationService correlationService) : base(logger)
        {
            _logger = logger;
            _memberService = memberService;
            _correlationService = correlationService;
        }

        [HttpGet("get-current")]
        [Authorize]
        public async Task<ServerResponseDto<GetCurrentMemberResponseDto>> GetCurrent()
        {
            var logPath = $"{_correlationService.GetUserId()}.{nameof(MemberController)} {nameof(GetCurrent)} | ";
            return await HandleRequest(
                _memberService.GetCurrent,
                res => new GetCurrentMemberResponseDto(
                     new MemberDataDto(
                         res.MemberData!.Id,
                         res.MemberData.Name,
                         res.MemberData.Surname,
                         res.MemberData.BirthDate,
                         res.MemberData.City,
                         res.MemberData.Email,
                         res.MemberData.Phone,
                         res.MemberData.Avatar?.ToArray(),
                         res.MemberData.AvatarIcon?.ToArray(),
                         res.MemberData.Team,
                         res.MemberData.Roles
                         )
                    ),
                logPath
            );
        }

        [HttpGet("get/{id}")]
        [Authorize]
        public async Task<ServerResponseDto<GetCurrentMemberResponseDto>> Get(string id)
        {
            var logPath = $"{_correlationService.GetUserId()}.{nameof(MemberController)} {nameof(Get)} | ";
            if (!Guid.TryParse(id, out var guidId))
            {
                return new ServerResponseDto<GetCurrentMemberResponseDto>(new ErrorDto(400,
                    "Неверный идентификатор пользователя"));
            }
            return await HandleGetRequest(
                _memberService.Get,
                () => new GetByIdMemberRequest(guidId),
                res => new GetCurrentMemberResponseDto(
                    new MemberDataDto(
                        res.MemberData!.Id,
                        res.MemberData.Name,
                        res.MemberData.Surname,
                        res.MemberData.BirthDate,
                        res.MemberData.City,
                        res.MemberData.Email,
                        res.MemberData.Phone,
                        res.MemberData.Avatar?.ToArray(),
                        res.MemberData.AvatarIcon?.ToArray(),
                        res.MemberData.Team,
                        res.MemberData.Roles
                    )
                ),
                logPath
            );
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<ServerResponseDto<UpdateMemberResponseDto>> Update(UpdateMemberRequestDto request)
        {
            var logPath = $"{_correlationService.GetUserId()}.{nameof(MemberController)} {nameof(Get)} | ";
            return await HandleRequest(
                _memberService.Update,
                request,
                (UpdateMemberRequestDto dto) => new UpdateMemberRequest(
                    dto.Id,
                    dto.Name,
                    dto.Surname,
                    dto.BirthDate,
                    dto.CityId
                    ),
                res => new UpdateMemberResponseDto(
                    new MemberDataDto(
                        res.MemberData!.Id,
                        res.MemberData.Name,
                        res.MemberData.Surname,
                        res.MemberData.BirthDate,
                        res.MemberData.City,
                        res.MemberData.Email,
                        res.MemberData.Phone,
                        res.MemberData.Avatar?.ToArray(),
                        res.MemberData.AvatarIcon?.ToArray(),
                        res.MemberData.Team,
                        res.MemberData.Roles
                    )
                ),
                logPath
            );
        }
    }
}
