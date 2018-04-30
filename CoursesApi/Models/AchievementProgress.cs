using System;

namespace CoursesApi.Models
{
    public class AchievementProgress
    {
        public int Id { get; set; }
        public int Progress { get; set; }
        public int Goal { get; set; }

        public bool IsCompleted => Progress >= Goal;

        public User User { get; set; }
        public int UserId { get; set; }

        public Achievement Achievement { get; set; }
        public int AchievementId { get; set; }
    }
}
