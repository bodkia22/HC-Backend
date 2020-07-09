using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using HC.Business.Models.DTO;

namespace HC.Business.Validators
{
    public sealed class RecoveryPasswordDataDtoValidator : AbstractValidator<RecoveryPasswordDataDto>
    {
        public RecoveryPasswordDataDtoValidator()
        {
            RuleFor(x => x.Data)
                .NotEmpty()
                .WithMessage("Data must be not empty");
        }
    }
}
