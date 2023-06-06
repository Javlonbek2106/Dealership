using FluentValidation;

namespace Application.Validation
{
    public class DealershipValidation : AbstractValidator<Dealership>
    {
        public DealershipValidation()
        {
            RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.");

            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Address is required.");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("Phone number is required.")
                //.Matches(@"^\+998\d{9}$")
                .WithMessage("Invalid phone number format. Phone number should start with '+998' and contain 9 additional digits.");

            RuleFor(x => x.WorkingHours)
                .NotEmpty()
                .WithMessage("Working hours are required.")
                .MaximumLength(20)
                .WithMessage("Working hours can have a maximum length of 20 characters.")
                .MinimumLength(3)
                .WithMessage("Working hours must have a minimum length of 5 characters.");
        }
    }
}
