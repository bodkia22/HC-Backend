using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using HC.Business.Models.DTO;

namespace HC.Business.Validators
{
    public sealed class DataForUsersSortDtoValidator : AbstractValidator<DataForUsersSortDto>
    {
        public DataForUsersSortDtoValidator()
        {
            RuleFor(x => x.SearchString)
                .NotNull()
                .WithMessage("Searching string must be not null");

            RuleFor(x => x.Current)
                .GreaterThan(-1)
                .WithMessage("Current page should be greater than -1");

            RuleFor(x => x.PageSize)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage(x => $"Page size must be not 0 and empty, your entered {x.PageSize}");
        }
    }
}
