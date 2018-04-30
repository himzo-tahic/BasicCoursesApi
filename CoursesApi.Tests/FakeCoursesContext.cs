using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CoursesApi.Models;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CoursesApi.Tests
{
    public class FakeCoursesContext : CoursesDbContext
    {
        static readonly DbContextOptions<CoursesDbContext> Options = new DbContextOptionsBuilder<CoursesDbContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;

        public FakeCoursesContext() : base(Options)
        {
            SampleData.Create(this);
        }

        public (int lessonsCompleted, int chaptersCompleted) FinishCourse(int userId, int courseId)
        {
            var user = GetUserWithLessonCompletions(userId);

            var course = Courses
                .Include(x => x.Chapters)
                .ThenInclude(s => s.Lessons)
                .FirstOrDefault(x => x.Id == courseId);

            var lessonCompletions = course.Chapters.SelectMany(chapter => chapter.Lessons.Select(lesson => new LessonCompletion
            {
                Lesson = lesson,
                LessonCompletedAt = DateTime.Now,
                LessonId = lesson.Id,
                LessonStartedAt = DateTime.Now.AddHours(-1),
                User = user,
                UserId = user.Id
            })).ToList();

            LessonCompletions.AddRange(lessonCompletions);
            SaveChanges();
            return (lessonCompletions.Count(), course.Chapters.Count());
        }


        public User GetUserWithLessonCompletions(int id) => Users
            .Include(x => x.LessonCompletions)
            .First(x => x.Id == id);
    }
}
