using System;
using System.Threading.Tasks;

namespace ProjectManagementApp.Repositories
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
}