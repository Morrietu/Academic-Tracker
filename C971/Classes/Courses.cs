using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace C971.Classes
{
    public class Courses
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int termId { get; set; }
        public string title { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string activity { get; set; }
        public string instructorName { get; set; }
        public string instructorPhone { get; set; }
        public string instructorEmail { get; set; }
        public string notes { get; set; }

        public Courses()
        {

        }
    }
}
