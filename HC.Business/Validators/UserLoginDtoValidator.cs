using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using HC.Business.Models;
using HC.Business.Models.DTO;

namespace HC.Business.Validators
{
    public sealed class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("'UserName' is Required");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("'Password' is Required");
        }
    }
}
