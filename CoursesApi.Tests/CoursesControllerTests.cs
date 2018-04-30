using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CoursesApi.Controllers;
using Xunit;
using System.Threading.Tasks;

namespace CoursesApi.Tests
{
    public class CoursesControllerTests
    {
        private FakeCoursesContext _fakeContext;
        private CoursesController _coursesController;

        public CoursesControllerTests()
        {
            _fakeContext = new FakeCoursesContext();
            _coursesController = new CoursesController(_fakeContext);
        }

        [Fact]
        public async Task ShouldListCoursesProperly()
        {
            await CheckInitialCourses();
            await CheckAfterAddingCourse();
            await CheckAfterAddingChapter();
            await CheckAfterAddingLesson();
        }

        public async Task CheckInitialCourses()
        {
            var courses = await _coursesController.GetAllCourses();
            var okResult = courses as OkObjectResult;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            var result = (IEnumerable<CourseResponse>)okResult.Value;

            var (coursesExpected, coursesActual) = (SampleData.Courses, result);
            Assert.Equal(coursesExpected.Select(x => x.Name), coursesActual.Select(x => x.Name));

            var (chaptersExpected, chaptersActual) = (coursesExpected.SelectMany(x => x.Chapters), coursesActual.SelectMany(x => x.Chapters));
            Assert.Equal(chaptersExpected.Select(x => x.Name), chaptersActual.Select(x => x.Name));

            var (lessonsExpected, lessonsActual) = (chaptersExpected.SelectMany(x => x.Lessons), chaptersActual.SelectMany(x => x.Lessons));
            Assert.Equal(lessonsExpected.Select(x => x.Name), lessonsActual.Select(x => x.Name));
        }

        public async Task CheckAfterAddingCourse()
        {
            var req = new CreateCourseRequest { Name = "Course Test 1232222" };
            var res = await _coursesController.CreateCourse(req);
            var createOkRes = res as OkObjectResult;
            Assert.NotNull(createOkRes);
            Assert.Equal(200, createOkRes.StatusCode);

            var courses = await _coursesController.GetAllCourses();
            var getOkRes = courses as OkObjectResult;

            Assert.NotNull(getOkRes);
            Assert.Equal(200, getOkRes.StatusCode);

            var result = (IEnumerable<CourseResponse>)getOkRes.Value;
            Assert.Contains(req.Name, result.Select(x => x.Name));
        }


        public async Task CheckAfterAddingChapter()
        {
            var req = new CreateChapterRequest { Name = "Chapter Test 1232222" };
            var res = await _coursesController.CreateChapter(1, req);
            var okRes = res as OkObjectResult;
            Assert.NotNull(okRes);
            Assert.Equal(200, okRes.StatusCode);

            var courses = await _coursesController.GetAllCourses();
            var getOkRes = courses as OkObjectResult;

            Assert.NotNull(getOkRes);
            Assert.Equal(200, getOkRes.StatusCode);

            var result = (IEnumerable<CourseResponse>) getOkRes.Value;
            Assert.Contains(req.Name, result.SelectMany(x => x.Chapters.Select(y => y.Name)));
        }

        public async Task CheckAfterAddingLesson()
        {
            var req = new CreateLessonRequest { Name = "Lesson Test 1232222" };
            var res = await _coursesController.CreateLesson(1,1,req);
            var okRes = res as OkObjectResult;
            Assert.NotNull(okRes);
            Assert.Equal(200, okRes.StatusCode);

            var courses = await _coursesController.GetAllCourses();
            var getOkRes = courses as OkObjectResult;

            Assert.NotNull(getOkRes);
            Assert.Equal(200, getOkRes.StatusCode);

            var result = (IEnumerable<CourseResponse>)getOkRes.Value;
            Assert.Contains(req.Name, result.SelectMany(x => x.Chapters.SelectMany(y => y.Lessons.Select(z => z.Name))));
        }
    }
}
