//using System.Linq;
//using System.Collections.Generic;

//namespace CoursesApi.Models.Achievements
//{
//    public class Achievements
//    {
//        public static Achievement[] GetAchievements()
//        {
//            List<Chapter> getCompletedChapters(List<LessonCompletion> completedLessons)
//            {
//                bool chapterIsFinished(Chapter chapter, List<LessonCompletion> completedInThisChapter) =>
//                    chapter.Lessons.Count == completedInThisChapter.Select(s => s.LessonId).Distinct().Count();

//                return completedLessons
//                    .GroupBy(x => x.Lesson.Chapter)
//                    .Where(x => chapterIsFinished(x.Key, x.ToList()))
//                    .Select(x => x.Key)
//                    .ToList();
//            }

//            return new Achievement[]
//            {
//                new Achievement
//                {
//                    Name = "Complete 5 lessons.",
//                    Validate = (lessons) => (lessons.Count, lessons.Count >= 5)
//                },
//                new Achievement
//                {
//                    Name = "Complete 25 lessons.",
//                    Validate = (lessons) => (lessons.Count, lessons.Count >= 25)
//                },
//                new Achievement
//                {
//                    Name = "Complete 50 lessons.",
//                    Validate = (lessons) => (lessons.Count, lessons.Count >= 50)
//                },
//                new Achievement
//                {
//                    Name = "Complete 1 chapter.",
//                    Validate = (lessons) =>
//                    {
//                        var finishedChapters = getCompletedChapters(lessons);
//                        return (finishedChapters.Count, finishedChapters.Count >= 1);
//                    }
//                },
//                new Achievement
//                {
//                    Name = "Complete 5 chapters.",
//                    Validate = (lessons) =>
//                    {
//                        var finishedChapters = getCompletedChapters(lessons);
//                        return (finishedChapters.Count, finishedChapters.Count >= 5);
//                    }
//                },
//                new Achievement
//                {
//                    Name = "Complete the Swift course.",
//                    Validate = (lessons) =>
//                    {
//                        var completedChapters = getCompletedChapters(lessons);
//                        var completedSwiftChapters = completedChapters.GroupBy(x => x.Course).FirstOrDefault(x => x.Key.Name == "Swift");
//                        var shouldHave = completedSwiftChapters.Key.Chapters.Count();
//                        var doesHave =  completedSwiftChapters.Distinct().Count();
//                        return (doesHave, shouldHave == doesHave);
//                    }
//                },
//                new Achievement
//                {
//                    Name = "Complete the JavaScript course.",
//                    Validate = (lessons) =>
//                    {
//                        var completedChapters = getCompletedChapters(lessons);
//                        var completedSwiftChapters = completedChapters.GroupBy(x => x.Course).FirstOrDefault(x => x.Key.Name == "JavaScript");
//                        var shouldHave = completedSwiftChapters.Key.Chapters.Count();
//                        var doesHave =  completedSwiftChapters.Distinct().Count();
//                        return (doesHave, shouldHave == doesHave);
//                    }
//                },
//                new Achievement
//                {
//                    Name = "Complete the C# course.",
//                    Validate = (lessons) =>
//                    {
//                        var completedChapters = getCompletedChapters(lessons);
//                        var completedSwiftChapters = completedChapters.GroupBy(x => x.Course).FirstOrDefault(x => x.Key.Name == "C#");
//                        var shouldHave = completedSwiftChapters.Key.Chapters.Count();
//                        var doesHave =  completedSwiftChapters.Distinct().Count();
//                        return (doesHave, shouldHave == doesHave);
//                    }
//                },
//            };
//        }
//    }
//}
