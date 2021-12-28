namespace Store.Service.Contracts.Store.Update;

public class UpdateStoreRequest
{
    public UpdateStoreRequest(string title, byte[] logo)
    {
        Title = title;
        Logo = logo;
    }

    public string Title { get; }

    public byte[] Logo { get; }
}