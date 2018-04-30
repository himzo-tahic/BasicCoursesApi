using System;
using System.Linq;
using CoursesApi.Models;

namespace CoursesApi.Controllers
{
    static class CreateSampleData
    {
        static readonly Course[] Courses = {
            new Course {
                Name = "Swift",
                CreatedAt = DateTime.Now
            },
            new Course {
                Name = "C#",
                CreatedAt = DateTime.Now
            },
            new Course {
                Name = "JavaScript",
                CreatedAt = DateTime.Now
            },
        };

        static readonly Chapter[] Chapters = {
            new Chapter {
                Name = "Chapter 0 - 0",
                Course = Courses[0],
                CourseId = Courses[0].Id,
                CreatedAt = DateTime.Now
            },
            new Chapter {
                Name = "Chapter 0 - 1",
                Course = Courses[0],
                CourseId = Courses[0].Id,
                CreatedAt = DateTime.Now
            },
            new Chapter {
                Name = "Chapter 1 - 0",
                Course = Courses[1],
                CourseId = Courses[1].Id,
                CreatedAt = DateTime.Now
            },
            new Chapter {
                Name = "Chapter 1 - 1",
                Course = Courses[1],
                CourseId = Courses[1].Id,
                CreatedAt = DateTime.Now
            },
            new Chapter {
                Name = "Chapter 1 - 2",
                Course = Courses[1],
                CourseId = Courses[1].Id,
                CreatedAt = DateTime.Now
            },
            new Chapter {
                Name = "Chapter 2 - 0",
                Course = Courses[2],
                CourseId = Courses[2].Id,
                CreatedAt = DateTime.Now
            },
        };

        static readonly Lesson[] Lessons = {
            new Lesson{
                Name = "Lesson 1",
                Chapter = Chapters[0],
                ChapterId = Chapters[0].Id,
                CreatedAt = DateTime.Now,
            },
            new Lesson{
                Name = "Lesson 2",
                Chapter = Chapters[0],
                ChapterId = Chapters[0].Id,
                CreatedAt = DateTime.Now,
            },
            new Lesson{
                Name = "Lesson 3",
                Chapter = Chapters[1],
                ChapterId = Chapters[1].Id,
                CreatedAt = DateTime.Now,
            },
            new Lesson{
                Name = "Lesson 4",
                Chapter = Chapters[2],
                ChapterId = Chapters[2].Id,
                CreatedAt = DateTime.Now,
            },
            new Lesson{
                Name = "Lesson 5",
                Chapter = Chapters[3],
                ChapterId = Chapters[3].Id,
                CreatedAt = DateTime.Now,
            },
            new Lesson{
                Name = "Lesson 6",
                Chapter = Chapters[3],
                ChapterId = Chapters[3].Id,
                CreatedAt = DateTime.Now,
            },
            new Lesson{
                Name = "Lesson 7",
                Chapter = Chapters[4],
                ChapterId = Chapters[4].Id,
                CreatedAt = DateTime.Now,
            },
            new Lesson{
                Name = "Lesson 8",
                Chapter = Chapters[5],
                ChapterId = Chapters[5].Id,
                CreatedAt = DateTime.Now,
            },
            new Lesson{
                Name = "Lesson 9",
                Chapter = Chapters[5],
                ChapterId = Chapters[5].Id,
                CreatedAt = DateTime.Now,
            },
            new Lesson{
                Name = "Lesson 10",
                Chapter = Chapters[5],
                ChapterId = Chapters[5].Id,
                CreatedAt = DateTime.Now,
            }
        };

        static readonly User[] Users = {
            new User{ Username = "Courses" }
        };

        static readonly Achievement[] Achievements = {
            new Achievement{ Name = "Complete 5 Lessons."},
            new Achievement{ Name = "Complete 25 Lessons."},
            new Achievement{ Name = "Complete 50 Lessons."},
            new Achievement{ Name = "Complete 1 chapter."},
            new Achievement{ Name = "Complete 5 chapters."},
            new Achievement{ Name = "Complete the Swift course."},
            new Achievement{ Name = "Complete the JavaScript course."},
            new Achievement{ Name = "Complete C# course."}
        };

        public static void Execute(CoursesDbContext context)
        {
            context.Courses.AddRange(Courses);
            context.Chapters.AddRange(Chapters);
            context.Lessons.AddRange(Lessons);
            context.Users.AddRange(Users);
            context.Achievements.AddRange(Achievements);
            context.SaveChanges();
        }
    }
}
