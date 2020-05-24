﻿using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using HC.Business.Models;

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
                .NotEmpty()
                .WithMessage(x => $"Can't be {x.StudentId}")
                .GreaterThan(0)
                .WithMessage(x => $"Must be greater than 0, but was {x.StudentId}");
        }
    }
}