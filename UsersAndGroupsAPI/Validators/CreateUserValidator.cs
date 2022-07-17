using FluentValidation;
using UsersAndGroupsAPI.Models;

namespace UsersAndGroupsAPI.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Name).Length(2, 20).WithMessage("Name must be between 2 and 20 characters long");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email must be in correct format.");
            RuleFor(x => x.GroupId).NotEmpty().WithMessage("You must provide group");
            RuleFor(x => x.Password).Length(5, 50).WithMessage("Password must be between 5 and 50 characters long.");
        }
    }

}
