using System.Collections.Generic;

namespace CoursesApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public List<LessonCompletion> LessonCompletions { get; set; } = new List<LessonCompletion>();
        public List<AchievementProgress> AchievementProgresses { get; set; } = new List<AchievementProgress>();
    }
}
