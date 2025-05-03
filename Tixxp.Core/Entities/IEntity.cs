namespace Tixxp.Core.Entities;

public interface IEntity
{
    public long Id { get; set; }
    public DateTime Created_Date { get; set; }
    public DateTime? Updated_Date { get; set; }
    public bool IsDeleted { get; set; }
}
