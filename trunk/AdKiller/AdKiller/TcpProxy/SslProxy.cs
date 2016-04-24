using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using System.Collections;
using System.IO;
using System.Threading;
using System.Diagnostics;
namespace AdKiller
{

    class SslProxy : IDisposable
    {
        Socket clientSocket, ipSocket;
        string header = string.Empty;//请求头。
        Uri hostUri = null;
        IPAddress[] connectIP = null;
        string errorMsg = string.Empty;
        bool isOutWallUrl = false;
        public SslProxy(Socket client, string headStr, Uri uri)
        {
            clientSocket = client;
            header = headStr;
            hostUri = uri;
            isOutWallUrl = UrlOutWall.Contains(hostUri);
        }

        public void Run()
        {
            //if (!SayOk())
            //{
            //    Close();
            //    return;
            //}
            if (!Connect())
            {
                Close();
                return;
            }
            if (!Send())
            {
                Close();
                return;
            }
            Thread.Sleep(10);
            Receive();//包括了Close();

        }
        bool SayOk()
        {
            byte[] data = Encoding.ASCII.GetBytes("HTTP/1.0 200\r\n\r\n");// Connection established\r\nProxy-agent: WebProxy 1.0
            try
            {
                clientSocket.Send(data, data.Length, 0);//发送回去，表明建立链接。
                return true;
            }
            catch
            {

            }
            return false;

        }
        bool Connect()
        {
            ipSocket = SocketPool.CreateSocketWithoutConnect(hostUri, clientSocket.LocalEndPoint as IPEndPoint);
            try
            {
                if (Config.NextProxy != null)
                {
                    ipSocket.Connect(Config.NextProxy);
                }
                else if (Config.OutWall && isOutWallUrl)
                {
                    ipSocket.Connect(Config.DefaultProxy);
                }
                else
                {
                    connectIP = Tool.GetHostIP(hostUri.Host);
                    if (connectIP == null)
                    {
                        return false;
                    }
                    ipSocket.Connect(connectIP, hostUri.Port);
                }
                return true;
            }
            catch (SocketException err)
            {
                errorMsg = "Ssl:Connect() : " + hostUri.OriginalString + "\r\n" + err.Message;
                Close();
                return false;
            }
        }
        bool Send()
        {
            if (Config.NextProxy != null || Config.RunAtType == 1 || (Config.OutWall && isOutWallUrl))
            {
                byte[] data = Encoding.ASCII.GetBytes(header);
                if (Config.RunAtType == 1 || (Config.OutWall && isOutWallUrl))
                {
                    data = DESCrypt.Crypt(data, true);
                }
                ipSocket.Send(data);
                if (!Wait())
                {
                    return false;
                }
                data = new byte[ipSocket.Available];
                ipSocket.Receive(data, 0, data.Length, SocketFlags.None);
            }
            return SayOk();
        }
        bool Wait()
        {

            int timeOut = 200;
            while (ipSocket.Available == 0)
            {
                timeOut--;
                if (timeOut < 1)
                {
                    errorMsg = "Wait() : time out " + hostUri.OriginalString;
                    return false;
                }
                Thread.Sleep(10);
            }
            return true;
        }
        void Receive()
        {
            //循环发送客户端请求,接收服务器返回
            try
            {
                int timeoutIndex = 50;
                byte[] data;
                while (true)
                {
                    if (!clientSocket.Connected && !ipSocket.Connected)
                    {
                        break;
                    }
                    while (clientSocket.Available != 0)//发送给服务端。
                    {
                        data = new byte[clientSocket.Available];
                        clientSocket.Receive(data, data.Length, 0);
                        ipSocket.Send(data, data.Length, 0);
                    }
                    Thread.Sleep(10);
                    while (ipSocket.Available != 0)
                    {
                        data = new byte[ipSocket.Available];
                        ipSocket.Receive(data, data.Length, 0);
                        clientSocket.Send(data, data.Length, 0);
                    }
                    if (clientSocket.Available == 0 && ipSocket.Available == 0)
                    {
                        timeoutIndex--;
                        if (timeoutIndex < 0 && hostUri.HostNameType == UriHostNameType.Dns)
                        {
                            break;
                        }
                        Thread.Sleep(200);
                    }
                    else
                    {
                        timeoutIndex = 50;
                    }
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);

            }
            finally
            {
                Close();
            }
        }
        void Close()
        {
            if (errorMsg != string.Empty)
            {
                DebugLog.WriteError(new Exception(errorMsg));
            }
            if (ipSocket != null)
            {
                try
                {
                    if (ipSocket.Connected)
                    {
                        ipSocket.Shutdown(SocketShutdown.Both);
                        ipSocket.Disconnect(false);
                    }

                }
                catch
                {
                }
                ipSocket.Close(1);
            }
            if (clientSocket != null)
            {
                try
                {
                    if (clientSocket.Connected)
                    {
                        clientSocket.Shutdown(SocketShutdown.Both);
                        clientSocket.Disconnect(false);
                    }
                }
                catch
                {
                }
                clientSocket.Close(1);
            }
        }

        #region IDisposable 成员

        public void Dispose()
        {
            clientSocket = ipSocket = null;
            header = null;
            hostUri = null;
            connectIP = null;
            errorMsg = null;
        }

        #endregion
    }
}
