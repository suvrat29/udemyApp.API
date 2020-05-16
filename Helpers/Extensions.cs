using Microsoft.AspNetCore.Http;
using System;
using udemyApp.API.Data;
using udemyApp.API.Models;

namespace udemyApp.API.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static void AddToApplicationLog(this HttpResponse response, string message, string source, string trace, DataContext context)
        {
            var error = new Errorlog
            {
                Message = message,
                Source = source,
                Stacktrace = trace,
                Errortime = DateTime.Now
            };

            context.AddAsync(error);
            context.SaveChangesAsync();
        }

        public static void AddToApplicationLog(string message, string source, string trace, string function, string page, string user, DataContext context)
        {
            var error = new Errorlog
            {
                Message = message,
                Source = source,
                Stacktrace = trace,
                Function = function,
                Page = page,
                User = user,
                Errortime = DateTime.Now
            };

            context.AddAsync(error);
            context.SaveChangesAsync();
        }

        public static int CalculateAge(this DateTime theDateTime)
        {
            var age = DateTime.Today.Year - theDateTime.Year;

            if (theDateTime.AddYears(age) > DateTime.Today)
                age--;

            return age;
        }
    }
}