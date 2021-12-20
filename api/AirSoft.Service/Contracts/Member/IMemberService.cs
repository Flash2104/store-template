using AirSoft.Service.Contracts.Member.Create;
using AirSoft.Service.Contracts.Member.Delete;
using AirSoft.Service.Contracts.Member.Get;
using AirSoft.Service.Contracts.Member.GetByUserId;
using AirSoft.Service.Contracts.Member.GetCurrent;
using AirSoft.Service.Contracts.Member.Update;

namespace AirSoft.Service.Contracts.Member;

public interface IMemberService
{
    Task<GetCurrentMemberResponse> GetCurrent();

    Task<GetByIdMemberResponse> Get(GetByIdMemberRequest request);

    Task<GetByUserIdMemberResponse> GetByUserId(Guid userId);

    Task<CreateMemberResponse> Create(CreateMemberRequest request);

    Task<UpdateMemberResponse> Update(UpdateMemberRequest request);

    Task Delete(DeleteMemberRequest request);
}