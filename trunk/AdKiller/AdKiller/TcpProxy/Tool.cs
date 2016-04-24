using System.Net.Sockets;
using System.Threading;
using System;
using System.Text;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.IO.Compression;
using System.Net.NetworkInformation;
using System.Web;
using System.Collections;
namespace AdKiller
{
    static class Tool
    {

        /// <summary>
        /// 获取请求的头。
        /// </summary>
        public static string GetHeader(Socket socket, out List<byte> postBodyData)
        {
            postBodyData = new List<byte>();
            string header = string.Empty;
            try
            {
                int index = 50;
                while (index > 0)
                {
                    if (socket.Available > 0)
                    {
                        index = 50;
                        byte[] data = new byte[socket.Available];
                        socket.Receive(data, data.Length, 0);
                        if (Config.NextProxy == null && Config.RunAtType == 2)//服务端，需要解密
                        {
                            data = DESCrypt.Crypt(data, false);
                        }
                        if (postBodyData.Count == 0)
                        {
                            header = Encoding.ASCII.GetString(data);
                        }
                        postBodyData.AddRange(data);
                        if (header.StartsWith("POST"))
                        {
                            Thread.Sleep(50);
                        }
                        if (socket.Available == 0)
                        {
                            break;
                        }
                    }
                    Thread.Sleep(200);
                    index--;
                }
            }
            catch (Exception err)
            {
                DebugLog.WriteError(err);
            }
            //#if DEBUG
            //            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString() + " : " + header);
            //#endif
            return header;
        }

        public static Uri GetUri(string httpHeader)
        {
            int startIndex = httpHeader.IndexOf(' ');
            int endIndex = httpHeader.IndexOf(' ', startIndex + 1);
            if ((startIndex == -1) || (endIndex == -1))
            {
                return null;
            }
            string urlText = httpHeader.Substring(startIndex + 1, endIndex - startIndex).Trim();

            string url = string.Empty;
            if (!urlText.Contains("http://"))
            {
                if (urlText.Substring(0, 1) == "/")
                {
                    startIndex = httpHeader.IndexOf("host:", StringComparison.OrdinalIgnoreCase) + 6;
                    if (startIndex > 6)
                    {
                        endIndex = httpHeader.IndexOf('\r', startIndex);
                        urlText = httpHeader.Substring(startIndex, endIndex - startIndex).Trim();
                    }
                }

                urlText = "http" + (urlText.Contains(":443") ? "s" : "") + "://" + urlText;

            }

            Uri uri = null;
            try
            {
                if (urlText.IndexOf('.') > -1)
                {
                    uri = new Uri(urlText);
                }
            }
            catch
            {
                return null;
            }

            return uri;
        }
        public static FileType GetFileType(Uri uri)
        {
            try
            {
                string url = uri.PathAndQuery;
                if (url.Contains(".js"))
                {
                    return FileType.Js;
                }
                else if (url.Contains(".css"))
                {
                    return FileType.Css;
                }
                else if (url.Contains(".gif") || url.Contains(".jpg") || url.Contains(".jpeg") || url.Contains(".png") || url.Contains(".bmp") || url.Contains(".ico"))
                {
                    return FileType.Photo;
                }
                else if (url.Contains(".htm") || url.Contains(".shtm") || url.Contains(".xml") || url.Contains(".asp") || url.Contains(".php") || url.Contains(".jsp") || url.Contains(".json"))
                {
                    return FileType.Text;
                }
                else if (url.Contains(".zip") || url.Contains(".rar") || url.Contains(".7z") || url.Contains(".doc") || url.Contains(".mp3") || url.Contains(".flv") || url.Contains(".swf"))
                {
                    return FileType.File;
                }
                else
                {
                    if (uri.LocalPath.IndexOf('.') == -1 || url.IndexOf('.') == -1)
                    {
                        return FileType.Text;
                    }
                    return FileType.Other;
                }
            }
            catch
            {
                return FileType.Other;
            }
        }

        #region Host DNS 解析

        public static IPAddress[] GetHostIP(string host)
        {
            if (host == "localhost")
            {
                return new IPAddress[] { IPAddress.Loopback };
            }
            object objIps = HttpRuntime.Cache.Get(host);
            if (objIps != null)
            {
                return objIps as IPAddress[];
            }
            else if (HttpRuntime.Cache.Get("Error_" + host) != null || host.IndexOf('.') == -1)
            {
                return null;
            }
            try
            {
                IAsyncResult result = Dns.BeginGetHostAddresses(host, null, null);
                IPAddress[] ips = Dns.EndGetHostAddresses(result);
                if (host.Split('.').Length < 4)
                {
                    SetHostIP(host, ips);
                }
                return ips;
            }
            catch (Exception err)
            {
                if (HttpRuntime.Cache.Get("Error_" + host) == null)
                {
                    HttpRuntime.Cache.Insert("Error_" + host, string.Empty, null, DateTime.Now.AddSeconds(5), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Low, null);
                }
                DebugLog.WriteError(new Exception("GetDnsCache() : " + host + "\r\n" + err.Message));
            }
            return null;

        }

        /// <summary>
        /// 设置Dns（若IP为空则清除）
        /// </summary>
        public static void SetHostIP(string host, IPAddress[] ips)
        {
            try
            {
                object objIps = HttpRuntime.Cache.Get(host);
                if (objIps != null)
                {
                    if (ips == null)
                    {
                        HttpRuntime.Cache.Remove(host);
                    }
                    else
                    {
                        objIps = ips;
                    }
                }
                else
                {
                    HttpRuntime.Cache.Insert(host, ips, null, DateTime.Now.AddMinutes(3), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Low, null);
                }
            }
            catch (Exception err)
            {
                DebugLog.WriteError(err);
            }
        }
        public static void ClearHostIPCache()
        {
            List<string> cacheKeys = new List<string>();
            IDictionaryEnumerator cacheEnum = HttpRuntime.Cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                cacheKeys.Add(cacheEnum.Key.ToString());
            }
            foreach (string cacheKey in cacheKeys)
            {
                HttpRuntime.Cache.Remove(cacheKey);
            }
        }

        #endregion

        /// <summary>
        /// 获取请求头中的某属性的值
        /// </summary>
        public static string GetContentLength(string header)
        {
            if (!header.StartsWith("HTTP/1.1 20") && !header.StartsWith("HTTP/1.0 20"))
            {
                return "0";
            }

            string key = "Content-Length:";
            int start = header.IndexOf(key) + key.Length;
            if (start > key.Length)
            {
                int end = header.IndexOf('\r', start);
                return header.Substring(start, end - start).Trim();
            }
            if (header.IndexOf("Transfer-Encoding: chunked", StringComparison.OrdinalIgnoreCase) > -1)
            {
                //if (header.IndexOf(": gzip", StringComparison.OrdinalIgnoreCase) > -1)
                //{
                //    return "-1";
                //}
                //else //if (header.IndexOf("Connection: Keep-Alive", StringComparison.OrdinalIgnoreCase) > -1)
                //{
                //    return "0";
                //}
                return "-1";
            }
            if (header.Contains("Connection: close"))
            {
                return "0";
            }
            return "-2";
        }
        public static Encoding GetEncoding(string header)
        {
            if (header.IndexOf("charset=gb", StringComparison.OrdinalIgnoreCase) > -1)//gb2312 && gbk
            {
                return Encoding.GetEncoding("gb2312");
            }
            else if (header.IndexOf("charset=utf", StringComparison.OrdinalIgnoreCase) > -1)
            {
                return Encoding.UTF8;
            }
            else
            {
                return Encoding.GetEncoding("gb2312");
            }
        }
        internal static string RemoveCookies(string header)
        {
            header = RemoveHeader(header, "Set-Cookie:");
            return header;// RemoveHeader(header, "Cookie:");
        }
        private static string RemoveHeader(string header, string removeKey)
        {
            if (!string.IsNullOrEmpty(header))
            {
                try
                {
                    int start = -1;
                    do
                    {
                        start = header.IndexOf(removeKey, StringComparison.OrdinalIgnoreCase);
                        if (start > -1)
                        {
                            int end = header.IndexOf("\r\n", start);
                            if (end > start)//正常结束
                            {
                                end += 2;//延长到\r\n
                            }
                            else // if (end == -1 || end < start) 最后一个，没有结束符号
                            {
                                end = header.Length;
                            }
                            header = header.Remove(start, end - start);

                        }
                    }
                    while (start > 0);

                }
                catch (Exception e)
                {
                    DebugLog.WriteError(e);
                }
            }
            return header;
        }

        internal static bool TestConnect(IPEndPoint point, out string err)
        {
            err = string.Empty;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {

                socket.Connect(point);
                socket.Disconnect(false);
                return true;
            }
            catch (Exception e)
            {
                err = e.Message;
                return false;
            }
            finally
            {
                socket.Close();
            }
        }
        internal static bool TestConnect(string ip, int port, out string err)
        {
            return TestConnect(new IPEndPoint(IPAddress.Parse(ip), port), out err);
        }

        internal static string GetYouKuUrl(string url)
        {
            WebClient wc = new WebClient();
            wc.Proxy = null;
            string html = string.Empty;
            try
            {

                html = wc.DownloadString(url);

                if (!string.IsNullOrEmpty(html))
                {
                    //return html;
                    //return html.Replace("getFlvPath/sid", ST.LocalIP);
                    html = GetJosnValue(html, "ATMSU");
                }

            }
            finally
            {
                wc.Dispose();
            }
            return html;
        }
        private static string GetJosnValue(string json, string key)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(json))
            {
                key = key.TrimEnd('"') + "\"";
                int index = json.IndexOf(key);
                if (index > 0)
                {
                    index = json.IndexOf('"', index + key.Length + 1) + 1;
                    result = json.Substring(index, json.IndexOf('"', index) - index);
                }
            }
            return result;
        }

    }
    public enum FileType
    {
        Photo,
        Js,
        Css,
        File,
        Text,
        Other,
    }

}
