using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Data;
using ProjectManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Include(e => e.Project)
                .ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Project)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Employee>> GetEmployeesByStatusAsync(string status)
        {
            return await _context.Employees
                .Include(e => e.Project)
                .Where(e => e.Status == status)
                .ToListAsync();
        }

        public async Task<bool> UpdateEmployeeStatusAsync(int id, string newStatus, int? projectId = null)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return false;

            employee.Status = newStatus;
            employee.ProjectId = projectId;

            if (newStatus != "Na Robocie")
            {
                employee.ProjectId = null;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetEmployeeVacationAsync(int id, DateTime startDate, DateTime endDate)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return false;

            employee.Status = "Urlop";
            employee.VacationStart = startDate;
            employee.VacationEnd = endDate;
            employee.ProjectId = null;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
