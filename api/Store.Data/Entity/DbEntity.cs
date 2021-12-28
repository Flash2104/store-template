
namespace Store.Data.Entity;

public interface IDbEntity
{
    public int CreatedBy { get; set; }

    public int ModifiedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? Deleted { get; set; }
}

public interface IDbEntity<T>: IDbEntity
{
    public T? Id { get; set; }
}

public abstract class DbEntity : IDbEntity
{
    public int CreatedBy { get; set; }

    public int ModifiedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? Deleted { get; set; }
}

public abstract class DbEntity<T> : DbEntity, IDbEntity<T>
{
    public T? Id { get; set; }
}

