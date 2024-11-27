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
    public class ProjectRepository : IProjectRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public ProjectRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Projects
                .Include(p => p.AssignedEmployees)
                .Include(p => p.AssignedEquipment)
                .ToListAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Projects
                .Include(p => p.AssignedEmployees)
                .Include(p => p.AssignedEquipment)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Project> AddProjectAsync(Project project)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            context.Projects.Add(project);
            await context.SaveChangesAsync();
            return project;
        }

        public async Task UpdateProjectAsync(Project project)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            context.Entry(project).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var project = await context.Projects.FindAsync(id);
            if (project != null)
            {
                context.Projects.Remove(project);
                await context.SaveChangesAsync();
            }
        }
    }
}
