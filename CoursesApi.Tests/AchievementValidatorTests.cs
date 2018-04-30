using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CoursesApi.Infrastructure;
using CoursesApi.Models;
using Xunit;

namespace CoursesApi.Tests.AchievementValidatorTests
{
    //TODO: chunk into smaller tests
    //TODO: make inmemory context run isolated
    //TODO: use enum to address the achievements instead of magic numbers
    public class AchievementValidatorTests
    {
        private FakeCoursesContext _fakeContext;
        private AchievementValidator _achievementValidator;

        public AchievementValidatorTests()
        {
            _fakeContext = new FakeCoursesContext();
            _achievementValidator = new AchievementValidator(_fakeContext);
        }

        [Fact]
        public void ValidatesProgressesCorrectly()
        {
            //Initially: 0 completed lessons, chapters and courses
            CheckCorrectProgress(1, 0, 0, new int[0]);

            //Finish one course. Chapter, lesson and course progress should update.
            var (lessonsCompletedOne, chaptersCompletedOne) = _fakeContext.FinishCourse(userId: 1, courseId: 1);
            CheckCorrectProgress(1, lessonsCompletedOne, chaptersCompletedOne, new int[] { 1 });

            //Finish another course.
            var (lessonsCompletedTwo, chaptersCompletedTwo) = _fakeContext.FinishCourse(userId: 1, courseId: 2);
            CheckCorrectProgress(1, lessonsCompletedOne + lessonsCompletedTwo, chaptersCompletedOne + chaptersCompletedTwo, new int[] { 1, 2 });
        }


        public void CheckCorrectProgress(int userId, int lessonCount, int chapterCount, int[] coursesCompleted)
        {
            var user = _fakeContext.GetUserWithLessonCompletions(userId);

            var achievementResult = _achievementValidator.Execute(user.LessonCompletions);

            var fiveLessonAchievement = achievementResult.First(ach => ach.achievementId == 1);
            Assert.Equal(fiveLessonAchievement.validation.Goal, 5);
            Assert.Equal(fiveLessonAchievement.validation.Progress, lessonCount);

            var twentyFiveLessonAchievement = achievementResult.First(ach => ach.achievementId == 2);
            Assert.Equal(twentyFiveLessonAchievement.validation.Goal, 25);
            Assert.Equal(twentyFiveLessonAchievement.validation.Progress, lessonCount);

            var fiftyLessonAchievement = achievementResult.First(ach => ach.achievementId == 3);
            Assert.Equal(fiftyLessonAchievement.validation.Goal, 50);
            Assert.Equal(fiftyLessonAchievement.validation.Progress, lessonCount);

            var oneChapterAchievement = achievementResult.First(ach => ach.achievementId == 4);
            Assert.Equal(oneChapterAchievement.validation.Goal, 1);
            Assert.Equal(oneChapterAchievement.validation.Progress, chapterCount);

            var fiveChapterAchievement = achievementResult.First(ach => ach.achievementId == 5);
            Assert.Equal(fiveChapterAchievement.validation.Goal, 5);
            Assert.Equal(fiveChapterAchievement.validation.Progress, chapterCount);

            var swiftAchievement = achievementResult.First(ach => ach.achievementId == 6);
            Assert.Equal(swiftAchievement.validation.Goal, 1);
            Assert.Equal(swiftAchievement.validation.Progress, coursesCompleted.Contains(1) ? 1 : 0);

            var javascriptAchievement = achievementResult.First(ach => ach.achievementId == 7);
            Assert.Equal(javascriptAchievement.validation.Goal, 1);
            Assert.Equal(javascriptAchievement.validation.Progress, coursesCompleted.Contains(2) ? 1 : 0);

            var csharpAchievement = achievementResult.First(ach => ach.achievementId == 8);
            Assert.Equal(csharpAchievement.validation.Goal, 1);
            Assert.Equal(csharpAchievement.validation.Progress, coursesCompleted.Contains(3) ? 1 : 0);
        }
    }
}
