using System;

namespace udemyApp.API.Models
{
    public class Errorlog
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string Stacktrace { get; set; }
        public DateTime Errortime { get; set; }
    }
}