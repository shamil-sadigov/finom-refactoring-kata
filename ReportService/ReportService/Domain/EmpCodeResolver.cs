using System.Net.Http;
using System.Threading.Tasks;

namespace ReportService.Domain
{
    public class EmpCodeResolver
    {
        // TODO: Есть предположение что на каждый inn будет всегда возвращаться один и тот же статичный Code 
        // поэтому стоит задуматься о кешировании
        public static async Task<string> GetCode(string inn)
        {
            // TODO: Создавать каждый раз новый HttpClient дорого, к тому же он разделяемый и потокобезопасный
            // стоит вынести в статичное поле
            var client = new HttpClient();
            return await client.GetStringAsync("http://buh.local/api/inn/" + inn);
        }
    }
}
