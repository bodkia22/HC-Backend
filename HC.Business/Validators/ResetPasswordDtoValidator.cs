using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using HC.Business.Models.DTO;

namespace HC.Business.Validators
{
    public sealed class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage(x => $"User Id must be not empty. Your entered {x.UserId}.");

            RuleFor(x => x.ResetToken)
                .NotEmpty()
                .WithMessage(x => $"Reset token mustn't be empty. Your entered {x.ResetToken}.");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(128)
                .WithMessage("'Password' is required. Min length 8 symbols max is 128.");
        }
    }
}
