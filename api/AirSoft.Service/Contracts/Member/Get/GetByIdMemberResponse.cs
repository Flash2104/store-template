using AirSoft.Service.Contracts.Models;

namespace AirSoft.Service.Contracts.Member.Get;

public class GetByIdMemberResponse
{
    public GetByIdMemberResponse(MemberData? memberData)
    {
        MemberData = memberData;
    }
    
    public MemberData? MemberData { get; }
}