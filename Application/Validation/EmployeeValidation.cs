using FluentValidation;

namespace Application.Validation
{
    public class EmployeeValidation : AbstractValidator<Employee>
    {
        public EmployeeValidation()
        {
            RuleFor(employee => employee.FullName)
                .NotEmpty()
                .WithMessage("Please enter the name.");

            RuleFor(employee => employee.Position)
                .NotEmpty()
                .WithMessage("Please enter the position.");

            RuleFor(employee => employee.Phone)
                .NotEmpty()
                .WithMessage("Please enter the phone number.")
                //.Matches(@"^\+998\d{9}$")
                .WithMessage("Invalid phone number format. Phone number should start with '+998' and contain 9 additional digits.");


            RuleFor(x => x.Username)
            .NotEmpty()
            .NotNull()
            .MaximumLength(20)
            .MinimumLength(5)
            .WithMessage("Username is not valid");


            RuleFor(x => x.Password)
              .NotEmpty()
              .NotNull()
              //.Matches("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$")
                .WithMessage("Password is not valid")
              .MinimumLength(6)
              .WithMessage("Password is not valid");
        }
    }
}
