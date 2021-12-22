namespace Store.Service.Contracts.Models;

public class ReferenceData<T>
{
    public ReferenceData(T id, string title, int? grade = null)
    {
        Id = id;
        Title = title;
        Grade = grade;
    }

    public T Id { get; }

    public string Title { get; }

    public int? Grade { get; }
}