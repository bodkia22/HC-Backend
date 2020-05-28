using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using FluentValidation;
using HC.Business.Models;
using HC.Business.Models.DTO;

namespace HC.Business.Validators
{
    public sealed class UserForRegisterDtoValidator : AbstractValidator<UserForRegisterDto>
    {
        public UserForRegisterDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("'First name' is Required");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("'Last name' is Required");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("'Email' is Required");

            RuleFor(x => x.NickName)
                .NotEmpty()
                .Must(RegexUserName)
                .MaximumLength(20)
                .WithMessage("Nick name must be less than 15 symbols. Only letters is Required"); 

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(50)
                .WithMessage("'Password' is required. Min length 8 - max 50");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty();
        }

        private bool RegexUserName(string nickName)
        {
            Regex reg = new Regex("^[a-zA-Z0-9]+$");

            return reg.IsMatch(nickName);
        }
    }
}
