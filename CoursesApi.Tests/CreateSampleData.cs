using System;
using System.Linq;
using CoursesApi.Models;

namespace CoursesApi.Tests
{
    static class SampleData
    {
        public static readonly Course[] Courses = {
            new Course {
                Name = "Swift",
                CreatedAt = DateTime.Now
            },
            new Course {
                Name = "JavaScript",
                CreatedAt = DateTime.Now
            },
            new Course {
                Name = "C#",
                CreatedAt = DateTime.Now
            }
        };

        public static readonly Chapter[] Chapters = {
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

        public static readonly Lesson[] Lessons = Chapters.SelectMany(chapter => 
            Enumerable.Range(1,20).Select(i => new Lesson
        {
            Chapter = chapter,
            ChapterId = chapter.Id,
            CreatedAt = DateTime.Now,
            LastUpdatedAt = null,
            Name = $"Lesson {chapter.Id}-{i}"
        })).ToArray();
  

        public static readonly User[] Users = {
            new User{ Id = 1, Username = "achievement-validator-tester" },
            new User{ Id = 2, Username = "achievement-service-tester" }
        };

        public static readonly Achievement[] Achievements = {
            new Achievement{Name = "Complete 5 Lessons."},
            new Achievement{Name = "Complete 25 Lessons."},
            new Achievement{Name = "Complete 50 Lessons."},
            new Achievement{Name = "Complete 1 chapter."},
            new Achievement{Name = "Complete 5 chapters."},
            new Achievement{Name = "Complete the Swift course."},
            new Achievement{Name = "Complete the JavaScript course."},
            new Achievement{Name = "Complete C# course."}
        };

        public static void Create(CoursesDbContext context)
        {
            try
            {
                context.Courses.AddRange(Courses);
                context.Chapters.AddRange(Chapters);
                context.Lessons.AddRange(Lessons);
                context.Users.AddRange(Users);
                context.Achievements.AddRange(Achievements);

                context.SaveChanges();
            } catch (InvalidOperationException)
            {
                //ignore, xunit multi threading inmemory db makes it tricky to initialize sample data.
            }
            catch (ArgumentException)
            {
                //ignore, xunit multi threading inmemory db makes it tricky to initialize sample data.
            }
        }
    }
}
