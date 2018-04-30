using System;

namespace CoursesApi.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }

        public long ChapterId { get; set; }
        public Chapter Chapter { get; set; }
    }
}
