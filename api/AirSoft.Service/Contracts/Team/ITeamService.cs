using Store.Service.Contracts.Team.Create;
using Store.Service.Contracts.Team.Delete;
using Store.Service.Contracts.Team.Get;
using Store.Service.Contracts.Team.GetCurrent;
using Store.Service.Contracts.Team.UpdateMainInfo;

namespace Store.Service.Contracts.Team;

public interface ITeamService
{
    Task<GetCurrentTeamResponse> GetCurrent();

    Task<GetByIdTeamResponse> Get(GetByIdTeamRequest request);

    Task<CreateTeamResponse> Create(CreateTeamRequest request);

    Task<UpdateTeamMainInfoResponse> UpdateMainInfo(UpdateTeamMainInfoRequest request);

    Task Delete(DeleteTeamRequest request);
}