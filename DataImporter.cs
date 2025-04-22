using DataImporter.DbModels;

namespace DataImporter;

public class DataImporter
{
    private readonly StarkovDbContext _context;

    public DataImporter(StarkovDbContext context)
    {
        _context = context;
    }

    public void ImportData(string filePath, string importType)
    {
        var lines = File.ReadAllLines(filePath);

        // i от 1, т.к. пропускаем названия столбцов
        for(var i = 1; i < lines.Length; i++)  
        {
            var line = lines[i];
            try
            {
                var cleanedLine = CleanData(line);
                var data = ParseLine(cleanedLine, importType);

                switch (importType)
                {
                    case "department":
                        ImportDepartment(data);
                        break;
                    case "employee":
                        ImportEmployee(data);
                        break;
                    case "position":
                        ImportPosition(data);
                        break;
                    default:
                        throw new ArgumentException("Invalid import type.");
                }
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                {
                    Console.Error.WriteLine($"{ex.Message}");
                    Environment.Exit(1);
                }
                Console.Error.WriteLine($"Error processing line '{line}': {ex.Message}");
                Environment.Exit(1);
            }
        }
    }

    private string CleanData(string line)
    {
        return line.Trim();
    }

    private object? ParseLine(string line, string importType)
    {
        var data = line.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

        switch (importType)
        {
            case "department":
                return data.Length == 4 || data.Length == 3 ? data : null;
            case "employee":
                return data.Length == 5 ? data : null;
            case "position":
                return data.Length == 1 ? data[0] : null;
            default:
                throw new ArgumentException("Invalid import type.");
        }
    }

    private void ImportDepartment(object data)
    {
        var departmentData = (string[])data;
        var name = departmentData[0]?.Trim();
        var parentDepartmentName = departmentData.Length == 4 ? departmentData[1]?.Trim() : null;
        var managerName = departmentData.Length == 4 ? departmentData[2]?.Trim() : departmentData[1].Trim();
        var phone = departmentData.Length == 4 ? departmentData[3]?.Trim() : departmentData[2]?.Trim();

        var parentDepartment = _context.Departments
            .FirstOrDefault(d => d.Name == parentDepartmentName);

        var manager = _context.Employees.Any() ? _context.Employees
            .FirstOrDefault(e => e != null && e.FullName == managerName) : null;

        var newDepartment = new Department
        {
            Name = name,
            ParentId = parentDepartment?.Id,
            ManagerId = manager?.Id,
            Phone = phone
        };

        _context.Departments.Add(newDepartment);
        _context.SaveChanges();
    }

    private void ImportEmployee(object data)
    {
        var employeeData = (string[])data;
        if (employeeData != null)
        {
            var departmentName = employeeData[0]?.Trim();
            var fullName = employeeData[1]?.Trim();
            var login = employeeData[2]?.Trim();
            var password = employeeData[3]?.Trim();
            var jobTitleName = employeeData[4]?.Trim();

            var department = _context.Departments
                .FirstOrDefault(d => d.Name == departmentName);

            var jobTitle = _context.JobTitles
                .FirstOrDefault(j => j.Name == jobTitleName);

            var newEmployee = new Employee
            {
                DepartmentId = department?.Id,
                FullName = fullName,
                Login = login,
                Password = password,
                JobTitleId = jobTitle?.Id
            };

            _context.Employees.Add(newEmployee);
            _context.SaveChanges();
        }
    }

    private void ImportPosition(object data)
    {
        var jobTitle = (string)data;

        if (_context.JobTitles.Any(j => j.Name == jobTitle))
        {
            return;
        }

        var newJobTitle = new JobTitle { Name = jobTitle };
        _context.JobTitles.Add(newJobTitle);
        _context.SaveChanges();
    }
}
