
namespace AirSoftApi.Models.Member.Get;

public class GeMemberResponseDto
{
    public GeMemberResponseDto(MemberDataDto memberData)
    {
        MemberData = memberData;
    }

    public MemberDataDto MemberData { get; }
}