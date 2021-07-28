using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace C971.Classes
{
    public class Assessments
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int courseId { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public DateTime dueDate { get; set; }
        public bool notification { get; set; }

        public Assessments()
        {

        }
    }
}
