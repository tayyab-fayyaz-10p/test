using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;﻿

namespace SSH.Common.Helper
{
    public class HttpRequestHelper
    {
        public static async Task<HttpResponseMessage> Execute(string method, string url, object jsonObject, string token = null)
        {
            using (var client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                }

                HttpResponseMessage result = null;
                var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
                if (method == "POST")
                {
                    result = await client.PostAsync(url, content);
                }
                else if (method == "PUT")
                {
                    result = await client.PutAsync(url, content);
                }
                else if (method == "GET")
                {
                    result = await client.GetAsync(url);
                }

                return result;
            }
        }
    }
}
