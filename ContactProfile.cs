using AutoMapper;
using ContactsApp.ViewModel;
using Domain.Entities;

namespace ContactsApp
{
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            CreateMap<Contact, ContactViewModel>().ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName)).ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName)).ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email)).ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));
            CreateMap<ContactViewModel, Contact>().ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName)).ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName)).ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email)).ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));
        }
    }
}
