using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CoursesApi.Models;
using System;
using CoursesApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoursesApi.Controllers
{
    [Route("users")]
    public class UsersController : Controller
    {
        readonly CoursesDbContext _context;
        readonly AchievementService _achievementService;

        public UsersController(CoursesDbContext context, AchievementService achievementService)
        {
            _achievementService = achievementService;
            _context = context;
        }

        [HttpPost]
        public IActionResult Create()
        {
            throw new NotSupportedException("User creation not implemented yet.");
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(int userId = 1)
        {
            var user = await _context.Users
                 .Include(x => x.LessonCompletions)
                 .FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                return NotFound($"User with Id [{userId}] could not be found.");
            
            var userResponse = UserResponse.Create(user);
            return Ok(userResponse);
        }

        [HttpPost("{userId}/lessons")]
        public async Task<IActionResult> CompleteLesson(int userId, [FromBody] CreateLessonCompletionRequest createLessonCompletionRequest)
        {
            if (createLessonCompletionRequest == null)
                return BadRequest("Request body is missing.");
            
            var errors = createLessonCompletionRequest.Validate();
            if (errors.Any())
                return BadRequest(errors);
            
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound($"Unknown user [{userId}].");
            
            var lesson = await _context.Lessons.FindAsync(createLessonCompletionRequest.LessonId);
            if(lesson == null)
                return NotFound($"Unknown lesson [{createLessonCompletionRequest.LessonId}].");
            
            var lessonCompletion = new LessonCompletion
            {
                Lesson = lesson,
                LessonId = lesson.Id,
                User = user,
                UserId = user.Id,
                LessonStartedAt = createLessonCompletionRequest.LessonStartedAt,
                LessonCompletedAt = createLessonCompletionRequest.LessonCompletedAt
            };

            var res = await _context.LessonCompletions.AddAsync(lessonCompletion);
            await _context.SaveChangesAsync();

            await _achievementService.UpdateAchievementsOfUserAsync(userId);

            return Ok(res.Entity.Id);
        }

        [HttpGet("{userId}/achievements")]
        public async Task<IActionResult> GetAllAchievements(int userId)
        {
            await _achievementService.UpdateAchievementsOfUserAsync(userId);

            var user = await _context.Users
               .Include(x => x.AchievementProgresses)
               .FirstOrDefaultAsync(x => x.Id == userId);

            if(user == null)
                return NotFound($"Unknown user [{userId}].");
            
            var userAchievements = user.AchievementProgresses?.ToList() ?? new List<AchievementProgress>();
            var allAchievements = await _context.Achievements.ToListAsync();

            var response = allAchievements.Select(ach =>
            {
               var userAchievementProgress = userAchievements.Find(x => x.AchievementId == ach.Id);
               return new AchievementResponse
               {
                   Id = ach.Id,
                   IsCompleted = userAchievementProgress?.IsCompleted ?? false,
                   Progress = userAchievementProgress?.Progress ?? 0,
                   Name = ach.Name
               };
            });

            return Ok(response);
        }
    }

    public class CreateLessonCompletionRequest
    {
        public int LessonId { get; set; }
        public DateTime LessonStartedAt { get; set; }
        public DateTime LessonCompletedAt { get; set; }

        public IEnumerable<string> Validate() 
        {
            if (LessonId == 0)
                yield return $"{nameof(LessonId)} not specified/cannot be 0.";
            
            if(LessonStartedAt == DateTime.MinValue)
                yield return $"{nameof(LessonStartedAt)} not specified.";
            
            if (LessonCompletedAt == DateTime.MinValue)
                yield return $"{nameof(LessonCompletedAt)} not specified.";
        }
    }

    public class AchievementResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        public int Progress { get; set; }
    }

    public class UserResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public IEnumerable<LessonCompletionResponse> LessonCompletions { get; set; } = new List<LessonCompletionResponse>();

        public static UserResponse Create(User user) 
        {
            return new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                LessonCompletions = user.LessonCompletions.Select(LessonCompletionResponse.Create)
            };
        }
    }

    public class LessonCompletionResponse 
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public DateTime CompletedAt { get; set; }

        public static LessonCompletionResponse Create(LessonCompletion lessonCompletion)
        {
            return new LessonCompletionResponse
            {
                Id = lessonCompletion.Id,
                LessonId = lessonCompletion.LessonId,
                CompletedAt = lessonCompletion.LessonCompletedAt
            };
        }
    }
}
