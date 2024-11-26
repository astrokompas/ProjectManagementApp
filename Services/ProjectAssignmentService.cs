using System;
using System.Threading.Tasks;
using ProjectManagementApp.Models;
using ProjectManagementApp.Repositories;

namespace ProjectManagementApp.Services
{
    public interface IProjectAssignmentService
    {
        Task<bool> AssignEmployeeToProject(int employeeId, int projectId);
        Task<bool> RemoveEmployeeFromProject(int employeeId);
        Task<bool> UpdateEmployeeStatus(int employeeId, string newStatus, int? projectId = null, DateTime? vacationStart = null, DateTime? vacationEnd = null);
    }

    public class ProjectAssignmentService : IProjectAssignmentService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IProjectRepository _projectRepository;

        public ProjectAssignmentService(
            IEmployeeRepository employeeRepository,
            IProjectRepository projectRepository)
        {
            _employeeRepository = employeeRepository;
            _projectRepository = projectRepository;
        }

        public async Task<bool> AssignEmployeeToProject(int employeeId, int projectId)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
            var project = await _projectRepository.GetProjectByIdAsync(projectId);

            if (employee == null || project == null)
                return false;

            // Update employee status and project assignment
            employee.Status = "Na Robocie";
            employee.ProjectId = projectId;
            employee.VacationStart = null;
            employee.VacationEnd = null;

            await _employeeRepository.UpdateEmployeeAsync(employee);
            return true;
        }

        public async Task<bool> RemoveEmployeeFromProject(int employeeId)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
            if (employee == null)
                return false;

            // Reset employee status and project assignment
            employee.Status = "Baza";
            employee.ProjectId = null;

            await _employeeRepository.UpdateEmployeeAsync(employee);
            return true;
        }

        public async Task<bool> UpdateEmployeeStatus(
            int employeeId,
            string newStatus,
            int? projectId = null,
            DateTime? vacationStart = null,
            DateTime? vacationEnd = null)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
            if (employee == null)
                return false;

            employee.Status = newStatus;

            switch (newStatus)
            {
                case "Na Robocie":
                    if (projectId.HasValue)
                    {
                        employee.ProjectId = projectId;
                        employee.VacationStart = null;
                        employee.VacationEnd = null;
                    }
                    break;

                case "Urlop":
                    employee.ProjectId = null;
                    employee.VacationStart = vacationStart;
                    employee.VacationEnd = vacationEnd;
                    break;

                case "Baza":
                    employee.ProjectId = null;
                    employee.VacationStart = null;
                    employee.VacationEnd = null;
                    break;
            }

            await _employeeRepository.UpdateEmployeeAsync(employee);
            return true;
        }
    }
}