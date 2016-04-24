using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Web;

namespace AdKiller
{
    public class SocketPool
    {
        /// <summary>
        /// 清除未使用的Socket
        /// </summary>
        public static void Clear()
        {
            try
            {
                Socket s = null;
                if (qsList.Count > 0)
                {
                    foreach (Queue<Socket> qs in qsList)
                    {
                        while (qs.Count > 0)
                        {
                            s = qs.Dequeue();
                            try
                            {
                                s.Close();
                                s = null;
                            }
                            catch
                            {

                            }
                        }
                    }
                    qsList.Clear();

                }
                while (qSocket.Count > 0)
                {
                    s = qSocket.Dequeue();
                    try
                    {
                        s.Close();
                        s = null;
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception err)
            {
                DebugLog.WriteError(err);
            }
        }
        public class MyStruct
        {
            public Uri hostUri;
            public Queue<Socket> qs;
            public Socket ipSocket;
            public MyStruct(Uri hostUri, Queue<Socket> qs, Socket ipSocket)
            {
                this.hostUri = hostUri;
                this.qs = qs;
                this.ipSocket = ipSocket;
            }
        }
        static readonly object lockObj = new object();
        static List<Queue<Socket>> qsList = new List<Queue<Socket>>(128);
        public static Socket CreateSocketWithConnect(Uri hostUri, IPEndPoint p, int connectNum)
        {
            IPAddress[] ips = Tool.GetHostIP(hostUri.Host);
            if (ips == null)
            {
                return null;
            }
            Socket ipSocket = null;
            try
            {
                ipSocket = CreateSocketWithoutConnect(hostUri, p);
                IAsyncResult result = ipSocket.BeginConnect(ips, hostUri.Port, null, null);
                ipSocket.EndConnect(result);
                return ipSocket;
            }
            catch (Exception err)
            {
                ipSocket.Close();
                ipSocket = null;
                return null;
            }
           

            //try
            //{
            //    Socket socket = null;
            //    Queue<Socket> qS = null;
            //    object o = HttpRuntime.Cache.Get(hostUri.Host + hostUri.Port);
            //    if (o == null)
            //    {
            //        qS = new Queue<Socket>();
            //        if (!qsList.Contains(qS))
            //        {
            //            qsList.Add(qS);
            //        }
            //        HttpRuntime.Cache.Add(hostUri.Host + hostUri.Port, qS, null, DateTime.Now.AddSeconds(45), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Low, null);
            //    }
            //    else
            //    {
            //        qS = o as Queue<Socket>;
            //    }

            //    if (qS.Count > 0)
            //    {
            //        try
            //        {
            //            lock (lockObj)
            //            {
            //                if (qS.Count > 0)
            //                {
            //                    socket = qS.Dequeue();
            //                }
            //            }
            //        }
            //        catch
            //        {

            //        }
            //    }
            //    else
            //    {
            //        IPAddress[] ips = Tool.GetHostIP(hostUri.Host);
            //        if (ips == null)
            //        {
            //            return null;
            //        }
            //        for (int i = 0; i < connectNum; i++)
            //        {
            //            Socket ipSocket = CreateSocketWithoutConnect(hostUri, p);
            //            ipSocket.BeginConnect(ips, hostUri.Port, new AsyncCallback(CallAsyConnect), new MyStruct(hostUri, qS, ipSocket));
            //            Thread.Sleep(5);
            //        }
            //        //异步产生10个链接；
            //        int wait = 500;
            //        while (true)//等待队列里有链接产生。
            //        {
            //            if (qS.Count > 0)
            //            {
            //                Thread.Sleep(10);
            //                lock (lockObj)
            //                {
            //                    if (qS.Count > 0)
            //                    {
            //                        socket = qS.Dequeue();
            //                    }
            //                }
            //                break;
            //            }
            //            else
            //            {
            //                wait--;
            //                if (wait < 0)
            //                {
            //                    break;
            //                }
            //                Thread.Sleep(10);
            //            }
            //        }
            //    }
            //    return socket;
            //}
            //catch (Exception err)
            //{
            //    DebugLog.WriteError(err);
            //    return null;
            //}
        }
        private static void CallAsyConnect(IAsyncResult result)
        {
            MyStruct myObj = null;
            try
            {
                while (!result.IsCompleted)
                {
                    Thread.Sleep(10); //等待完成。
                }
                myObj = result.AsyncState as MyStruct;

                myObj.ipSocket.EndConnect(result);
                myObj.qs.Enqueue(myObj.ipSocket);

            }
            catch
            {
                myObj.ipSocket.Close();
            }
        }
        public static Socket CreateSocketWithoutConnect(Uri hostUri, IPEndPoint p)
        {
            Socket ipSocket = Create(hostUri.HostNameType);
            #region 绑定公网
            try
            {
                if (IsPrivateNet(p))
                {
                    ipSocket.Bind(new IPEndPoint(IPAddress.Any, 0));
                }
                else
                {
                    ipSocket.Bind(new IPEndPoint(p.Address, 0));
                }
            }
            catch
            {
                try
                {
                    ipSocket = Create(hostUri.HostNameType);
                    if (IsPrivateNet(p))
                    {
                        ipSocket.Bind(new IPEndPoint(IPAddress.Any, 0));
                    }
                    else
                    {
                        ipSocket.Bind(new IPEndPoint(p.Address, 0));
                    }
                }
                catch (Exception err)
                {
                    DebugLog.WriteError(err);
                    ipSocket = Create(hostUri.HostNameType);
                }
            }
            #endregion
            return ipSocket;
        }
        /// <summary>
        /// 是否私有网络（局域网IP） 
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsPrivateNet(IPEndPoint ip)
        {
            try
            {
                string address = ip.Address.ToString();
                string[] items = address.Split('.');
                int first = int.Parse(items[0]);
                int second = int.Parse(items[1]);
                switch (first)
                {
                    case 0:
                    case 10:
                    case 127:
                        return true;
                    case 172:
                        return second > 15 && second < 32;
                    case 192:
                        return second == 168;
                    default:
                        if (first > 223)
                        {
                            return true;
                        }
                        break;
                }
            }
            catch (Exception err)
            {
                DebugLog.WriteError(err);
            }
            return false;
        }
        #region 创建Socket，但不绑定指定链接


        static Queue<Socket> qSocket = new Queue<Socket>(100);
        /// <summary>
        /// 创建一个Socket返回
        /// </summary>
        /// <returns></returns>
        static Socket Create(UriHostNameType type)
        {
            try
            {

                if (type == UriHostNameType.Dns)
                {
                    Socket ipSocket = null;
                    if (qSocket.Count > 0)
                    {
                        ipSocket = qSocket.Dequeue();
                    }
                    if (qSocket.Count < 150)
                    {
                        lock (lockCreateObj)
                        {
                            if (qSocket.Count < 100)
                            {
                                Thread thread = new Thread(new ThreadStart(CreateSocketPool), 512);
                                thread.IsBackground = true;
                                thread.Start();
                            }
                        }
                    }
                    if (ipSocket == null)
                    {
                        ipSocket = Create(128);
                    }
                    return ipSocket;
                }
                else
                {
                    return Create(512);

                }
            }
            catch (Exception err)
            {
                DebugLog.WriteError(err);
                return null;
            }
        }
        static object lockCreateObj = new object();
        static void CreateSocketPool()
        {
            try
            {
                if (qSocket.Count < 30)
                {
                    for (int i = 0; i < 120; i++)
                    {
                        if (qSocket.Count < 150)
                        {
                            qSocket.Enqueue(Create(128));
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                DebugLog.WriteError(err);
            }
        }
        static Socket Create(int size)
        {
            try
            {
                Socket ipSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ipSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);//不允许重复端口，避免多线程请求出问题。
                ipSocket.SendTimeout = 20000;
                ipSocket.ReceiveTimeout = 20000;
                ipSocket.ReceiveBufferSize = 1024 * size;
                ipSocket.SendBufferSize = 1024 * size;
                return ipSocket;
            }
            catch (Exception err)
            {
                DebugLog.WriteError(err);
                return null;
            }
        }

        #endregion
    }
}
