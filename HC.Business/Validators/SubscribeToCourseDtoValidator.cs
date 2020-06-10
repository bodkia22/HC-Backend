using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using HC.Business.Models;
using HC.Business.Models.DTO;

namespace HC.Business.Validators
{
    public sealed class SubscribeToCourseDtoValidator : AbstractValidator<SubscribeToCourseDto>
    {
        public SubscribeToCourseDtoValidator()
        {
            RuleFor(x => x.CourseId)
                .NotEmpty()
                .WithMessage(x => $"Can't be {x.CourseId}")
                .GreaterThan(0)
                .WithMessage(x => $"Must be greater than 0, but was {x.CourseId}");

            RuleFor(x => x.StudentId)
                .Empty()
                .WithMessage(x => $"StudentId should be empty. Not {x.StudentId}");

            RuleFor(x => x.StartDate)
                .GreaterThan(DateTime.Now)
                .WithMessage(x => $"Start date must be greater than now, not {x.StartDate}");
        }
    }
}
