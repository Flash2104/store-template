using System.Reflection;
using AirSoft.Data.Entity;
using AirSoft.Data.InitialData;
using AirSoft.Service.Common;
using AirSoft.Service.Contracts;
using AirSoft.Service.Contracts.Member;
using AirSoft.Service.Contracts.Member.Create;
using AirSoft.Service.Contracts.Member.Delete;
using AirSoft.Service.Contracts.Member.Get;
using AirSoft.Service.Contracts.Member.GetByUserId;
using AirSoft.Service.Contracts.Member.GetCurrent;
using AirSoft.Service.Contracts.Member.Update;
using AirSoft.Service.Contracts.Models;
using AirSoft.Service.Exceptions;
using Microsoft.Extensions.Logging;

namespace AirSoft.Service.Implementations.Member;

public class MemberService : IMemberService
{
    private readonly ILogger<MemberService> _logger;
    private readonly ICorrelationService _correlationService;
    private readonly IDataService _dataService;

    public MemberService(ILogger<MemberService> logger, ICorrelationService correlationService, IDataService dataService)
    {
        _logger = logger;
        _correlationService = correlationService;
        _dataService = dataService;
    }

    public async Task<GetCurrentMemberResponse> GetCurrent()
    {
        var userId = _correlationService.GetUserId();
        var logPath = $"{userId} {nameof(MemberService)} {nameof(GetCurrent)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        if (!userId.HasValue)
        {
            throw new AirSoftBaseException(ErrorCodes.MemberService.EmptyUserId, "Пустой идентификатор пользователя");
        }
        DbMember? dbMember = await _dataService.Member.GetByUserAsync(userId.GetValueOrDefault());

        if (dbMember == null)
        {
            throw new AirSoftBaseException(ErrorCodes.MemberService.NotFound, "Профиль не найден");
        }

        return new GetCurrentMemberResponse(MapToMemberData(dbMember));
    }

    public async Task<GetByIdMemberResponse> Get(GetByIdMemberRequest request)
    {
        var userId = _correlationService.GetUserId();
        var logPath = $"{userId} {nameof(MemberService)} {nameof(Get)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        if (!userId.HasValue)
        {
            throw new AirSoftBaseException(ErrorCodes.MemberService.EmptyUserId, "Пустой идентификатор пользователя");
        }
        DbMember? dbMember = await _dataService.Member.GetAsync(x => x.UserId == userId && x.Id == request.Id);

        if (dbMember == null)
        {
            throw new AirSoftBaseException(ErrorCodes.MemberService.NotFound, "Профиль не найден");
        }

        _logger.Log(LogLevel.Information, $"{logPath} Member updated: {dbMember!.Id}.");
        return new GetByIdMemberResponse(MapToMemberData(dbMember));
    }

    private MemberData? MapToMemberData(DbMember? dbMember)
    {
        return dbMember != null ? new MemberData(
            dbMember.Id,
            dbMember.Name,
            dbMember.Surname,
            dbMember.BirthDate,
            dbMember.CityAddress?.City,
            dbMember.User?.Email,
            dbMember.User?.Phone,
            dbMember.Avatar?.ToArray(),
            dbMember.AvatarIcon?.ToArray(),
            dbMember.Team != null ? new ReferenceData<Guid>(dbMember.Team.Id, dbMember.Team.Title) : null,
            dbMember.TeamMemberRoles?.Select(x => new ReferenceData<Guid>(x.Id, x.Title, x.Rank)).ToList()
        ) : null;
    }

    public async Task<GetByUserIdMemberResponse> GetByUserId(Guid userId)
    {
        var logPath = $"{userId} {nameof(MemberService)} {nameof(GetByUserId)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        DbMember? dbMember = await _dataService.Member.GetAsync(x => x.UserId == userId);

        _logger.Log(LogLevel.Information, $"{logPath} Member updated: {dbMember!.Id}.");
        return new GetByUserIdMemberResponse(MapToMemberData(dbMember));
    }

    public async Task<CreateMemberResponse> Create(CreateMemberRequest request)
    {
        var logPath = $"{request.UserId} {nameof(MemberService)} {nameof(Create)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");

        DbMember? dbMember = await _dataService.Member.GetByUserAsync(request.UserId);
        if (dbMember != null)
        {
            throw new AirSoftBaseException(ErrorCodes.MemberService.AlreadyExist, "Профиль уже существует");
        }
        string? root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        byte[] avatar = await File.ReadAllBytesAsync(root + "\\InitialData\\member-default.png");
        byte[] avatarIcon = ImageHelper.CreateThumbnail(avatar, 120);
        dbMember = new DbMember()
        {
            Id = Guid.NewGuid(),
            Name = request.Name ?? "Новенький",
            Surname = request.Surname,
            CreatedBy = request.UserId,
            ModifiedBy = request.UserId,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            UserId = request.UserId,
            Avatar = avatar,
            AvatarIcon = avatarIcon
        };
        var created = this._dataService.Member.Insert(dbMember);
        if (created == null)
        {
            throw new AirSoftBaseException(ErrorCodes.AuthService.CreatedUserIsNull, "Созданный пользователь пустой");
        }

        await _dataService.SaveAsync();
        _logger.Log(LogLevel.Information, $"{logPath} Member created: {created!.Id}.");
        return new CreateMemberResponse(MapToMemberData(created));
    }

    public async Task<UpdateMemberResponse> Update(UpdateMemberRequest request)
    {
        var userId = _correlationService.GetUserId();
        var logPath = $"{userId} {nameof(MemberService)} {nameof(Update)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        if (!userId.HasValue)
        {
            throw new AirSoftBaseException(ErrorCodes.MemberService.EmptyUserId, "Пустой идентификатор пользователя");
        }
        DbMember? dbMember = await _dataService.Member.GetAsync(x => x.UserId == userId && x.Id == request.Id);

        if (dbMember == null)
        {
            throw new AirSoftBaseException(ErrorCodes.MemberService.NotFound, "Профиль не найден");
        }
        dbMember.BirthDate = request.BirthDate;
        dbMember.Name = request.Name;
        dbMember.Surname = request.Surname;
        dbMember.CityAddressId = request.CityId;
        this._dataService.Member.Update(dbMember);
        await _dataService.SaveAsync();
        _logger.Log(LogLevel.Information, $"{logPath} Member updated: {dbMember!.Id}.");
        return new UpdateMemberResponse(MapToMemberData(dbMember));
    }

    public async Task Delete(DeleteMemberRequest request)
    {
        var userId = _correlationService.GetUserId();
        var logPath = $"{userId} {nameof(MemberService)} {nameof(Update)}. | ";
        _logger.Log(LogLevel.Trace, $"{logPath} started.");
        if (!userId.HasValue)
        {
            throw new AirSoftBaseException(ErrorCodes.MemberService.EmptyUserId, "Пустой идентификатор пользователя");
        }

        DbMember? dbMember = await _dataService.Member.GetAsync(x => x.UserId == userId && x.Id == request.Id);

        if (dbMember == null)
        {
            throw new AirSoftBaseException(ErrorCodes.MemberService.NotFound, "Профиль не найден");
        }

        this._dataService.Member.Delete(dbMember);
        await _dataService.SaveAsync();
        _logger.Log(LogLevel.Information, $"{logPath} Member deleted: {request!.Id}.");
    }
}