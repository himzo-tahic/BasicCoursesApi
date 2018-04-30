using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CoursesApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace CoursesApi.Controllers
{
    //TODO: Validation could use more DRY.
    //TODO: PUT/DELETE/PATCH For courses, chapters and lessons.
    [Route("courses")]
    public class CoursesController : Controller
    {
        readonly CoursesDbContext _context;

        public CoursesController(CoursesDbContext context)
        {
            _context = context;
        }

        //TODO: Scrolling
        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _context.Courses
               .Include(m => m.Chapters)
               .ThenInclude(s => s.Lessons)
               .ToListAsync();

            var result = courses.Select(CourseResponse.Create);
            return Ok(result);
        }

        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetCourse(int courseId)
        {
            var course = await _context.Courses
               .Include(m => m.Chapters)
               .ThenInclude(s => s.Lessons)
               .FirstOrDefaultAsync(x => x.Id == courseId);

            if (course == null)
                return NotFound($"Course [{courseId}] not found.");
            
            var result = CourseResponse.Create(course);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest createCourseRequest)
        {
            if (createCourseRequest == null)
                return BadRequest("Request body not specified.");
            
            var errors = createCourseRequest.Validate();
            if (errors.Any())
                return BadRequest(errors);
            
            var res = await _context.Courses.AddAsync(new Course {
                Name = createCourseRequest.Name,
                CreatedAt = DateTime.Now
            });

            await _context.SaveChangesAsync();
            return Ok(res.Entity.Id);
        }

        [HttpPost("{courseId}/chapters")]
        public async Task<IActionResult> CreateChapter(int courseId, [FromBody] CreateChapterRequest createChapterRequest)
        {
            if (createChapterRequest == null)
                return BadRequest("Request body not specified.");
            
            var errors = createChapterRequest.Validate();
            if (errors.Any())
                return BadRequest(errors);
            
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
                return NotFound($"Unknown course [{courseId}].");
            
            var res = await _context.Chapters.AddAsync(new Chapter
            {
                Name = createChapterRequest.Name,
                CourseId = course.Id,
                Course = course,
                CreatedAt = DateTime.Now
            });

            await _context.SaveChangesAsync();
            return Ok(res.Entity.Id);
        }

        [HttpPost("{courseid}/chapters/{chapterId}/lessons")]
        public async Task<IActionResult> CreateLesson(int courseId, int chapterId, [FromBody] CreateLessonRequest createLessonRequest)
        {
            if(createLessonRequest == null)
                return BadRequest("Request body not specified.");

            var errors = createLessonRequest.Validate();
            if (errors.Any())
                return BadRequest(errors);
            
            var course = await _context.Courses.Include(c => c.Chapters).FirstOrDefaultAsync(x => x.Id == courseId);
            if(course == null)
                return NotFound($"Unknown course [{courseId}].");
            
            var chapter = course.Chapters.Find(x => x.Id == chapterId);
            if(chapter == null)
                return NotFound($"Unknown chapter [{chapterId}] in course [{courseId}].");
            
            var res = await _context.AddAsync(new Lesson
            {
                Name = createLessonRequest.Name,
                ChapterId = chapter.Id,
                Chapter = chapter,
                CreatedAt = DateTime.Now
            });

            await _context.SaveChangesAsync();
            return Ok(res.Entity.Id);
        }
    }

    public class CourseResponse 
    {
        public int Id { get; set; }
        public string Name {get; set; }
        public IEnumerable<ChapterResponse> Chapters { get; set; }

        public static CourseResponse Create(Course course)
        {
            return new CourseResponse
            {
                Name = course.Name,
                Id = course.Id,
                Chapters = course.Chapters.Select(ChapterResponse.Create)
            };
        }
    }

    public class ChapterResponse 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<LessonResponse> Lessons { get; set; }

        public static ChapterResponse Create(Chapter chapter)
        {
            return new ChapterResponse
            {
                Name = chapter.Name,
                Id = chapter.Id,
                Lessons = chapter.Lessons.Select(LessonResponse.Create)
            };
        }
    }

    public class LessonResponse 
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static LessonResponse Create(Lesson lesson)
        {
            return new LessonResponse
            {
                Id = lesson.Id,
                Name = lesson.Name
            };
        }
    }

    public class CreateCourseRequest
    {
        public string Name { get; set; }

        public IEnumerable<string> Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                yield return $"{nameof(Name)} not specified";
        }
    }

    public class CreateLessonRequest
    {
        public string Name { get; set; }

        public IEnumerable<string> Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                yield return $"{nameof(Name)} not specified";
        }
    }
    public class CreateChapterRequest
    {
        public string Name { get; set; }
        public IEnumerable<string> Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                yield return $"{nameof(Name)} not specified";
        }
    }
}
