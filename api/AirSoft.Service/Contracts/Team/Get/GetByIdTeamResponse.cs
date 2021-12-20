using AirSoft.Service.Contracts.Member;

namespace AirSoft.Service.Contracts.Team.Get;

public class GetByIdTeamResponse
{
    public GetByIdTeamResponse(MemberData memberData)
    {
        MemberData = memberData;
    }
    
    public MemberData MemberData { get; }
}