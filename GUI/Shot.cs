using Cassandra;
using System;
using System.Collections.Generic;
using System.Text;

namespace GUI
{
    public class Shot
    {
        public LocalDate date { get; set; }
        public string name { get; set; }
        public bool obligatory { get; set; }
        public bool done { get; set; }
    }

}
