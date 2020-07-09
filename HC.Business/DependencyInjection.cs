using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AutoMapper;
using FluentValidation;
using HC.Business.Models;
using HC.Business.Models.DTO;
using HC.Business.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace HC.Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddTransient<IValidator<SubscribeToCourseDto>, SubscribeToCourseDtoValidator>();
            services.AddTransient<IValidator<UserLoginDto>, UserLoginDtoValidator>();
            services.AddTransient<IValidator<UserForRegisterDto>, UserForRegisterDtoValidator>();
            services.AddTransient<IValidator<ResetPasswordDto>, ResetPasswordDtoValidator>();
            services.AddTransient<IValidator<RecoveryPasswordDataDto>, RecoveryPasswordDataDtoValidator>();
            services.AddTransient<IValidator<FacebookLoginDto>, FacebookLoginDtoValidator>();
            services.AddTransient<IValidator<DataForUsersSortDto>, DataForUsersSortDtoValidator>();

            return services;
        }
    }
}
