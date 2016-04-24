using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace AdKiller
{
    public enum AsyConnectResult
    {
        None,
        Ok,
        Timeout,
        Error
    }
    /// <summary>
    /// by 路过秋天
    /// http://www.cnblogs.com/cyq1162
    /// </summary>s
    class HttpProxy : IDisposable
    {
        Socket clientSocket, ipSocket;

        List<byte> sourceHeaderBytes = null;//所有的数据，包括（POST时长数据)
        byte[] sendBytes = null;//存储中转请求发送的数据 
        FileType fileType;//分析是否文件请求
        string header = string.Empty;//请求头。
        Uri hostUri;
        //IPAddress[] connectIP = null;

        string css = string.Empty, script = string.Empty, errorMsg = string.Empty;
        HostEntity dnsHost = null;
        bool isOutWallUrl = false;
        static bool isAllowTodouSleep = false;
        IntPtr ipSocketHandle = IntPtr.Zero;
        int connectNum = 0;
        public HttpProxy(Socket socket, string headStr, List<byte> headData, FileType fType, Uri headUri)
        {
            connectNum = fType == FileType.Text ? MyRandom.Rnd.Next(2, 5) : 1;
            clientSocket = socket;
            header = headStr;
            sourceHeaderBytes = headData;
            fileType = fType;
            hostUri = headUri;
            isOutWallUrl = UrlOutWall.Contains(headUri);
        }

        public void Run()
        {
            try
            {
                if (!FormatHeader())//格式化请求头
                {
                    Close("FormatHeader fail:" + hostUri.OriginalString);
                    return;
                }
                if (!Connect())
                {
                    Close("Connect fail:" + hostUri.OriginalString);
                    return;
                }
                if (!Send())
                {
                    Close("Send fail:" + hostUri.OriginalString);
                    return;
                }
                Wait(5000);
                Receive();
                Close();
            }
            catch (Exception err)
            {
                DebugLog.WriteError(err);
            }
        }
        /// <summary>
        /// 格式化请求头
        /// </summary>
        /// <returns></returns>
        bool FormatHeader()
        {
            try
            {
                string host = hostUri.Host;
                if (WebHost.DomainList.ContainsKey(host) && WebHost.DomainList[host].IsEnabled)
                {
                    dnsHost = WebHost.DomainList[host];
                }
                if (dnsHost != null && header.StartsWith("GET")) //&& host != WebHost.domainIqiyi
                {
                    if (dnsHost.DomainType == DomainType.Video && dnsHost.IsReturnNow)//对于直接屏蔽的的视频网址，直接结束。
                    {
                        clientSocket.Send(Encoding.ASCII.GetBytes(ST.SocketStopResult));
                        return false;
                    }
                    #region 处理内部映射域名
                    switch (host)
                    {
                        case WebHost.domainIqiyi:
                            switch (hostUri.LocalPath)
                            {
                                case "/crossdomain.xml":
                                    clientSocket.Send(Encoding.ASCII.GetBytes(ST.IqiyiXmlResult));
                                    return false;
                            }
                            break;
                        case WebHost.domainTudou:
                            switch (hostUri.LocalPath)
                            {
                                case "/crossdomain.xml":
                                    isAllowTodouSleep = true;
                                    clientSocket.Send(Encoding.ASCII.GetBytes(ST.YoukuXmlResult));
                                    return false;
                            }
                            break;
                        case WebHost.domainYouku:
                            //修改为手机访问：

                            switch (hostUri.LocalPath)
                            {
                                case "/crossdomain.xml":
                                    clientSocket.Send(Encoding.ASCII.GetBytes(ST.YoukuXmlResult));
                                    return false;
                                case "/vf":
                                case "/valf":
                                    //clientSocket.Send(Encoding.ASCII.GetBytes(ST.YoukuVipResult));
                                    //return false;
                                    break;
                                default:
                                    clientSocket.Send(Encoding.ASCII.GetBytes(ST.SocketStopResult));
                                    return false;
                            }
                            break;
                        default:
                            if (host == WebHost.domainBaidu && hostUri.LocalPath == "/proxy.html")
                            {
                                clientSocket.Send(Encoding.ASCII.GetBytes(ST.SocketBaiDuProxyResult));
                                return false;
                            }
                            if (fileType == FileType.Text)
                            {
                                int index = header.IndexOf("Accept-Encoding: gzip");
                                if (index > -1)
                                {
                                    header = header.Replace("HTTP/1.1", "HTTP/1.0");
                                    header = header.Remove(index, header.IndexOf('\r', index) - index + 2);
                                }

                            }
                            break;

                    }

                    header = header.Replace("Proxy-Connection:", "Connection:");
                    //connectIP = Tool.GetHostIP(hostUri.Host);
                    if (Config.NextProxy == null && (!Config.OutWall || !isOutWallUrl))
                    {
                        header = header.Replace("GET http://" + host, "GET ");
                    }
                   
                    sendBytes = Encoding.ASCII.GetBytes(header);
                   
                    #endregion
                }
                else
                {
                    if (Config.NextProxy == null && (!Config.OutWall || !isOutWallUrl))
                    {
                        #region 直接代理请求的处理

                        string proxy = "Proxy-Connection";
                        int removeStart = header.IndexOf(proxy);
                        if (removeStart > 0)
                        {
                            sourceHeaderBytes.RemoveRange(removeStart, 6);
                            header = header.Remove(removeStart, 6);
                        }
                        string key = "http://" + hostUri.Host;
                        if (header.Contains(key))
                        {
                            int len = key.Length;
                            if (header.StartsWith("GET"))
                            {
                                removeStart = 4;
                            }
                            else if (header.StartsWith("POST"))
                            {
                                removeStart = 5;
                            }
                            else
                            {
                                removeStart = header.Split(' ')[0].Length + 1;
                            }
                            key = key + ":" + hostUri.Port;
                            if (header.Contains(key))
                            {
                                len = key.Length;
                            }
                            sourceHeaderBytes.RemoveRange(removeStart, len);
                            header = header.Remove(removeStart, len);
                        }

                        #endregion
                    }
                    if (Config.WriteLog && (host.IndexOf("weibo.cn") > -1 || host.IndexOf("g.sina.com.cn") > -1))
                    {
                        connectNum = 1;
                        #region 微博手机特殊处理
                        string uacpu = "UA-CPU:";

                        int removeStart = header.IndexOf(uacpu);
                        if (removeStart > 0)
                        {
                            int removeEnd = header.IndexOf('\r', removeStart);
                            sourceHeaderBytes.RemoveRange(removeStart, removeEnd - removeStart + 2);
                            header = header.Remove(removeStart, removeEnd - removeStart + 2);
                        }

                        string agent = "User-Agent:";
                        removeStart = header.IndexOf(agent);
                        if (removeStart > 0)
                        {
                            int removeEnd = header.IndexOf('\r', removeStart);
                            sourceHeaderBytes.RemoveRange(removeStart, removeEnd - removeStart);
                            header = header.Remove(removeStart, removeEnd - removeStart);
                            string att = "UA-OS: Windows CE (Pocket PC) - Version 4.21\r\nUA-color: color16\r\nUA-pixels: 240x320\r\nUA-CPU: ARM ARM920\r\nUA-Voice: TRUE\r\n";
                            att += "User-Agent: Mozilla/4.0 (compatible; MSIE 4.01; Windows CE; PPC; 240x320;T; " + DateTime.Now.ToString("hhmmss") + ")";
                            sourceHeaderBytes.InsertRange(removeStart, Encoding.ASCII.GetBytes(att));
                            header = header.Insert(removeStart, att);
                        }
                        #endregion
                    }
                    sendBytes = sourceHeaderBytes.ToArray();
                    

                }
                //if (hostUri.Host.EndsWith("youku.com"))
                //{
                //    string agent = "User-Agent:";
                //    int removeStart = header.IndexOf(agent);
                //    if (removeStart > 0)
                //    {
                //        int removeEnd = header.IndexOf('\r', removeStart);
                //        sourceHeaderBytes.RemoveRange(removeStart, removeEnd - removeStart);
                //        header = header.Remove(removeStart, removeEnd - removeStart);
                //        string att = "User-Agent:Mozilla/5.0 (compatible; MSIE 10.0; Windows Phone 8.0; Trident/6.0; IEMobile/10.0; ARM; Touch; NOKIA; 909)\r\nUA-CPU:ARM\r\nDNT:1";
                //        sourceHeaderBytes.InsertRange(removeStart, Encoding.ASCII.GetBytes(att));
                //        header = header.Insert(removeStart, att);
                //        sendBytes = sourceHeaderBytes.ToArray();
                //    }
                //}
                return true;
            }
            catch (Exception err)
            {
                errorMsg = "FormatHeader():" + err.Message + header;
                return false;
            }
        }

        bool Connect()
        {
            try
            {
                IPEndPoint p = clientSocket.LocalEndPoint as IPEndPoint;
                if (Config.NextProxy != null)
                {
                    ipSocket = SocketPool.CreateSocketWithoutConnect(hostUri, p);
                    ipSocket.Connect(Config.NextProxy);
                    return true;
                }
                else if (Config.OutWall && isOutWallUrl)
                {
                    ipSocket = SocketPool.CreateSocketWithoutConnect(hostUri, p);
                    ipSocket.Connect(Config.DefaultProxy);
                    return true;
                }
                else
                {
                    ipSocket = SocketPool.CreateSocketWithConnect(hostUri, p, connectNum);
                    return ipSocket != null && ipSocket.Connected;
                }
            }
            catch (SocketException err)
            {
                errorMsg = "Connect() : " + hostUri.OriginalString + "\r\n" + err.Message;

            }
            return false;
        }

        bool Send()
        {
            try
            {
                if ((Config.RunAtType == 1 && Config.NextProxy != null) || (Config.OutWall && isOutWallUrl))
                {
                    sendBytes = DESCrypt.Crypt(sendBytes, true);
                }
                //#if DEBUG
                //                int len = sendBytes.Length / 1024;
                //                if (header.StartsWith("POST") || len > 2)
                //                {
                //                    System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString() + " " + hostUri.OriginalString + ":" + sendBytes.Length + "(" + len + " kb)");
                //                }
                //#endif
                ipSocket.Send(sendBytes, sendBytes.Length, 0);
                return true;
            }
            catch (Exception err)
            {
                errorMsg = "Send() : " + hostUri.OriginalString + "\r\n" + err.Message;
                return false;
            }
        }
        void Wait(int timeOut)
        {
            while (ipSocket.Available == 0)
            {
                timeOut--;
                if (timeOut < 1)
                {
                    break;
                }
                Thread.Sleep(10);
            }
        }

        void Receive()
        {

            int length = 0;//, count = 0;

            try
            {
                int waitCount = 50;
                Encoding encode = Encoding.ASCII;//文本的编码
                string text = string.Empty;
                bool isTextChanged = false, isEnd = false;
                int index = -1, available = -1;

                int bodyLen = -1;//文件的长度。
                int headerLen = 0;//http头长度
                int receiveLen = 0;//已接收的长度。
                while (waitCount > 0 || ipSocket.Available > 0)
                {
                    available = ipSocket.Available;
                    if (available > 0)
                    {
                        waitCount = 50;
                        #region 有数据

                        isTextChanged = false;
                        byte[] recvBytes = new byte[available];
                        length = ipSocket.Receive(recvBytes, 0, available, SocketFlags.Partial);
                        if ((Config.NextProxy != null && Config.RunAtType == 1) || (Config.OutWall && isOutWallUrl))//客户端，先解密，再返回给浏览器。
                        {
                            recvBytes = DESCrypt.Crypt(recvBytes, false);//解密返给浏览器。
                            length = recvBytes.Length;
                        }
                        receiveLen += length;//接收的长度。
                        if (bodyLen == -1 && receiveLen == length)//分析Http头的总长度。
                        {
                            text = Encoding.ASCII.GetString(recvBytes, 0, recvBytes.Length > 1024 ? 1024 : recvBytes.Length);
                            if (int.TryParse(Tool.GetContentLength(text), out bodyLen))
                            {
                                headerLen = text.IndexOf("\r\n\r\n") + 3;
                                receiveLen = receiveLen - headerLen;//接收的总长要除去请求头。
                            }
                        }
                        #region 域名映射处理。
                        if (fileType == FileType.Text && hostUri.LocalPath.Length > 1 && (dnsHost != null || (hostUri.Host.EndsWith(".youku.com") && hostUri.Host.Length > 11)))//
                        {
                            if (encode == Encoding.ASCII)
                            {
                                encode = Tool.GetEncoding(text);
                            }
                            try
                            {
                                #region 格式化Html

                                string host = hostUri.Host;
                                switch (host)
                                {
                                    case WebHost.domainIqiyi:
                                        if (text.StartsWith("HTTP/1.1 302"))
                                        {
                                            return;
                                        }
                                        break;
                                    case WebHost.domainYouku:
                                        if (hostUri.LocalPath == "/valf" || hostUri.LocalPath == "/vf")
                                        {
                                            if (bodyLen < 1 || bodyLen > 500)
                                            {
                                                Thread.Sleep(10000);
                                            }
                                            //移除Cookies
                                            text = Tool.RemoveCookies(text);
                                            isTextChanged = true;
                                        }
                                        break;
                                    case WebHost.domainTudou:
                                        // System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString() + " : " + bodyLen + " : " + hostUri.OriginalString);
                                        if (isAllowTodouSleep && hostUri.LocalPath == "/tdcm/adcontrol")
                                        {
                                            isAllowTodouSleep = false;
                                            Thread.Sleep(5000);
                                        }
                                        break;
                                    default:
                                        if (hostUri.Host.EndsWith(".youku.com") && hostUri.Host.Length > 11)
                                        {
                                            text = Tool.RemoveCookies(text);
                                            isTextChanged = true;
                                        }
                                        #region 处理样式
                                        else if (dnsHost.IsEnabled && dnsHost.DomainType == DomainType.Search)
                                        {
                                            text = encode.GetString(recvBytes, 0, recvBytes.Length);
                                            if (text.StartsWith("HTTP/1."))
                                            {
                                                css = AdFormat.GetCss(hostUri.Host);
                                                if (bodyLen > 0)
                                                {
                                                    text = text.Replace("Content-Length: " + bodyLen, "Content-Length: " + (bodyLen + css.Length));
                                                    isTextChanged = true;
                                                }
                                            }
                                            index = text.IndexOf("</title>");
                                            if (index > -1 && css.Length > 0)
                                            {
                                                text = text.Insert(index + 8, css);
                                                isTextChanged = true;
                                            }
                                            index = text.LastIndexOf("</body>");
                                            if (index > 500)//说明结束了。
                                            {
                                                if (text.IndexOf("</html>") > -1)
                                                {
                                                    Thread.Sleep(200);
                                                    isEnd = ipSocket.Available == 0;
                                                }
                                            }
                                            if (isTextChanged)
                                            {
                                                recvBytes = encode.GetBytes(text);
                                            }
                                        }

                                        #endregion
                                        break;
                                }

                                #endregion
                            }
                            catch// (Exception err)
                            {
                                // DebugLog.WriteError(err);
                            }
                        }
                        #endregion
                        try
                        {
                            if (Config.NextProxy == null && Config.RunAtType == 2)
                            {
                                byte[] temp = DESCrypt.Crypt(recvBytes, true);
                                clientSocket.Send(temp, 0, temp.Length, SocketFlags.Partial);//加密传给客户端
                                temp = null;
                            }
                            else
                            {
                                clientSocket.Send(recvBytes, 0, recvBytes.Length, SocketFlags.Partial);
                            }
                            if (bodyLen == -1) // chunked
                            {
                                text = Encoding.ASCII.GetString(recvBytes);
                                if (text.IndexOf("\0\r\n") > -1)
                                {
                                    Thread.Sleep(200);
                                    isEnd = ipSocket.Available == 0;
                                }
                            }
                            recvBytes = null;
                            text = null;
                        }
                        catch
                        {
                            //errorMsg = "clientSocket.Send() : send be close : " + hostUri.OriginalString;
                            break;
                        }
                        #endregion
                    }
                    else
                    {

                        if (isEnd || bodyLen == 0 || (bodyLen > 0 && receiveLen >= bodyLen))
                        {
                            if (waitCount > 3)
                            {
                                waitCount = 3;
                            }
                            //break;
                        }
                        waitCount--;
                        if (bodyLen < 0)
                        {
                            if (!ipSocket.Connected)
                            {
                                break;
                            }
                            //if (waitCount > 25)//bodyLen == -2 && 
                            //{
                            //    waitCount = 15;
                            //}
                        }
                        Thread.Sleep(200);//关键点,请求太快数据接收不全(由于有业务逻辑，从原来200减到100左右）
                    }
                }
            }
            catch //(Exception err)
            {
                //DebugLog.WriteError(err);
            }
        }
        void Close(string msg)
        {
#if DEBUG
            try
            {
                clientSocket.Send(Encoding.ASCII.GetBytes(ST.GetHttpResult(324, msg)));
            }
            catch
            {

            }
#endif

            Close();
        }
        void Close()
        {
            if (errorMsg != string.Empty)
            {
                DebugLog.WriteError(new Exception(errorMsg));
            }
            if (ipSocket != null)
            {
                ipSocket.Close(1);
                ipSocket = null;
            }
            if (clientSocket != null)
            {
                clientSocket.Close(1);
                clientSocket = null;
            }
        }


        #region IDisposable 成员

        public void Dispose()
        {
            clientSocket = null;
            ipSocket = null;
            sourceHeaderBytes = null;//所有的数据，包括（POST时长数据)
            sendBytes = null;//存储中转请求发送的数据 
            header = null;
            hostUri = null;
            css = script = errorMsg = null;
            dnsHost = null;

        }

        #endregion
    }

}
