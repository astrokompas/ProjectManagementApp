using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagementApp.Data;
using ProjectManagementApp.Repositories;
using ProjectManagementApp.Services;
using ProjectManagementApp.ViewModels;
using System.IO;
using System.Windows;

namespace ProjectManagementApp
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;
        private IConfiguration _configuration;

        public static App Current => (App)Application.Current;

        public ServiceProvider ServiceProvider => _serviceProvider;

        public App()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();

            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Transient);

            services.AddSingleton<IContactRepository, ContactRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();

            services.AddScoped<IProjectAssignmentService, ProjectAssignmentService>();

            services.AddTransient<ContactsViewModel>();
            services.AddTransient<EmployeesViewModel>();

            services.AddTransient<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
            }

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
