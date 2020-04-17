using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
