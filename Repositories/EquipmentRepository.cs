using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Data;
using ProjectManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementApp.Repositories
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public EquipmentRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<Equipment>> GetAllEquipmentAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Equipment
                .Include(e => e.Project)
                .ToListAsync();
        }

        public async Task<Equipment> GetEquipmentByIdAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Equipment
                .Include(e => e.Project)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Equipment> AddEquipmentAsync(Equipment equipment)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            context.Equipment.Add(equipment);
            await context.SaveChangesAsync();
            return equipment;
        }

        public async Task UpdateEquipmentAsync(Equipment equipment)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            context.Entry(equipment).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteEquipmentAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var equipment = await context.Equipment.FindAsync(id);
            if (equipment != null)
            {
                context.Equipment.Remove(equipment);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Equipment>> GetEquipmentByStatusAsync(string status)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Equipment
                .Include(e => e.Project)
                .Where(e => e.Status == status)
                .ToListAsync();
        }

        public async Task<bool> UpdateEquipmentStatusAsync(int id, string newStatus, int? projectId = null)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var equipment = await context.Equipment.FindAsync(id);
            if (equipment == null) return false;

            equipment.Status = newStatus;
            equipment.ProjectId = projectId;
            if (newStatus != "Na Robocie")
            {
                equipment.ProjectId = null;
            }
            await context.SaveChangesAsync();
            return true;
        }
    }
}