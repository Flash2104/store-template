namespace StoreApi.Models.Store.Get;

public class GetStoreResponseDto
{
    public GetStoreResponseDto(string title, byte[] logo)
    {
        Title = title;
        Logo = logo;
    }

    public string Title { get; }

    public byte[] Logo { get; }
}