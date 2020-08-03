using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TestAppAddressBook.Models;
using System.Linq;
using System.Transactions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace TestAppAddressBook.Services
{
    public class ContactsService : IContactsService
    {
        private readonly string connection;
        private readonly ILogger logger;
        private readonly IConfiguration configuration;
       
        public ContactsService(IConfiguration _configuration, ILogger<ContactsService> _logger) 
        {
            logger = _logger;
            configuration = _configuration;
            connection = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<List<Contact>> GetContactsAsync()
        {
            var contactDict = new Dictionary<int, Contact>();
            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                using (IDbConnection con = new SqlConnection(connection))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    var contacts = await con.QueryAsync<Contact, Phone, Contact>("sp_GetContacts", (contact, phone) =>
                    {
                        Contact entry;
                        if (!contactDict.TryGetValue(contact.ContactId, out entry))
                        {
                            entry = contact;
                            entry.Phones = new List<Phone>();
                            contactDict.Add(entry.ContactId, entry);
                        }

                        entry.Phones.Add(phone);
                        return entry;
                    }, splitOn: "ContactId", commandType: CommandType.StoredProcedure);
                    tran.Complete();
                    return contacts.Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in ContactsService/GetContactsAsync action: {ex.Message}");
                throw;
            }
        }

        public async Task <Contact> GetContactByIdAsync(int? id)
        {
            var contactDict = new Dictionary<int, Contact>();
            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                using (IDbConnection con = new SqlConnection(connection))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    DynamicParameters parameter = new DynamicParameters();
                    parameter.Add("@ContactId", id);
                    var query = await con.QueryAsync<Contact, Phone, Contact> ("sp_GetContactById", (contact, phone) => 
                    {
                        Contact entry;
                        if (!contactDict.TryGetValue(contact.ContactId, out entry))
                        {
                            entry = contact;
                            entry.Phones = new List<Phone>();
                            contactDict.Add(entry.ContactId, entry);
                        }

                        entry.Phones.Add(phone);
                        return entry;
                    }, parameter, splitOn: "ContactId", commandType: CommandType.StoredProcedure);
                    tran.Complete();
                    return query.FirstOrDefault();
                }
            }
            catch(Exception ex) 
            {
                logger.LogError($"Error in ContactsService/GetContactById action: {ex.Message}");
                throw;
            }
        }

        public async Task<int> CreateContactAsync(Contact contact)
        {
            int rowAffected = 0;
            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                using (IDbConnection con = new SqlConnection(connection))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@FirstName", contact.FirstName);
                    parameters.Add("@LastName", contact.LastName);
                    parameters.Add("@Address", contact.Address);
                    rowAffected = await con.ExecuteAsync("sp_InsertContact", parameters, commandType: CommandType.StoredProcedure);
                    tran.Complete();
                }
             
                return rowAffected;
            }
            catch (Exception ex) 
            {
                logger.LogError($"Error in ContactsServiceCreateContact action: {ex.Message}");
                throw;
            }
        }

        public async Task<int> UpdateContactAsync(Contact contact, int? id)
        {
           
            int rowAffected = 0;
            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                using (IDbConnection con = new SqlConnection(connection))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ContactId", id);
                    parameters.Add("@Firstname", contact.FirstName);
                    parameters.Add("@LastName", contact.LastName);
                    parameters.Add("@Address", contact.Address);
                    rowAffected = await con.ExecuteAsync("sp_UpdateContact", parameters, commandType: CommandType.StoredProcedure);
                    tran.Complete();
                }
                return rowAffected;
            }
            catch(Exception ex)
            {
                logger.LogError($"Error in ContactsService/UpdateContact action: {ex.Message}");
                throw;
            }
        }
        public async Task<int> DeleteContactAsync(int? id)
        {

            int rowAffected = 0;
            try
            {
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                using (IDbConnection con = new SqlConnection(connection))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ContactId", id);
                    rowAffected = await con.ExecuteAsync("sp_DeleteContact", parameters, commandType: CommandType.StoredProcedure);
                    tran.Complete();
                }
                
                return rowAffected;
            }
            catch (Exception ex) 
            {
                logger.LogError($"Error in ContactsService/DeleteContact action: {ex.Message}");
                throw;
            }
        }
    }
}
