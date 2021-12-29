namespace Store.Service.Contracts.Category.GetTree;

public class GetCategoryTreeRequest
{
    public int Id { get; }

    public GetCategoryTreeRequest(int id)
    {
        Id = id;
    }
}