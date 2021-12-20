using AirSoft.Service.Contracts.Models;

namespace AirSoft.Service.Contracts.Member.GetCurrent;

public class GetCurrentMemberResponse
{
    public GetCurrentMemberResponse(MemberData? memberData)
    {
        MemberData = memberData;
    }
    
    public MemberData? MemberData { get; }
}