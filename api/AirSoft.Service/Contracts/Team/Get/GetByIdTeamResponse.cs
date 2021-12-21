using Store.Service.Contracts.Member;

namespace Store.Service.Contracts.Team.Get;

public class GetByIdTeamResponse
{
    public GetByIdTeamResponse(MemberData memberData)
    {
        MemberData = memberData;
    }
    
    public MemberData MemberData { get; }
}