using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAppAddressBook.Models
{
    public class Contact
    {
        public int ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public List<Phone> Phones { get; set; }
 
    }
}
