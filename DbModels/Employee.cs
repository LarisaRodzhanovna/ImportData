using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataImporter.DbModels;

[Table("employees")]
public class Employee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int? DepartmentId { get; set; }

    [Required]
    [StringLength(255)]
    [Column("full_name")]
    public string FullName { get; set; }

    [Required]
    [StringLength(255)]
    [Column("login")]
    public string Login { get; set; }

    [StringLength(255)]
    [Column("password")]
    public string Password { get; set; }

    public int? JobTitleId { get; set; }
    
    public Department? Department { get; set; }

    public JobTitle? JobTitle { get; set; }
}
