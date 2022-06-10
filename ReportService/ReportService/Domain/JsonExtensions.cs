using System;
using Newtonsoft.Json;

namespace ReportService.Domain
{
    public static class JsonExtensions
    {
        public static string ToJson(this object obj)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));
            
            return JsonConvert.SerializeObject(obj);
        }
    }
}