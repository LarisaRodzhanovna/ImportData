using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace DataImporter;

class Program
{
    static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<StarkovDbContext>();

            Console.WriteLine("Выберите режим работы: 1 - импорт, 2 - печать состояния БД");
            string jobType = Console.ReadLine();
            if (jobType == "1")
            {
                Console.WriteLine("Введите полный путь к файлу импорта: ");
                string filePath = Console.ReadLine();

                if (File.Exists(filePath))
                {
                    Console.WriteLine("Введите тип импорта (position, department, employee): ");
                    string importType = Console.ReadLine();

                    var dataImporter = new DataImporter(context);
                    dataImporter.ImportData(filePath, importType);
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Ошибка: Файл не найден. Пожалуйста, проверьте путь и попробуйте снова.");
                    Environment.Exit(1);
                }
            }
            else if (jobType == "2") 
            {
                var dataViewer = new DataViewer(context);
                dataViewer.ShowStructure();
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Ошибка: Файл не найден. Пожалуйста, проверьте путь и попробуйте снова.");
                Environment.Exit(1);
            }
        }

        host.Run();

    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                var configuration = hostContext.Configuration;

                services.AddDbContext<StarkovDbContext>(options =>
                   options.UseNpgsql(
                       configuration.GetConnectionString("DefaultConnection")                       
                   )
               );
            });
}