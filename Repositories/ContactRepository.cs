using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagementApp.Data;
using ProjectManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private List<Contact> _cache;

        public ContactRepository(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _cache = new List<Contact>();
        }

        public async Task<List<Contact>> GetAllContactsAsync()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (!_cache.Any())
                {
                    _cache = await context.Contacts.ToListAsync();
                }
                return _cache;
            }
        }

        public async Task<Contact> AddContactAsync(Contact contact)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var exists = await context.Contacts.AnyAsync(c => c.Email.ToLower() == contact.Email.ToLower());
                if (exists)
                {
                    throw new InvalidOperationException("Ten adres email już istnieje.");
                }

                context.Contacts.Add(contact);
                await context.SaveChangesAsync();
                _cache.Add(contact);
                return contact;
            }
        }

        public async Task UpdateContactAsync(Contact contact)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Entry(contact).State = EntityState.Modified;
                await context.SaveChangesAsync();

                var cachedContact = _cache.FirstOrDefault(c => c.Id == contact.Id);
                if (cachedContact != null)
                {
                    cachedContact.Email = contact.Email;
                }
            }
        }

        public async Task DeleteContactAsync(int id)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var contact = await context.Contacts.FindAsync(id);
                if (contact != null)
                {
                    context.Contacts.Remove(contact);
                    await context.SaveChangesAsync();
                    _cache.RemoveAll(c => c.Id == id);
                }
            }
        }
    }
}
