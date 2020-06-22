using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hangfire;
using HC.Business.Interfaces;
using HC.Business.Models;
using HC.Business.Models.DTO;
using HC.Business.Models.VM;
using HC.Data;
using HC.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HC.Business.Services
{
    public class StudentService : IStudentService
    {
        private readonly IHCDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailSenderService _mailSenderService;
        private readonly UserManager<User> _userManager;

        public StudentService(HCDbContext context, IMapper mapper, IEmailSenderService mailSenderService,
            UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _mailSenderService = mailSenderService;
            _userManager = userManager;
        }

        public async Task<SubscribeToCourseViewModel> SubscribeToCourse(SubscribeToCourseDto subscribeToCourse)
        {
            var courseToStudent = _mapper.Map<CourseToStudent>(subscribeToCourse);

            if (courseToStudent != null && _context.CoursesToStudents.FirstOrDefault(x =>
                    x.CourseId == courseToStudent.CourseId && x.StudentId == courseToStudent.StudentId) == null)
            {
                _context.CoursesToStudents.Add(courseToStudent);

                await _context.SaveChangesAsync();
                 
                courseToStudent = await _context.CoursesToStudents
                    .Include(x => x.Course)
                    .Include(x => x.Student)
                    .FirstOrDefaultAsync(x => x.CourseId == courseToStudent.CourseId && x.StudentId == courseToStudent.StudentId);

                await _mailSenderService.SendEmailAsync(courseToStudent.Student.Email,
                    "You have been registered to the course !",
                    $"<p>Congratulations, you have been successfully registered to " +
                    $"the {courseToStudent.Course.Name}.</p>" +
                    $"<p>This course will be available for you from {courseToStudent.StartDate:d} to {courseToStudent.EndDate:d}</p>");

                var mouthDate = courseToStudent.StartDate.AddMonths(-1);
                var weekDate = courseToStudent.StartDate.AddDays(-7);
                var dayDate = courseToStudent.StartDate.AddDays(-1).Add(new TimeSpan(8, 0, 0));

                if (mouthDate > DateTime.Today)
                {
                    BackgroundJob.Schedule(
                        () => _mailSenderService.SendEmailAsync(courseToStudent.Student.Email,
                            "Honey Courses, reminders about the upcoming course !",
                            $"<p>Hi {courseToStudent.Student.UserName}, we'd like to remind you that you have exactly " +
                            $"1 month before the start of the '{courseToStudent.Course.Name}' course.</p>" +
                            $"<p>Course date: {courseToStudent.StartDate}</p>"),
                        mouthDate - DateTime.Today - DateTime.Now.TimeOfDay);
                }

                if (weekDate > DateTime.Today)
                {
                    BackgroundJob.Schedule(
                        () => _mailSenderService.SendEmailAsync(courseToStudent.Student.Email,
                            "Honey Courses, reminders about the upcoming course !",
                            $"<p>Hello again {courseToStudent.Student.UserName}, we want to remind you that your course" +
                            $"'{courseToStudent.Course.Name}' will start in  1 week.</p>" +
                            $"<p>Course date: {courseToStudent.StartDate}</p>"),
                        weekDate - DateTime.Today - DateTime.Now.TimeOfDay);
                }

                if (dayDate > DateTime.Today.Add(new TimeSpan(8, 0, 0)))
                {
                    BackgroundJob.Schedule(
                        () => _mailSenderService.SendEmailAsync(courseToStudent.Student.Email,
                            "Honey Courses, reminders about the upcoming course !",
                            $"<p>Hi {courseToStudent.Student.UserName}, we'd like to remind you that '{courseToStudent.Course.Name}'" +
                            $" is going to start tomorrow.</p>" +
                            $"<p>Course date: {courseToStudent.StartDate}</p>"),
                        dayDate - DateTime.Today - DateTime.Now.TimeOfDay);
                }

                return _mapper.Map<SubscribeToCourseViewModel>(courseToStudent);
            }

            return null;
        }
    }
}
