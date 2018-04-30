using System;
using System.Linq;
using System.Collections.Generic;
using CoursesApi.Models;

namespace CoursesApi.Infrastructure
{
    //TODO: loads way too much in memory
    //TODO: validates ALL achievements every time.
    //TODO: too much dbcontext access.
    //TODO: parallelize validation
    //TODO: enum for achievements
    public class AchievementValidator
    {
        readonly CoursesDbContext _context;

        public AchievementValidator(CoursesDbContext context)
        {
            _context = context;
            _validators = new Dictionary<int, Func<List<LessonCompletion>, AchievementValidationResult>>
            {
                [1] = (lessons) => ValidateLessonCount(lessons, 5),
                [2] = (lessons) => ValidateLessonCount(lessons, 25),
                [3] = (lessons) => ValidateLessonCount(lessons, 50),
                [4] = (lessons) => ValidateChapterCount(lessons, 1),
                [5] = (lessons) => ValidateChapterCount(lessons, 5),
                [6] = (lessons) => ValidateCourseCompleted(lessons, "Swift"),
                [7] = (lessons) => ValidateCourseCompleted(lessons, "JavaScript"),
                [8] = (lessons) => ValidateCourseCompleted(lessons, "C#"),
            };
        }

        public Dictionary<int, Func<List<LessonCompletion>, AchievementValidationResult>> _validators;
           

        public List<(int achievementId, AchievementValidationResult validation)> Execute(List<LessonCompletion> lessonCompletions)
        {
            return _validators.Select(validator => (validator.Key, validator.Value(lessonCompletions))).ToList();
        }

        public AchievementValidationResult ValidateLessonCount(List<LessonCompletion> lessonCompletions, int goal)
        {
            return new AchievementValidationResult
            {
                Progress = lessonCompletions.Select(x => x.LessonId).Distinct().Count(),
                Goal = goal
            };
        }

        public AchievementValidationResult ValidateChapterCount(List<LessonCompletion> lessonCompletions, int goal)
        {
            var completedLessonIds = lessonCompletions.Select(x => x.LessonId).Distinct();
            var completedChapters = _context.Chapters.Where(ch => ch.Lessons.All(x => completedLessonIds.Contains(x.Id)));
                                                                                 
            return new AchievementValidationResult
            {
                Progress = completedChapters.Count(),
                Goal = goal
            };
        }

        public AchievementValidationResult ValidateCourseCompleted(List<LessonCompletion> lessonCompletions, string courseName)
        {
            var completedLessonIds = lessonCompletions.Select(x => x.LessonId).Distinct();
            var course = _context.Courses.Where(x => x.Chapters.All(ch => ch.Lessons.All(l => completedLessonIds.Contains(l.Id))));

            return new AchievementValidationResult
            {
                Goal = 1,
                Progress = course.Any(x => x.Name == courseName) ? 1 : 0
            };
        }
    }

    public class AchievementValidationResult 
    {
        public int Progress { get; set; }
        public int Goal { get; set; }
    }
}
