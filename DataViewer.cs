using DataImporter.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DataImporter;

public class DataViewer
{
    private readonly StarkovDbContext _context;

    public DataViewer(StarkovDbContext context)
    {
        _context = context;
    }

    public void ShowStructure(int? departmentId = null)
    {
        // Получаем все подразделения и сотрудников с учетом иерархии
        var departments = _context.Departments
            .Include(d => d.Children)
            .Include(d => d.Employees)
                .ThenInclude(e => e.JobTitle)
            .ToList();

        // Если departmentId задан, то находим это конкретное подразделение
        if (departmentId.HasValue)
        {
            var targetDepartment = departments.FirstOrDefault(d => d.Id == departmentId.Value);
            if (targetDepartment != null)
            {
                PrintDepartment(targetDepartment, 0);
            }
        }
        else
        {
            // Если departmentId не задан, выводим все подразделения
            var rootDepartments = departments.Where(d => d.ParentId == null).ToList();
            foreach (var department in rootDepartments)
            {
                PrintDepartment(department, 0);
            }
        }
    }

    private void PrintDepartment(Department department, int level)
    {
        // Выводим информацию о подразделении с префиксом "="
        Console.WriteLine($"{new string('=', level + 1)} {department.Name} ID={department.Id}");

        // Выводим информацию о руководителе
        if (department.ManagerId.HasValue && department.Manager != null)
        {
            Console.WriteLine($"{new string(' ', level)}* Сотрудник ID={department.ManagerId} (должность ID={department.Manager.JobTitleId})");
        }

        // Выводим сотрудников подразделения
        foreach (var employee in department?.Employees.Where(e => e.JobTitleId.HasValue && e.DepartmentId == department.Id))
        {
            Console.WriteLine($"{new string(' ', level + 1)}- Сотрудник ID={employee.Id} (должность ID={employee.JobTitleId})");
        }

        // Рекурсивный вызов для всех дочерних подразделений
        var sortedChildren = department?.Children.OrderBy(d => d.Name).ToList();
        foreach (var child in sortedChildren)
        {
            PrintDepartment(child, level + 1);
        }
    }
}
