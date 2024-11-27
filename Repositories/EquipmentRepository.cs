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
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly ApplicationDbContext _context;

        public EquipmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Equipment>> GetAllEquipmentAsync()
        {
            return await _context.Equipment
                .Include(e => e.Project)
                .ToListAsync();
        }

        public async Task<Equipment> GetEquipmentByIdAsync(int id)
        {
            return await _context.Equipment
                .Include(e => e.Project)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Equipment> AddEquipmentAsync(Equipment equipment)
        {
            _context.Equipment.Add(equipment);
            await _context.SaveChangesAsync();
            return equipment;
        }

        public async Task UpdateEquipmentAsync(Equipment equipment)
        {
            _context.Entry(equipment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEquipmentAsync(int id)
        {
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment != null)
            {
                _context.Equipment.Remove(equipment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Equipment>> GetEquipmentByStatusAsync(string status)
        {
            return await _context.Equipment
                .Include(e => e.Project)
                .Where(e => e.Status == status)
                .ToListAsync();
        }

        public async Task<bool> UpdateEquipmentStatusAsync(int id, string newStatus, int? projectId = null)
        {
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null) return false;

            equipment.Status = newStatus;
            equipment.ProjectId = projectId;

            if (newStatus != "Na Robocie")
            {
                equipment.ProjectId = null;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
