using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TestAppAddressBook.Models;
using System.Linq;
using System.Transactions;

namespace TestAppAddressBook.Services
{
    public class ContactsService : IContactsService
    {
        private readonly string connection;
        public ContactsService(string _connection) 
        {
            connection = _connection;
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
                    var contacts = await con.QueryAsync<Contact, Phone, Contact>("GetContacts", (contact, phone) =>
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
                    parameter.Add("@Id", id);
                    var query = await con.QueryAsync<Contact, Phone, Contact> ("GetContactById", (contact, phone) => 
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
                 
                    return query.FirstOrDefault();
                }
            }
            catch(Exception ex) 
            {
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
                    rowAffected = await con.ExecuteAsync("InsertContact", parameters, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex) 
            {
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
                    rowAffected = await con.ExecuteAsync("UpdateContact", parameters, commandType: CommandType.StoredProcedure);
                }

                return rowAffected;
            }
            catch(Exception ex)
            {
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
                    parameters.Add("@Id", id);
                    rowAffected = await con.ExecuteAsync("DeleteContact", parameters, commandType: CommandType.StoredProcedure);
                }

                return rowAffected;
            }
            catch (Exception ex) 
            {
                throw;
            }
        }
    }
}
