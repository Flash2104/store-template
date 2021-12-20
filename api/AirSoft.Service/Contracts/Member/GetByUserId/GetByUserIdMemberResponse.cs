using AirSoft.Service.Contracts.Models;

namespace AirSoft.Service.Contracts.Member.GetByUserId;

public class GetByUserIdMemberResponse
{
    public GetByUserIdMemberResponse(MemberData? memberData)
    {
        MemberData = memberData;
    }
    
    public MemberData? MemberData { get; }
}