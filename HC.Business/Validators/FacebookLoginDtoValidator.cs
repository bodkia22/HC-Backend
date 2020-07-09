using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using HC.Business.Models.DTO;

namespace HC.Business.Validators
{
    class FacebookLoginDtoValidator : AbstractValidator<FacebookLoginDto>
    {
        public FacebookLoginDtoValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty()
                .WithMessage("Access token mustn't be empty");
        }
    }
}
