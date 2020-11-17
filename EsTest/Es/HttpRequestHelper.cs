using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EsTest.Es
{
    public static class HttpRequestHelper
    {
        #region Private Method
        public static StreamReader HttpPostStream(string url, string jsonData, Dictionary<string, string> headers = null)
        {
            Stream requestWriter = null;
            StreamReader responseReader = null;
            WebResponse webResponse = null;
          
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;

            try
            {
                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";
                webRequest.Timeout = 180000;
                webRequest.ReadWriteTimeout = 180000;
                webRequest.Headers.Set("Pragma", "no-cache");
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        webRequest.Headers.Set(item.Key, item.Value);
                    }
                }
                byte[] bytes = Encoding.UTF8.GetBytes(jsonData);
                webRequest.ContentLength = bytes.Length;
                requestWriter = webRequest.GetRequestStream();
                requestWriter.Write(bytes, 0, bytes.Length);
                requestWriter.Close();
                requestWriter = null;
                webResponse = webRequest.GetResponse();
                responseReader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
                return responseReader;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            
        }
        /// <summary>
        /// 同步方式发起http post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static string HttpPost(string url, string jsonData, Dictionary<string, string> headers = null)
        {
            Stream requestWriter = null;
            StreamReader responseReader = null;
            WebResponse webResponse = null;
            string responseData = null;
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;

            try
            {
                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";
                webRequest.Timeout = 180000;
                webRequest.ReadWriteTimeout = 180000;
                webRequest.Headers.Set("Pragma", "no-cache");
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        webRequest.Headers.Set(item.Key, item.Value);
                    }
                }
                byte[] bytes = Encoding.UTF8.GetBytes(jsonData);
                webRequest.ContentLength = bytes.Length;
                requestWriter = webRequest.GetRequestStream();
                requestWriter.Write(bytes, 0, bytes.Length);
                requestWriter.Close();
                requestWriter = null;
                webResponse = webRequest.GetResponse();
                responseReader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
                responseData = responseReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                try
                {
                    if (requestWriter != null)
                    {
                        requestWriter.Close();
                        requestWriter = null;
                    }
                    if (responseReader != null)
                    {
                        responseReader.Close();
                        responseReader = null;
                    }
                    if (webResponse != null)
                    {
                        webResponse.GetResponseStream().Close();
                    }
                    webRequest = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return responseData;
        }

        /// <summary>
        /// 异步方式发起http post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="queryString"></param>
        /// <returns></returns>
        private static async Task<string> HttpPostAsync(string url, string jsonData, Dictionary<string, string> headers = null)
        {
            Stream requestWriter = null;
            StreamReader responseReader = null;
            WebResponse webResponse = null;
            string responseData = null;
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            try
            {
                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";
                webRequest.Timeout = 180000;
                webRequest.ReadWriteTimeout = 180000;
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        webRequest.Headers.Set(item.Key, item.Value);
                    }
                }
                webRequest.Headers.Set("Pragma", "no-cache");
                byte[] bytes = Encoding.UTF8.GetBytes(jsonData);
                webRequest.ContentLength = bytes.Length;
                requestWriter = await webRequest.GetRequestStreamAsync();
                requestWriter.Write(bytes, 0, bytes.Length);
                requestWriter.Close();
                requestWriter = null;
                webResponse = await webRequest.GetResponseAsync();
                responseReader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
                responseData = responseReader.ReadToEnd();
            }
            catch (WebException wex)
            {
                throw wex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                try
                {
                    if (requestWriter != null)
                    {
                        requestWriter.Close();
                        requestWriter = null;
                    }
                    if (responseReader != null)
                    {
                        responseReader.Close();
                        responseReader = null;
                    }
                    if (webResponse != null)
                    {
                        webResponse.GetResponseStream().Close();
                    }
                    webRequest = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return responseData;
        }

        /// <summary>
        /// 同步方式发起http get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="queryString"></param>
        /// <returns></returns>
        private static string HttpGet(string url, Dictionary<string, string> parm = null, Dictionary<string, string> headers = null)
        {
            StreamReader responseReader = null;
            WebResponse webResponse = null;
            string responseData = null;
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;

            try
            {
                webRequest.Method = "GET";
                webRequest.ContentType = "text/html;charset=UTF-8";
                webRequest.Timeout = 180000;
                webRequest.ReadWriteTimeout = 180000;
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        webRequest.Headers.Set(item.Key, item.Value);
                    }
                }
                webResponse = webRequest.GetResponse();
                responseReader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
                responseData = responseReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                try
                {
                    if (responseReader != null)
                    {
                        responseReader.Close();
                        responseReader = null;
                    }
                    if (webResponse != null)
                    {
                        webResponse.GetResponseStream().Close();
                    }
                    webRequest = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return responseData;
        }

        /// <summary>
        /// 异步方式发起http get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="queryString"></param>
        /// <returns></returns>
        private static async Task<string> HttpGetAsync(string url, Dictionary<string, string> parm = null, Dictionary<string, string> headers = null)
        {
            StreamReader responseReader = null;
            WebResponse webResponse = null;
            string responseData = null;
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;

            try
            {
                webRequest.Method = "GET";
                webRequest.ContentType = "text/html;charset=UTF-8";
                webRequest.Timeout = 180000;
                webRequest.ReadWriteTimeout = 180000;
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        webRequest.Headers.Set(item.Key, item.Value);
                    }
                }
                webResponse = await webRequest.GetResponseAsync();
                responseReader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
                responseData = responseReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                try
                {
                    if (responseReader != null)
                    {
                        responseReader.Close();
                        responseReader = null;
                    }
                    if (webResponse != null)
                    {
                        webResponse.GetResponseStream().Close();
                    }
                    webRequest = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return responseData;
        }

        #endregion

        #region public


       

        public static T Post<T>(string url, object postData = null, Dictionary<string, string> headers = null)
        {
            var postDataStr = postData == null ? string.Empty : JsonConvert.SerializeObject(postData);
            string result = HttpPost(url, postDataStr, headers);
            return JsonConvert.DeserializeObject<T>(result);
        }

        public static async Task<T> PostAsync<T>(string url, object postData = null, Dictionary<string, string> headers = null)
        {
            var postDataStr = postData == null ? string.Empty : JsonConvert.SerializeObject(postData);
            string result = await HttpPostAsync(url, postDataStr, headers);
            return JsonConvert.DeserializeObject<T>(result);
        }

        public static T PostJsonString<T>(string url, string postData = null, Dictionary<string, string> headers = null)
        {
            string result = HttpPost(url, postData, headers);
            return JsonConvert.DeserializeObject<T>(result);
        }

        public static async Task<T> PostJsonStringAsync<T>(string url, string postData = null, Dictionary<string, string> headers = null)
        {
            string result = await HttpPostAsync(url, postData, headers);
            return JsonConvert.DeserializeObject<T>(result);
        }

        public static T Get<T>(string url, Dictionary<string, string> getData = null, Dictionary<string, string> headers = null)
        {
            string result = HttpGet(url, getData, headers);
            return JsonConvert.DeserializeObject<T>(result);
        }

        public static async Task<T> GetAsync<T>(string url, Dictionary<string, string> getData = null, Dictionary<string, string> headers = null)
        {
            string result = await HttpGetAsync(url, getData, headers);
            return JsonConvert.DeserializeObject<T>(result);
        }

        #endregion
    }
}
