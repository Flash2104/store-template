using AirSoft.Service.Contracts.Models;

namespace AirSoft.Service.Contracts.Member.Create;

public class CreateMemberResponse
{
    public CreateMemberResponse(MemberData? memberData)
    {
        MemberData = memberData;
    }
    
    public MemberData? MemberData { get; }
}