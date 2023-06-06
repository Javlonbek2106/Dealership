using FluentValidation;

namespace Application.Validation
{
    public class SoldCarValidation : AbstractValidator<SoldCar>
    {
        public SoldCarValidation()
        {
            //RuleFor(x => x.)
            //.NotEmpty()
            //.WithMessage("SoldPrice is required.");
        }
    }
}
