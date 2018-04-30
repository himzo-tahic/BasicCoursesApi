using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CoursesApi.Models;

namespace CoursesApi.Infrastructure
{
    public class AchievementService
    {
        readonly CoursesDbContext _context;
        readonly AchievementValidator _achievementValidator;

        public AchievementService(CoursesDbContext context, AchievementValidator achievementValidator)
        {
            _achievementValidator = achievementValidator;
            _context = context;
        }

        public async Task UpdateAchievementsOfUserAsync(int userId) 
        {
            var user = await _context.Users
               .Include(us => us.LessonCompletions)
               .Include(us => us.AchievementProgresses)
               .FirstAsync(us => us.Id == userId);
            
            var achievementValidationResults = _achievementValidator.Execute(user.LessonCompletions);

            var userAchievements = user.AchievementProgresses?.ToList() ?? new List<AchievementProgress>();
            var allAchievements = await _context.Achievements.ToListAsync();

            foreach(var ach in allAchievements)
            {
                var userAchievementProgress = userAchievements.Find(x => x.AchievementId == ach.Id);
                var achievementValidation = achievementValidationResults.Find(x => x.achievementId == ach.Id);

                if (achievementValidation.validation == null)
                    throw new NotImplementedException($"Missing validation for Achievement [{ach.Id}]");

                if (userAchievementProgress == null)
                    await _context.AchievementProgresses.AddAsync(new AchievementProgress
                    {
                        Achievement = ach,
                        AchievementId = ach.Id,
                        Goal = achievementValidation.validation.Goal,
                        Progress = achievementValidation.validation.Progress,
                        User = user,
                        UserId = userId
                    });
                else
                {
                    userAchievementProgress.Goal = achievementValidation.validation.Goal;
                    userAchievementProgress.Progress = achievementValidation.validation.Progress;
                    _context.Update(userAchievementProgress);
                }
            };

            await _context.SaveChangesAsync();
        }
    }
}
