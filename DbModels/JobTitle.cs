using DataImporter.DbModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataImporter.DbModels;

[Table("job_titles")]
public class JobTitle
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    [Column("name")]
    public string Name { get; set; }

    // Навигационное свойство для связи с сотрудниками
    public ICollection<Employee>? Employees { get; set; } = [];
}
