
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirSoft.Data.Entity;

public interface IDbEntity
{
    public Guid CreatedBy { get; set; }

    public Guid ModifiedBy { get; set; }

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
    public Guid CreatedBy { get; set; }

    public Guid ModifiedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? Deleted { get; set; }
}

public abstract class DbEntity<T> : DbEntity, IDbEntity<T>
{
    public T? Id { get; set; }
}

