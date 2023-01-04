using ContactsApp.Contracts;
using ContactsApp.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ContactsApp.Services
{
    public class ContactRepository : IContactRepository
    {
        private readonly ContactsAppContext _context;
        private readonly ILogger<ContactRepository> _logger;

        public ContactRepository(ContactsAppContext context, ILogger<ContactRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Contact>> GetContacts()
        {
            try
            {
                return await _context.Contact.ToListAsync();

            }
            catch(Exception ex ) 
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<Contact> GetContact(int contactId)
        {
            try
            {
                return await _context.Contact
               .FirstOrDefaultAsync(e => e.Id == contactId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<Contact> AddContact(Contact contact)
        {
            try
            {
                var result = await _context.Contact.AddAsync(contact);
                await _context.SaveChangesAsync();
                return result.Entity;
            }

            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<int> UpdateContact(Contact contact)
        {
            try
            {
                _context.Entry(contact).State = EntityState.Modified;

                EditHistory editHistory = new EditHistory();
                editHistory.ContactId = contact.Id;
                editHistory.ModifiedDate = DateTime.Now;
                await _context.EditHistories.AddAsync(editHistory);

                await _context.SaveChangesAsync();

                return contact.Id;
            }
          
             catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        public async Task<int> DeleteContact(int contactId)
        {
            try
            {
                var result = _context.Contact
                .Find(contactId);
                if (result != null)
                {
                    _context.Contact.Remove(result);
                    await _context.SaveChangesAsync();
                    return contactId;
                }
                return 0;
            }
             catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        public async Task<IEnumerable<EditHistory>> GetEditHistory(int contactId)
        {
            try
            {
                return await _context.EditHistories.Where(e => e.ContactId == contactId).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }
    }
}

    

