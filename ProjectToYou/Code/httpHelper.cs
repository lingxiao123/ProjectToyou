using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.IO;

namespace ProjectToYou
{
    public class HttpHelper
    {
        public static string GetHttpContentType(string ext)
        {
            string cType = "image/jpeg";
            switch (ext.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    cType = "image/jpeg";
                    break;
                case ".png":
                    cType = "image/png";
                    break;
                case ".bmp":
                    cType = "image/bmp";
                    break;
                case ".gif":
                    cType = "image/gif";
                    break;
                default:
                    cType = "application/octet-stream";
                    break;
            }
            return cType;
        }

        /// <summary>
        /// 模拟http post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string CreateHttpPostResponse(string url, IDictionary<string, string> parameters)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            HttpWebRequest request = null;
            //如果是发送HTTPS请求   
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                //request = WebRequest.Create(url) as HttpWebRequest;
                //request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";

            request.Headers.Add("X_REG_CODE", "288a633ccc1");
            request.Headers.Add("X_MACHINE_ID", "a306b7c51254cfc5e22c7ac0702cdf87");
            request.Headers.Add("X_REG_SECRET", "de308301cf381bd4a37a184854035475d4c64946");
            request.Headers.Add("X_STORE", "0001");
            request.Headers.Add("X_BAY", "0001-01");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("Accept-Language", "zh-CN");
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Accept = "*/*";

            request.CookieContainer = new CookieContainer();

            //如果需要POST数据   
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }

                //Encoding encoding = Encoding.GetEncoding("gb2312");
                Encoding encoding = Encoding.GetEncoding("utf-8");
                byte[] data = encoding.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            HttpWebResponse res;
            try
            {
                res = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse)ex.Response;
            }
            Stream s = res.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            //读取服务器端返回的消息 
            string sReturnString = sr.ReadLine();
            return sReturnString;
        }


        public static string CreateHttpGetResponse(string url, IDictionary<string, string> parameters)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            HttpWebRequest request = null;

            StringBuilder buffer = new StringBuilder();
            if (!(parameters == null || parameters.Count == 0))
            {
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("?{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
            }
            //如果是发送HTTPS请求   
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                //request = WebRequest.Create(url) as HttpWebRequest;
                //request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url + buffer.ToString()) as HttpWebRequest;
            }
            request.Method = "GET";

            request.Headers.Add("X_REG_CODE", "288a633ccc1");
            request.Headers.Add("X_MACHINE_ID", "a306b7c51254cfc5e22c7ac0702cdf87");
            request.Headers.Add("X_REG_SECRET", "de308301cf381bd4a37a184854035475d4c64946");
            request.Headers.Add("X_STORE", "0001");
            request.Headers.Add("X_BAY", "0001-01");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("Accept-Language", "zh-CN");
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Accept = "*/*";

            request.CookieContainer = new CookieContainer();


            HttpWebResponse res;
            try
            {
                res = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse)ex.Response;
            }
            Stream s = res.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            //读取服务器端返回的消息 
            string sReturnString = sr.ReadLine();
            return sReturnString;
        }

        public static string CreateHttpUploadFile(string url, Stream postedStream, string fileName, string cType, Dictionary<string, string> formDataDic)
        {
            if (string.IsNullOrEmpty(url))
                throw new Exception("Upload Web URL Is Empty.");

            String sReturnString = "";
            //时间戳 
            string strBoundary = "----" + DateTime.Now.Ticks.ToString("x");
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + strBoundary + "\r\n");

            //请求头部信息 
            StringBuilder sb = new StringBuilder();
            sb.Append("--");
            sb.Append(strBoundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"");
            sb.Append("file");
            sb.Append("\"; filename=\"");
            sb.Append(fileName);
            sb.Append("\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: ");
            sb.Append(cType);
            sb.Append("\r\n");
            sb.Append("\r\n");
            string strPostHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(strPostHeader);

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

            //Our method is post, otherwise the buffer (postvars) would be useless
            webRequest.Method = WebRequestMethods.Http.Post;

            // Proxy setting
            WebProxy proxy = new WebProxy();
            proxy.UseDefaultCredentials = true;
            webRequest.Proxy = proxy;

            //We use form contentType, for the postvars.
            webRequest.ContentType = "multipart/form-data;boundary=" + strBoundary; ;
            webRequest.ContentLength = postedStream.Length + postHeaderBytes.Length + boundaryBytes.Length;
            webRequest.Timeout = 300000;
            if (formDataDic != null)
            {
                foreach (string key in formDataDic.Keys)
                {
                    webRequest.Headers.Add(key, formDataDic[key]);
                }
            }

            byte[] fileByte = new byte[postedStream.Length];
            postedStream.Read(fileByte, 0, fileByte.Length);
            using (Stream requestStream = webRequest.GetRequestStream())
            {
                requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                requestStream.Write(fileByte, 0, fileByte.Length);
                requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                requestStream.Close();
            }
            try
            {
                WebResponse resp = webRequest.GetResponse();
                Stream s = resp.GetResponseStream();
                StreamReader sr = new StreamReader(s);

                //读取服务器端返回的消息 
                sReturnString = sr.ReadLine();
            }
            catch (WebException ex)
            {
                HttpWebResponse response = ex.Response as HttpWebResponse;
                if (response != null)
                {
                    throw new WebException(response.StatusDescription, ex);
                }

                throw ex;
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return sReturnString;
        }
    }
}
