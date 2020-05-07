using System;
using System.Collections.Generic;
using System.Text;
using HC.Data.Entities.Base;

namespace HC.Data.Entities
{
    public class Student : EntityBase
    {
        public Student()
        {
            CoursesToStudents = new HashSet<CourseToStudent>();
        }

        public int UserId { get; set; }
        public User User { get; set; }

        public HashSet<CourseToStudent> CoursesToStudents { get; set; }
    }
}
