using System;
using System.Collections.Generic;
using System.Text;
using HC.Data.Entities.Base;

namespace HC.Data.Entities
{
    public class Course : EntityBase
    {
        public Course()
        {
            CoursesToStudents = new HashSet<CourseToStudent>();
        }

        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Info { get; set; }

        public int CreatorId { get; set; } 
        public Admin Admin { get; set; }

        public HashSet<CourseToStudent> CoursesToStudents { get; set; }
    }
}
