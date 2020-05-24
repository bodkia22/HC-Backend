using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace HC.Data.Entities
{
    public class User : IdentityUser<int>
    {
        public User()
        {
            CoursesToStudents = new HashSet<CourseToStudent>();
            Courses = new HashSet<Course>();
            RegisteredDate = DateTime.Now;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        //TO DO: Delete set
        public DateTime RegisteredDate { get; set; } 
        public string EmailConfirmationToken { get; set; }
        public HashSet<Course> Courses { get; set; }
        public HashSet<CourseToStudent> CoursesToStudents { get; set; }
    }
}
