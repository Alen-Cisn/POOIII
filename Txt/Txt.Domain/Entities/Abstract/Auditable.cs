using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Txt.Domain.Entities.Abstract;

public abstract class Auditable : IAuditable
{
    [Required]
    [ForeignKey("CreatedBy")]
    public string CreatedById { get; set; }
    [Required]
    public User CreatedBy { get; set; }
    [Required]
    public DateTime CreatedOn { get; set; }

    [ForeignKey("ModifiedBy")]
    public string? ModifiedById { get; set; }
    public User? ModifiedBy { get; set; }
    [Required]
    public DateTime? ModifiedOn { get; set; }

}