namespace Store.Service.Contracts.Store.Update;

public class UpdateStoreResponse
{
    public UpdateStoreResponse(string title, byte[] logo)
    {
        Title = title;
        Logo = logo;
    }

    public string Title { get; }

    public byte[] Logo { get; }
}