using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Runtime.InteropServices;
namespace AdKiller
{
    /// <summary>
    /// by 路过秋天
    /// http://www.cnblogs.com/cyq1162
    /// </summary>
    class TcpListen
    {
        static TcpListener tcplistener = null;
        public static void Listen(int port)
        {
            ThreadPool.SetMaxThreads(100, 100);
            ThreadPool.SetMinThreads(50, 30);

            IPAddress ipp = null;
            if (tcplistener == null)
            {
                ipp = System.Net.IPAddress.Any;
                tcplistener = new TcpListener(ipp, port);
                tcplistener.ExclusiveAddressUse = true;
                tcplistener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, false);
                tcplistener.Server.SendTimeout = 30000;
                tcplistener.Server.ReceiveTimeout = 30000;
                tcplistener.Server.ReceiveBufferSize = 1024 * 128;
                tcplistener.Server.SendBufferSize = 1024 * 128;
            }
            tcplistener.Start();

            //侦听端口号 
            while (true)
            {
                try
                {
                    Socket socket = tcplistener.AcceptSocket();
                    ThreadPool.QueueUserWorkItem(new WaitCallback(RunSocket), socket);
                    Thread.Sleep(1);//传说ThreadPool有Bug，需要这一行来解决。
#if DEBUG
                    int a, b;
                    ThreadPool.GetAvailableThreads(out a, out b);
                    Debug.WriteLine(a + "," + b);
#endif
                }
                catch (Exception err)
                {
                    DebugLog.WriteError(err);
                    break;
                }
            }
        }
        public static void RunSocket(object s)
        {
            Socket socket = null;
            List<byte> data = null;//请求头数据。
            string header = null;
            Uri uri = null;
            try
            {
                socket = s as Socket;
                #region 基础错误检测

                //并获取传送和接收数据的Scoket实例 
                header = Tool.GetHeader(socket, out data);
                if (string.IsNullOrEmpty(header))
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    return;
                }
                uri = Tool.GetUri(header);

                if (uri == null || UrlFilter.Contains(uri))
                {
                    socket.Send(Encoding.ASCII.GetBytes(ST.SocketStopResult));
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    return;
                }

                if (uri.Host.Length - uri.Host.LastIndexOf('.') > 5)//域名的后缀最多4个。
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    return;
                }
                #endregion
                if (uri.Port == 443 || header.StartsWith("CONNECT"))
                {
                    using (SslProxy sslProxy = new SslProxy(socket, header, uri))
                    {
                        sslProxy.Run();//完成dispose
                    }
                }
                else
                {
                    FileType fileType = Tool.GetFileType(uri);
                    //switch (fileType)
                    //{
                    //    //case FileType.Css:
                    //    //case FileType.Js:
                    //    case FileType.Photo:
                    //    case FileType.File:
                    //    case FileType.Other:
                    //        Thread.Sleep(5);
                    //        break;
                    //}
#if DEBUG
                    System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString() + " : " + uri.ToString());
#endif
                    using (HttpProxy proxy = new HttpProxy(socket, header, data, fileType, uri))
                    {
                        proxy.Run();
                    }
                }
            }
            catch (Exception err)
            {
                DebugLog.WriteError(err);
            }
            finally
            {
                socket = null;
                data = null;//请求头数据。
                header = null;
                uri = null;
            }

        }
        public static void Stop()
        {
            try
            {
                if (tcplistener != null)
                {
                    tcplistener.Stop();
                    tcplistener = null;
                }
            }
            catch (Exception err)
            {
                DebugLog.WriteError(err);
            }
        }

    }
}
