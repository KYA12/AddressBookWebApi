using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestAppAddressBook.ViewModels
{
    public class AddUpdateContactViewModel
    {
        [Required(ErrorMessage = "Required First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Required Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Required Address")]
        public string Address { get; set; }

        public List<PhoneViewModel> Phones { get; set; }
    }
}
