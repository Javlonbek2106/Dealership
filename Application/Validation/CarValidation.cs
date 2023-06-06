using FluentValidation;

namespace Application.Validation
{
    public class CarValidation : AbstractValidator<Car>
    {
        public CarValidation() 
        {
            RuleFor(x => x.Brand)
            .NotEmpty()
            .WithMessage("Name is required.");

        }
    }
}
