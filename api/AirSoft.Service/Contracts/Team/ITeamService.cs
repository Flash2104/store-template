using AirSoft.Service.Contracts.Team.Create;
using AirSoft.Service.Contracts.Team.Delete;
using AirSoft.Service.Contracts.Team.Get;
using AirSoft.Service.Contracts.Team.GetCurrent;
using AirSoft.Service.Contracts.Team.UpdateMainInfo;

namespace AirSoft.Service.Contracts.Team;

public interface ITeamService
{
    Task<GetCurrentTeamResponse> GetCurrent();

    Task<GetByIdTeamResponse> Get(GetByIdTeamRequest request);

    Task<CreateTeamResponse> Create(CreateTeamRequest request);

    Task<UpdateTeamMainInfoResponse> UpdateMainInfo(UpdateTeamMainInfoRequest request);

    Task Delete(DeleteTeamRequest request);
}