using DataImporter.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DataImporter;

public class StarkovDbContext : DbContext
{
    public StarkovDbContext() 
    { 
    }
    public StarkovDbContext(DbContextOptions<StarkovDbContext> options)
            : base(options)
    {
    }

    public DbSet<JobTitle> JobTitles { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JobTitle>(entity =>
        {
            entity.HasKey(e => e.Id)
                  .HasName("pk_job_titles");

            entity.ToTable("job_titles", tb => tb.HasComment("Названия должностей"));

            entity.Property(e => e.Id)
                  .ValueGeneratedOnAdd()
                  .HasColumnName("id")
                  .HasComment("Идентификатор должности");

            entity.Property(e => e.Name)
                  .IsRequired()
                  .HasColumnType("varchar(255)")
                  .HasComment("Название должности")
                  .HasColumnName("name");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id)
                  .HasName("pk_departments");

            entity.ToTable("departments", tb => tb.HasComment("Подразделение"));

            entity.Property(e => e.Id)
                  .ValueGeneratedOnAdd()
                  .HasColumnName("id")
                  .HasComment("Идентификатор подразделения");

            entity.Property(e => e.Name)
                  .IsRequired()
                  .HasColumnType("varchar(255)")
                  .HasComment("Название подразделения")
                  .HasColumnName("name");

            entity.Property(e => e.ParentId)
                  .HasColumnName("parent_id")
                  .HasComment("Идентификатор вышестоящего подразделения");

            entity.Property(e => e.ManagerId)
                  .HasColumnName("manager_id")
                  .HasComment("Идентификатор руководителя подразделения");

            entity.Property(e => e.Phone)
                  .IsRequired()
                  .HasColumnType("varchar(255)")
                  .HasComment("Телефон подразделения")
                  .HasColumnName("phone");

            entity.HasOne(d => d.Parent)
                   .WithMany(d => d.Children)
                   .HasForeignKey(d => d.ParentId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("fk_parent_id");

            entity.HasOne(d => d.Manager)
                .WithMany()
                .HasForeignKey(d => d.ManagerId);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id)
                  .HasName("pk_employees");

            entity.ToTable("employees", tb => tb.HasComment("Сотрудники"));

            entity.Property(e => e.Id)
                  .ValueGeneratedOnAdd()
                  .HasColumnName("id")
                  .HasComment("Идентификатор сотрудника");

            entity.Property(e => e.DepartmentId)
                  .HasColumnName("department_id")
                  .HasComment("Идентификатор подразделения сотрудника");

            entity.Property(e => e.FullName)
                  .IsRequired()
                  .HasColumnType("varchar(255)")
                  .HasComment("Фамилия Имя Отчество")
                  .HasColumnName("full_name");

            entity.Property(e => e.Login)
                  .IsRequired()
                  .HasColumnType("varchar(255)")
                  .HasComment("Логин")
                  .HasColumnName("login");

            entity.Property(e => e.Password)
                  .HasColumnType("varchar(255)")
                  .HasComment("Пароль (хэш)")
                  .HasColumnName("password");

            entity.Property(e => e.JobTitleId)
                  .HasColumnName("job_title_id")
                  .HasComment("Идентификатор должности сотрудника");

            entity.HasOne(e => e.JobTitle)
                  .WithMany(j => j.Employees)
                  .HasForeignKey(e => e.JobTitleId);

            entity.HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId);
                });
    }
}
