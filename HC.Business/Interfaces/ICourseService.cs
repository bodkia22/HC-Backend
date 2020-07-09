using System.Collections.Generic;
using System.Threading.Tasks;
using HC.Business.Models.VM;

namespace HC.Business.Interfaces
{
    public interface ICourseService
    {
        public Task<List<CourseToStudentViewModel>> GetCoursesByStudentId(int userId);
        public Task<List<CourseToStudentViewModel>> GetCoursesByStudentEmail(string email);
        public Task<bool> GetIsUserSubscribedToTheCourse(int courseId, int userId);
        public Task<List<CourseViewModel>> GetAllCourses();
        public Task<CourseViewModel> GetCourseById(int courseId);
    }
}