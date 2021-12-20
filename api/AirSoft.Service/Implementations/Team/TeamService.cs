using System.Reflection;
using AirSoft.Data.Entity;
using AirSoft.Service.Common;
using AirSoft.Service.Contracts;
using AirSoft.Service.Contracts.Models;
using AirSoft.Service.Contracts.Team;
using AirSoft.Service.Contracts.Team.Create;
using AirSoft.Service.Contracts.Team.Delete;
using AirSoft.Service.Contracts.Team.Get;
using AirSoft.Service.Contracts.Team.GetCurrent;
using AirSoft.Service.Contracts.Team.UpdateMainInfo;
using AirSoft.Service.Contracts.User;
using AirSoft.Service.Exceptions;
using Microsoft.Extensions.Logging;

namespace AirSoft.Service.Implementations.Team;

public class TeamService : ITeamService
{
    private readonly ILogger<TeamService> _logger;
    private readonly ICorrelationService _correlationService;
    private readonly IDataService _dataService;
    private readonly IUserService _userService;

    public TeamService(
        ILogger<TeamService> logger, 
        ICorrelationService correlationService, 
        IDataService dataService,
        IUserService userService
        )
    {
        _logger = logger;
        _correlationService = correlationService;
        _dataService = dataService;
        _userService = userService;
    }

    public async Task<GetCurrentTeamResponse> GetCurrent()
    {
        var userId = _correlationService.GetUserId();
        var logPath = $"{userId} {nameof(TeamService)} {nameof(GetCurrent)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        if (!userId.HasValue)
        {
            throw new AirSoftBaseException(ErrorCodes.TeamService.EmptyUserId, "Пустой идентификатор пользователя", logPath);
        }
        DbTeam? dbTeam = await _dataService.Team.GetByUserAsync(userId.GetValueOrDefault());

        //if (dbTeam == null)
        //{
        //    throw new AirSoftBaseException(ErrorCodes.TeamService.NotFound, "Команда пользователя не найдена");
        //}

        return new GetCurrentTeamResponse(MapToTeamData(dbTeam));
    }


    public Task<GetByIdTeamResponse> Get(GetByIdTeamRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<CreateTeamResponse> Create(CreateTeamRequest request)
    {
        var userId = _correlationService.GetUserId();
        var logPath = $"{userId} {nameof(TeamService)} {nameof(GetCurrent)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        if (!userId.HasValue)
        {
            throw new AirSoftBaseException(ErrorCodes.TeamService.EmptyUserId, "Пустой идентификатор пользователя", logPath);
        }

        DbMember? dbMember = await _dataService.Member.GetByUserAsync(userId.GetValueOrDefault());

        if (dbMember == null)
        {
            throw new AirSoftBaseException(ErrorCodes.MemberService.NotFound, "Профиль не найден", logPath);
        }
        DbTeam? dbTeam = await _dataService.Team.GetByUserAsync(userId.GetValueOrDefault());

        if (dbTeam != null)
        {
            throw new AirSoftBaseException(ErrorCodes.TeamService.AlreadyExist, "У пользователя уже есть команда", logPath);
        }
        string? root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        dbTeam = new DbTeam
        {
            LeaderId = dbMember.Id,
            FoundationDate = request.FoundationDate,
            CityAddressId = request.CityId,
            Title = request.Title,
            Avatar = request.Avatar ?? await File.ReadAllBytesAsync(root + "\\InitialData\\team.png")
        };
        var created = await this._dataService.Team.Create(dbTeam);
        await _userService.SetUserTeamManager(userId.GetValueOrDefault());
        await _dataService.SaveAsync();
        _logger.Log(LogLevel.Information, $"{logPath} Team created: {dbTeam.Id}.");
        return new CreateTeamResponse(MapToTeamData(created));
    }

    public async Task<UpdateTeamMainInfoResponse> UpdateMainInfo(UpdateTeamMainInfoRequest request)
    {
        var userId = _correlationService.GetUserId();
        var logPath = $"{userId} {nameof(TeamService)} {nameof(UpdateMainInfo)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        if (!userId.HasValue)
        {
            throw new AirSoftBaseException(ErrorCodes.TeamService.EmptyUserId, "Пустой идентификатор пользователя", logPath);
        }
        DbTeam? dbTeam = await _dataService.Team.GetByUserAsync(userId.GetValueOrDefault());

        if (dbTeam == null)
        {
            throw new AirSoftBaseException(ErrorCodes.TeamService.NotFound, "Команда не найдена", logPath);
        }
        dbTeam.FoundationDate = request.FoundationDate;
        dbTeam.CityAddressId = request.CityId;

        dbTeam.Title = request.Title;

        this._dataService.Team.Update(dbTeam);
        await _dataService.SaveAsync();
        _logger.Log(LogLevel.Information, $"{logPath} Team Main info updated: {dbTeam.Id}.");
        return new UpdateTeamMainInfoResponse(MapToTeamData(dbTeam!));
    }

    public Task Delete(DeleteTeamRequest request)
    {
        throw new NotImplementedException();
    }
    
    private TeamData? MapToTeamData(DbTeam? dbTeam)
    {
        return dbTeam != null ? new TeamData(
            dbTeam.Id,
            dbTeam.Title,
            dbTeam.CityAddress?.CityAddress,
            dbTeam.FoundationDate,
            dbTeam.Avatar,
            dbTeam.Members?
                .Select(x => new MemberViewData(
                    x.Id,
                    x.Name,
                    x.Surname,
                    x.CityAddress?.CityAddress,
                    x.About,
                    x.Avatar,
                    x.Id == dbTeam.LeaderId,
                    x.TeamMemberRoles?.Select(y => new ReferenceData<Guid>(
                        y.Id,
                        y.Title ?? throw new AirSoftBaseException(ErrorCodes.TeamService.EmptyRoleTitle, "Пустое имя роли члена команды"),
                        y.Rank)
                    ).ToList()))
                .ToList(),
            dbTeam.TeamRoles?.Select(x => new ReferenceData<Guid>(x.Id, x.Title, x.Rank)).ToList()
        ) : null;
    }
}