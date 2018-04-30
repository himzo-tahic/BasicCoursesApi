using System;
using System.Collections.Generic;

namespace CoursesApi.Models
{
    public class Course
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public List<Chapter> Chapters { get; set; } = new List<Chapter>();
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
    }
}
