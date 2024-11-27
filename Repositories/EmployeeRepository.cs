using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Data;
using ProjectManagementApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace ProjectManagementApp.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public EmployeeRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Employees
                .Include(e => e.Project)
                .ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Employees
                .Include(e => e.Project)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            context.Employees.Add(employee);
            await context.SaveChangesAsync();
            return employee;
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            context.Entry(employee).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var employee = await context.Employees.FindAsync(id);
            if (employee != null)
            {
                context.Employees.Remove(employee);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Employee>> GetEmployeesByStatusAsync(string status)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Employees
                .Include(e => e.Project)
                .Where(e => e.Status == status)
                .ToListAsync();
        }

        public async Task<bool> UpdateEmployeeStatusAsync(int id, string newStatus, int? projectId = null)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var employee = await context.Employees.FindAsync(id);
            if (employee == null) return false;

            employee.Status = newStatus;
            employee.ProjectId = projectId;
            if (newStatus != "Na Robocie")
            {
                employee.ProjectId = null;
            }
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetEmployeeVacationAsync(int id, DateTime startDate, DateTime endDate)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var employee = await context.Employees.FindAsync(id);
            if (employee == null) return false;

            employee.Status = "Urlop";
            employee.VacationStart = startDate;
            employee.VacationEnd = endDate;
            employee.ProjectId = null;
            await context.SaveChangesAsync();
            return true;
        }
    }
}