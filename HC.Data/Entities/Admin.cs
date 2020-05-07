using System;
using System.Collections.Generic;
using System.Text;
using HC.Data.Entities.Base;

namespace HC.Data.Entities
{
    public class Admin : EntityBase
    {
        public Admin()
        {
            Courses = new HashSet<Course>();
        }

        public int UserId { get; set; }
        public User User { get; set; }

        public int CourseId { get; set; }
        public HashSet<Course> Courses { get; set; }
    }
}
