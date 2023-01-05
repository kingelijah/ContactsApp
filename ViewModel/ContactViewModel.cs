using FluentValidation;

namespace ContactsApp.ViewModel
{
    public class ContactViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class ContactValidator : AbstractValidator<ContactViewModel>
    {
        public ContactValidator()
        {
            RuleFor(contact => contact.FirstName).NotNull().NotEmpty().MaximumLength(250);
            RuleFor(contact => contact.LastName).NotNull().NotEmpty().MaximumLength(250);
            RuleFor(contact => contact.Email).NotNull().NotEmpty().EmailAddress().MaximumLength(250);
            RuleFor(contact => contact.PhoneNumber).NotNull().NotEmpty().MaximumLength(250);


        }
    }
}
