using System;

namespace udemyApp.API.Models
{
    public class Errorlog
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string Stacktrace { get; set; }
        public string Function { get; set; }
        public string Page { get; set; }
        public string User { get; set; }
        public DateTime Errortime { get; set; }
    }
}