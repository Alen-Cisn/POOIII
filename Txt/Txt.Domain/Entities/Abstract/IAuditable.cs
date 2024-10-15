
namespace Txt.Domain.Entities.Abstract;

public interface IAuditable
{
    public string CreatedById { get; set; }
    public User CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }

}