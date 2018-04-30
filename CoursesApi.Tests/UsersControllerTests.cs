using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoursesApi.Controllers;
using CoursesApi.Infrastructure;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace CoursesApi.Tests
{
    //TODO: ref achievements by enum
    //TODO: chunk tests
    //TODO: find way to isolate context
    public class UsersControllerTests
    {
        private FakeCoursesContext _fakeContext;
        private AchievementValidator _achievementValidator;
        private AchievementService _achievementService;
        private UsersController _usersController;

        public UsersControllerTests()
        {
            _fakeContext = new FakeCoursesContext();
            _achievementValidator = new AchievementValidator(_fakeContext);
            _achievementService = new AchievementService(_fakeContext, _achievementValidator);
            _usersController = new UsersController(_fakeContext, _achievementService);
        }

        [Fact]
        public async Task ShouldUpdateAchievementsProperly()
        {
            await CheckInitialAchievements();
            await CheckCompletingLessons();
            await CheckAchievementsAfterCompletingLessons();
        }

        public async Task CheckInitialAchievements()
        {
            var response = await _usersController.GetAllAchievements(userId: 2);
            var okResult = response as OkObjectResult;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            var result = (IEnumerable<AchievementResponse>)okResult.Value;
            var completedAchievements = result.Where(x => x.IsCompleted).Select(x => x.Id);
            Assert.Empty(completedAchievements);
        }

        public async Task CheckCompletingLessons()
        {
            var user = _fakeContext.GetUserWithLessonCompletions(2);

            var course = _fakeContext.Courses
                .Include(x => x.Chapters)
                .ThenInclude(s => s.Lessons)
                .FirstOrDefault(x => x.Id == 1);

            var lessonCompletionRequests = course.Chapters.SelectMany(chapter => chapter.Lessons.Select(lesson => new CreateLessonCompletionRequest
            {
                LessonId = lesson.Id,
                LessonCompletedAt = DateTime.Now,
                LessonStartedAt = DateTime.Now.AddHours(-1)
            }));

            foreach (var req in lessonCompletionRequests)
            {
                var res = await _usersController.CompleteLesson(2, req);
                var okRes = res as OkObjectResult;
                Assert.NotNull(okRes);
                Assert.Equal(200, okRes.StatusCode);
            }
        }

        public async Task CheckAchievementsAfterCompletingLessons()
        {
            var response = await _usersController.GetAllAchievements(userId: 2);
            var okResult = response as OkObjectResult;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            var result = (IEnumerable<AchievementResponse>)okResult.Value;
            var completedAchievements = result.Where(x => x.IsCompleted).Select(x => x.Id).ToArray();
            var achievementsThatShouldBeCompleted = new int[] { 1, 2, 4, 6 };
            Assert.Equal(completedAchievements, achievementsThatShouldBeCompleted);
        }
    }
}
