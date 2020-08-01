using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestAppAddressBook.Models;

namespace TestAppAddressBook.Services
{
    public interface IContactsService
    {
        Task<List<Contact>> GetContactsAsync();
        Task<Contact> GetContactByIdAsync(int? id);
        Task<int> CreateContactAsync(Contact contact);
        Task<int> DeleteContactAsync(int? id);
        Task<int> UpdateContactAsync(Contact contact, int? id);
    }
}
