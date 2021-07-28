using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace C971.Classes
{
    public class Terms
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string title { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

        public Terms()
        {

        }
    }
}
