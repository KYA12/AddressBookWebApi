using AutoMapper;
using TestAppAddressBook.Models;
using TestAppAddressBook.ViewModels;

namespace TestAppAddressBook
{
    public class AutoMapperProfile : Profile
    {
        //Setup AutoMapper profile settings
        public AutoMapperProfile()
        {
            CreateMap<Contact, ContactViewModel>().ForMember(destination => destination.User,
                options => options.MapFrom(source => source.FirstName + " " + source.LastName)).ReverseMap();
            CreateMap<PhoneViewModel, Phone>().ReverseMap();
            CreateMap<AddUpdateContactViewModel, Contact>().ReverseMap();
        }
    }
}