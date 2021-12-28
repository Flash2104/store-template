namespace StoreApi.Models.Store.Update;

public class UpdateStoreResponseDto
{
    public UpdateStoreResponseDto(string title, byte[] logo)
    {
        Title = title;
        Logo = logo;
    }

    public string Title { get; }

    public byte[] Logo { get; }
}