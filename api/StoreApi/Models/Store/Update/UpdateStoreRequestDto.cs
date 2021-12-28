namespace StoreApi.Models.Store.Update;

public class UpdateStoreRequestDto
{
    public UpdateStoreRequestDto(string title, byte[] logo)
    {
        Title = title;
        Logo = logo;
    }

    public string Title { get; }

    public byte[] Logo { get; }
}