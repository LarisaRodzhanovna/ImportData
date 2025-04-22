using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataImporter.DbModels;

[Table("departments")]
public class Department
{
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    [Column("name")]
    public string Name { get; set; }

    public int? ParentId { get; set; }
    public int? ManagerId { get; set; }

    [Required]
    [StringLength(255)]
    [Column("phone")]
    public string Phone { get; set; }
    
    public virtual Department? Parent { get; set; } 

    public virtual ICollection<Department>? Children { get; set; }
    public Employee? Manager { get; set; }

    [NotMapped]
    public ICollection<Employee>? Employees { get; set; } = [];
}
