using ContactsApp.Models;

namespace ContactsApp.Contracts
{
    public interface IContactRepository
    {
        Task<IEnumerable<Contact>> GetContacts();
        Task<Contact> GetContact(int contactId);
        Task<IEnumerable<EditHistory>> GetEditHistory(int contactId);

        Task<Contact> AddContact(Contact contact);
        Task<int> UpdateContact(Contact contact);
        Task<int> DeleteContact(int employeeId);
    }
}
