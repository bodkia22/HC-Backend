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
        public string Info { get; set; }
        public string ImgUrl { get; set; }

        public int CreatorId { get; set; }
        public User Creator { get; set; }

        public HashSet<CourseToStudent> CoursesToStudents { get; set; }
    }
}
