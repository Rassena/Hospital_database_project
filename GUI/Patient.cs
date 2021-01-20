using System;
using System.Collections.Generic;
using System.Text;

namespace GUI
{
    class Patient
    {
        public long pesel { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string login { get; internal set; }
        public string password { get; internal set; }
    }
}
