using AirSoft.Service.Contracts.Models;

namespace AirSoftApi.Models.Member.Update;

public class UpdateMemberResponseDto
{
    public UpdateMemberResponseDto(MemberDataDto memberData)
    {
        MemberData = memberData;
    }
    
    public MemberDataDto MemberData { get; }
}