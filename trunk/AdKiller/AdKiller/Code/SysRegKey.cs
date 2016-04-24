using System;
using Microsoft.Win32;
using System.Windows.Forms;

namespace AdKiller
{
    /// <summary>
    /// 注册表操作
    /// </summary>
    class SysRegKey
    {
        /// <summary>
        /// 自动开机
        /// </summary>
        /// <param name="start"></param>
        public static void AutoStartComputer(bool start)
        {
            try
            {
                RegistryKey runKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (runKey != null)
                {
                    if (runKey.GetValue("noad") != null)
                    {
                        runKey.DeleteValue("noad");
                    }
                    string value = Convert.ToString(runKey.GetValue("adkiller"));
                    if (start)
                    {
                        if (value != Application.ExecutablePath)
                        {
                            runKey.SetValue("adkiller", Application.ExecutablePath);
                        }
                    }
                    else if (!string.IsNullOrEmpty(value))
                    {
                        runKey.DeleteValue("adkiller");
                    }
                    runKey.Close();
                }
            }
            catch (Exception err)
            {
                DebugLog.WriteError(err);
            }
        }
        ///// <summary>
        ///// 禁止IE缓存dns
        ///// </summary>
        ///// <param name="close"></param>
        //public static void CloseDnsCache(bool close)
        //{
        //    try
        //    {
        //        RegistryKey runKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings", true);
        //        if (runKey != null)
        //        {
        //            if (close)
        //            {
        //                runKey.SetValue("DnsCacheEnabled", 0, RegistryValueKind.DWord);
        //                runKey.SetValue("DnsCacheTimeout", 0, RegistryValueKind.DWord);
        //                runKey.SetValue("ServerInfoTimeOut", 0, RegistryValueKind.DWord);
        //            }
        //            else if (runKey.GetValue("DnsCacheEnabled") != null)
        //            {
        //                runKey.DeleteValue("DnsCacheEnabled");
        //                runKey.DeleteValue("DnsCacheTimeout");
        //                runKey.DeleteValue("ServerInfoTimeOut");
        //            }
        //            runKey.Close();
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        DebugLog.WriteError(err);
        //    }
        //}
        /// <summary>
        /// 是否已设置了代理
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static bool HasSetProxy()
        {
            bool result = false;
            try
            {
                RegistryKey proxyKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings", true);
                if (proxyKey != null)
                {
                    if (Convert.ToString(proxyKey.GetValue("ProxyEnable")) == "1")
                    {
                        result = true;
                    }
                    proxyKey.Close();
                }
            }
            catch (Exception err)
            {
                DebugLog.WriteError(err);
            }
            return result;
        }
        //public static void SetProxy(bool isEnabled, string proxyIP)
        //{
        //    try
        //    {
        //        RegistryKey setKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings", true);
        //        if (setKey != null)
        //        {
        //            if (isEnabled)
        //            {
        //                setKey.SetValue("ProxyEnable", 0x1);
        //                setKey.SetValue("ProxyServer", string.Format("http={0};https={0};", proxyIP));
        //            }
        //            else //关闭链接
        //            {
        //                setKey.SetValue("ProxyEnable", 0x0);
        //                setKey.DeleteValue("ProxyServer");
        //            }
        //            setKey.Close();
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        DebugLog.WriteError(err);
        //    }
        //}
    }
}
