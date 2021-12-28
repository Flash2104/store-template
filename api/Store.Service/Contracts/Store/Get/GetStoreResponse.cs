namespace Store.Service.Contracts.Store.Get;

public class GetStoreResponse
{
    public GetStoreResponse(string title, byte[] logo)
    {
        Title = title;
        Logo = logo;
    }

    public string Title { get; }

    public byte[] Logo { get; }
}