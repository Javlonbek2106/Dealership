using Domain.Entities.IdentityEntities;
using FluentValidation;

namespace Application.Validation
{
    public class PermissionValidation : AbstractValidator<Permission>
    {
        public PermissionValidation()
        {
            RuleFor(x => x.PermissionName)
    .NotEmpty()
    .WithMessage("Name is required.")
    .Length(3, 50)
    .WithMessage("Name must be between 3 and 50 characters long.");

        }
    }
}
