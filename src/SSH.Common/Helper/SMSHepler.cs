using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSH.Common.Helper
{
    public class SMSHelper
    {
        public static async Task DoWork(SMSConfiguration configuration)
        {
            var uri = string.Format("http://www.jawalbsms.ws/api.php/sendsms?user={0}&pass={1}&to={2}&message={3}&sender=SMSA-Test", configuration.UserName, configuration.Password, configuration.PhoneNumber, configuration.Message);
            var smsApi = await HttpRequestHelper.Execute("GET", uri, JSONHelper.ConvertObjectToJSON(new object()));
            var smsApiResult = await smsApi.Content.ReadAsStringAsync();
        }
    }

    public class SMSConfiguration
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string Message { get; set; }

        public int Sender { get; set; }
    }
}
