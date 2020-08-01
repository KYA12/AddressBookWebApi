using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAppAddressBook.ViewModels
{
    public class ContactViewModel
    {
        public string User {get; set; }
        public List<PhoneViewModel> Phones { get; set; }

    }
}
