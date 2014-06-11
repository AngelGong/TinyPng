using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace TinyPng
{
    /// <summary>
    /// 在不需要修改key和url的情况下，次类是无状态的，也是线程安全的
    /// </summary>
    public class CompressPng
    {
        private static readonly CompressPng _instance = new CompressPng();
        public static CompressPng getInstance()
        {
            return _instance;
        }
        private string _key = "23Rs_FdQDVib7P-5Pa5wUw3dNoaOw8EW";
        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }
        private string _urlStr = "https://api.tinypng.com/shrink";
        public string UrlStr
        {
            get
            {
                return _urlStr;
            }
            set
            {
                _urlStr = value;
            }
        }

        public WebClient getWebClient()
        {
            string auth = Convert.ToBase64String(Encoding.UTF8.GetBytes("api:" + Key));
            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.Authorization, "Basic " + auth);
            return client;
        }

        public void tinyPng(string input,string output)
        {
            WebClient client = this.getWebClient();
            try
            {
                client.UploadData(UrlStr, File.ReadAllBytes(input));
                /* Compression was successful, retrieve output from Location header. */
                client.DownloadFile(client.ResponseHeaders["Location"], output);
                Console.WriteLine("Success:" + output);
                File.Delete(input);
            }
            catch (WebException e)
            {
                /* Something went wrong! You can parse the JSON body for details. */
                Console.WriteLine("Failed{0}:{1}", e.Message + e.TargetSite, input);
            }
        }
    
    }
}
