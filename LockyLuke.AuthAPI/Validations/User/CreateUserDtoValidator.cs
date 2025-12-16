using FluentValidation;
using SharedLibrary.Dtos.User;

namespace LockyLuke.AuthAPI.Validations.User
{
    public class CreateUserDtoValidator:AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("E-Mail address cannot be empty").EmailAddress().WithMessage("E-Mail format is wrong");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password cannot be empty").MinimumLength(8).WithMessage("Password must be at least 8 character");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName cannot be empty").MinimumLength(3).WithMessage("UserName must be at least 3 character");
        }
    }
}
