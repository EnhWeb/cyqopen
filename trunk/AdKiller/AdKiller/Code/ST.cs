using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace AdKiller
{
    /// <summary>
    /// 静态或常量（Static or Const)
    /// </summary>
    static class ST
    {
        /// <summary>
        /// 本机IP
        /// </summary>
        internal const string LocalIP = "127.0.0.1";
        /// <summary>
        /// 消息提示的标题
        /// </summary>
        internal const string MsgTitle = "消息提示";
        /// <summary>
        /// 默认返回的200标识
        /// </summary>
        public const string SocketStopResult = "HTTP/1.1 201 OK\r\nContent-Type: text/html\r\nConnection: close\r\nContent-Length: 0\r\n\r\n";

        public const string SocketBaiDuProxyResult = "HTTP/1.1 200 OK\r\nContent-Type: text/html\r\nConnection: close\r\nContent-Length: 99\r\n\r\n<script>if(parent && parent.parent){parent.parent.document.title=location.hash.substr(1);}</script>";

        public static string GetHttpResult(int httpCode,string html)
        {
            return "HTTP/1.1 "+httpCode+" Found\r\nConnection: close\r\nContent-Length: " + html.Length + "\r\n\r\n" + html;
        }

        public static string Get302Result(string url)
        {
            string html = string.Format("<html><head><title>Object moved</title></head><body><h2>Object moved to <a href=\"{0}\">here</a>.</h2></body></html>", url);
            return string.Format("HTTP/1.1 302 Found\r\nLocation: {0}\r\nConnection: close\r\nContent-Length: " + html.Length + "\r\n\r\n" + html, url);
        }

        public static string IqiyiXmlResult
        {
            get
            {
                return string.Format("HTTP/1.1 200 OK\r\nServer: nginx/\r\nDate: {0}\r\nContent-Type: text/xml\r\nContent-Length: 227\r\nLast-Modified: Mon, 30 May 2011 06:31:00 GMT\r\nConnection: keep-alive\r\nAccept-Ranges: bytes\r\n\r\n<?xml version=\"1.0\"?>\r\n\t\t<cross-domain-policy> <site-control permitted-cross-domain-policies=\"all\" />\r\n    <allow-access-from domain=\"*\" /> \r\n    <allow-http-request-headers-from domain=\"*\" headers=\"*\"/>\r\n</cross-domain-policy>", DateTime.UtcNow.ToUniversalTime().ToString("r"));
            }
        }
        public static string YoukuXmlResult
        {
            get
            {
                return string.Format("HTTP/1.1 200 OK\r\nServer: nginx/0.5.33/\r\nDate: {0}\r\nContent-Type: text/xml\r\nContent-Length: 112\r\nConnection: close\r\nAccept-Ranges: bytes\r\n\r\n<?xml version=\"1.0\" encoding=\"UTF-8\"?><cross-domain-policy><allow-access-from domain=\"*\"/></cross-domain-policy>", DateTime.UtcNow.ToUniversalTime().ToString("r"));
            }
        }
//        public static string YoukuVipResult
//        {
//            get
//            {
//                return @"HTTP/1.1 200 OK
//Server: adserver
//Cache-Control: private
//Pragma: no-cache
//Cache-Control: private
//Expires: 0
//Content-Type: text/html; charset=UTF-8
//Content-Length: 99
//Connection: close"+"\r\nDate: "+ DateTime.UtcNow.ToUniversalTime().ToString("r")+"\r\n\r\n{\"N\":\"b56924b028b49417c286c129d091e96c\",\"T\":\"1372155187184\",\"M\":\"fefcad1958ae4b6048c18e10387bf27a\"}";
//            }
//        }
    }
}
