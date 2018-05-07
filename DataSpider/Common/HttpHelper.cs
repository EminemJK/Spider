using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System;
using System.Net;
using System.IO;
using System.Text;

namespace DataSpider.Common
{
    public static class HttpHelper
    {
        private static HttpClient instance = null;
        public static HttpClient GetClient()
        {
            if (instance == null)
                instance = new HttpClient();
            return instance;
        }

        /// <summary>
        /// Get Method
        /// </summary>
        public static async Task<string> Get(string url)
        {
            try
            {
                var client = GetClient();
                var responseMsg = await client.GetAsync(url);
                if (responseMsg.IsSuccessStatusCode)
                { 
                    return await responseMsg.Content.ReadAsStringAsync();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch
            {
                instance = new HttpClient();
                return string.Empty;
            }
        }
        
        /// <summary>
        /// Post Method
        /// </summary>
        public static async Task<string> Post(string url, dynamic para, string ContentType = "application/json")
        {
            try
            {
                if (para != null)
                {
                    var requestJson = JsonConvert.SerializeObject(para);
                    HttpContent httpContent = new StringContent(requestJson);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue(ContentType);

                    var client = GetClient(); 
                    return await client.PostAsync(url, httpContent).Result.Content.ReadAsStringAsync();
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                instance = new HttpClient();
                return string.Empty;
            }
        }

        public static string PostData(string url, dynamic para, string contentType = "application/x-www-form-urlencoded")
        {
            try
            {
                //HttpClient 提交不来，改用HttpWebRequest来提交form表单数据
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url); 
                byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(para);
                req.Method = "POST";
                req.ContentType = contentType;
                req.ContentLength = requestBytes.Length;
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.19 Safari/537.36 ";
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.UTF8);
                string backstr = sr.ReadToEnd();
                sr.Close();
                res.Close();
                return backstr;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
