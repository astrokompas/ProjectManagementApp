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

        Task<bool> AssignEquipmentToProject(int equipmentId, int projectId);
        Task<bool> RemoveEquipmentFromProject(int equipmentId);
        Task<bool> UpdateEquipmentStatus(int equipmentId, string newStatus, int? projectId = null);
    }

    public class ProjectAssignmentService : IProjectAssignmentService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IProjectRepository _projectRepository;

        public ProjectAssignmentService(
            IEmployeeRepository employeeRepository,
            IEquipmentRepository equipmentRepository,
            IProjectRepository projectRepository)
        {
            _employeeRepository = employeeRepository;
            _equipmentRepository = equipmentRepository;
            _projectRepository = projectRepository;
        }

        public async Task<bool> AssignEmployeeToProject(int employeeId, int projectId)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
            var project = await _projectRepository.GetProjectByIdAsync(projectId);
            if (employee == null || project == null)
                return false;

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

        public async Task<bool> AssignEquipmentToProject(int equipmentId, int projectId)
        {
            var equipment = await _equipmentRepository.GetEquipmentByIdAsync(equipmentId);
            var project = await _projectRepository.GetProjectByIdAsync(projectId);
            if (equipment == null || project == null)
                return false;

            equipment.Status = "Na Robocie";
            equipment.ProjectId = projectId;
            await _equipmentRepository.UpdateEquipmentAsync(equipment);
            return true;
        }

        public async Task<bool> RemoveEquipmentFromProject(int equipmentId)
        {
            var equipment = await _equipmentRepository.GetEquipmentByIdAsync(equipmentId);
            if (equipment == null)
                return false;

            equipment.Status = "Baza";
            equipment.ProjectId = null;
            await _equipmentRepository.UpdateEquipmentAsync(equipment);
            return true;
        }

        public async Task<bool> UpdateEquipmentStatus(
            int equipmentId,
            string newStatus,
            int? projectId = null)
        {
            var equipment = await _equipmentRepository.GetEquipmentByIdAsync(equipmentId);
            if (equipment == null)
                return false;

            equipment.Status = newStatus;
            switch (newStatus)
            {
                case "Na Robocie":
                    if (projectId.HasValue)
                    {
                        equipment.ProjectId = projectId;
                    }
                    break;
                case "Serwis":
                case "Baza":
                    equipment.ProjectId = null;
                    break;
            }
            await _equipmentRepository.UpdateEquipmentAsync(equipment);
            return true;
        }
    }
}