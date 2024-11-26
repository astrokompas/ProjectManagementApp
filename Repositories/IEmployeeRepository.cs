using ProjectManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<Employee> AddEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(int id);
        Task<List<Employee>> GetEmployeesByStatusAsync(string status);
        Task<bool> UpdateEmployeeStatusAsync(int id, string newStatus, int? projectId = null);
        Task<bool> SetEmployeeVacationAsync(int id, DateTime startDate, DateTime endDate);
    }
}
