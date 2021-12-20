using AirSoft.Data;
using AirSoft.Data.Entity;
using AirSoft.Service.Common;
using AirSoft.Service.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AirSoft.Service.Repositories;

public class TeamRepository : GenericRepository<DbTeam>
{
    private readonly DbSet<DbMember> _dbMembers;
    private readonly DbSet<DbTeamRole> _dbTeamRoles;

    public TeamRepository(IDbContext context) : base(context)
    {
        _dbMembers = context.Set<DbMember>();
        _dbTeamRoles = context.Set<DbTeamRole>();
    }


    public async Task<DbTeam?> GetByUserAsync(Guid userId)
    {
        var dbMember = await _dbMembers
            .Include(x => x.Team)
            .ThenInclude(x => x.CityAddress)
            .FirstOrDefaultAsync(x => x.UserId == userId).ConfigureAwait(false);
        if (dbMember == null)
        {
            throw new AirSoftBaseException(ErrorCodes.TeamRepository.MemberNotFound, "Профиль пользователя не найден");
        }
        return dbMember.Team;
    }

    public async Task<DbTeam> Create(DbTeam dbTeam)
    {
        var created = Insert(dbTeam);
        if (created == null || created.Id == Guid.Empty)
        {
            throw new AirSoftBaseException(ErrorCodes.TeamRepository.CreatedTeamIsNull, "Результат создания команды пустой или отсутствует идентификатор");
        }
        var teamRoleIds = new Dictionary<int, Guid>(Enum.GetValues<DefaultMemberRoleType>().Select(x => new KeyValuePair<int, Guid>((int)x, Guid.NewGuid())));
        await CreateDefaultTeamRolesAsync(created.Id, teamRoleIds);
        await AddMemberToTeam(created.LeaderId, created.Id);
        await AddTeamRoleToMember(created.LeaderId, teamRoleIds[(int) DefaultMemberRoleType.Командир]);
        return created;
    }

    private async Task AddMemberToTeam(Guid memberId, Guid teamId)
    {
        var dbMember = await _dbMembers.FirstOrDefaultAsync(x => x.Id == memberId).ConfigureAwait(false);
        if (dbMember == null)
        {
            throw new AirSoftBaseException(ErrorCodes.TeamRepository.MemberNotFound, "Профиль пользователя не найден");
        }

        dbMember.TeamId = teamId;
        _dbMembers.Update(dbMember);
    }

    private async Task AddTeamRoleToMember(Guid memberId, Guid teamRoleId)
    {
        var dbMember = await _dbMembers.FirstOrDefaultAsync(x => x.Id == memberId).ConfigureAwait(false);
        if (dbMember == null)
        {
            throw new AirSoftBaseException(ErrorCodes.TeamRepository.MemberNotFound, "Профиль пользователя не найден");
        }
        var role = await _dbTeamRoles.FindAsync(teamRoleId);
        if (role == null)
        {
            throw new AirSoftBaseException(ErrorCodes.TeamRolesRepository.TeamRoleNotFound, "Роль не найдена");
        }
        if (role.TeamId != dbMember.TeamId)
        {
            throw new AirSoftBaseException(ErrorCodes.TeamRolesRepository.MemberTeamNotEqualRoleTeam, "Роль не пренадлежит команде пользователя");
        }
        dbMember.TeamRolesToMembers = new List<DbTeamRolesToMembers>()
        {
            new DbTeamRolesToMembers()
            {
                MemberId = memberId,
                TeamRoleId = teamRoleId
            }
        };
        _dbMembers.Update(dbMember);
    }

    private async Task CreateDefaultTeamRolesAsync(Guid teamId, Dictionary<int, Guid> teamRoleIds)
    {
        var hasRoles = await _dbTeamRoles.AnyAsync(x => x.TeamId == teamId);
        if (hasRoles)
        {
            throw new AirSoftBaseException(ErrorCodes.TeamRolesRepository.AlreadyHasRoles, "У команды уже есть роли");
        }

        var roles = Enum.GetValues<DefaultMemberRoleType>().Select(v => new DbTeamRole
        {
            Id = teamRoleIds[(int)v],
            Title = v.ToString(),
            CreatedDate = new DateTime(2021, 12, 02, 1, 50, 00),
            ModifiedDate = new DateTime(2021, 12, 02, 1, 50, 00),
            Rank = (int)v,
            TeamId = teamId
        })
            .ToArray();
        foreach (var role in roles)
        {
            _dbTeamRoles.Add(role);
        }
    }
}