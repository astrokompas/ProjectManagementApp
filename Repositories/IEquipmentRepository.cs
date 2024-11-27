using ProjectManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.Repositories
{
    public interface IEquipmentRepository
    {
        Task<List<Equipment>> GetAllEquipmentAsync();
        Task<Equipment> GetEquipmentByIdAsync(int id);
        Task<Equipment> AddEquipmentAsync(Equipment equipment);
        Task UpdateEquipmentAsync(Equipment equipment);
        Task DeleteEquipmentAsync(int id);
        Task<List<Equipment>> GetEquipmentByStatusAsync(string status);
        Task<bool> UpdateEquipmentStatusAsync(int id, string newStatus, int? projectId = null);
    }
}