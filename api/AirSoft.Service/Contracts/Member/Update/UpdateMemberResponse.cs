using AirSoft.Service.Contracts.Models;

namespace AirSoft.Service.Contracts.Member.Update;

public class UpdateMemberResponse
{
    public UpdateMemberResponse(MemberData? memberData)
    {
        MemberData = memberData;
    }
    
    public MemberData? MemberData { get; }
}