namespace AirSoftApi.Models.Member.GetCurrent;

public class GetCurrentMemberResponseDto
{
    public GetCurrentMemberResponseDto(MemberDataDto memberData)
    {
        MemberData = memberData;
    }

    public MemberDataDto MemberData { get; }
}