using System;

namespace CoursesApi.Models
{
    public class LessonCompletion
    {
        public int Id { get; set; }
        public DateTime LessonStartedAt { get; set; }
        public DateTime LessonCompletedAt { get; set; }

        public Lesson Lesson { get; set; }
        public int LessonId { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }
    }
}
