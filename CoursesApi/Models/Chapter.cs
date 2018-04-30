using System;
using System.Collections.Generic;

namespace CoursesApi.Models
{
    public class Chapter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Lesson> Lessons { get; set; } = new List<Lesson>();
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }

        public long CourseId { get; set; }
        public Course Course { get; set; }
    }
}
