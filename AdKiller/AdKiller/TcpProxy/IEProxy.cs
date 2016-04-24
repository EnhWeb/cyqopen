namespace AdKiller
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Text;

    internal class IEProxy
    {
        [DllImport("rasapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern uint RasEnumEntries(IntPtr reserved, IntPtr lpszPhonebook, [In, Out] RASENTRYNAME[] lprasentryname, ref int lpcb, ref int lpcEntries);

        [DllImport("wininet.dll", EntryPoint = "InternetSetOption", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern bool InternetSetOptionList(IntPtr hInternet, int Option, ref ProxyOptionList OptionList, int size);
        /// <summary>
        /// 设置或取消IE代理（参数Proxy为空时则消代理）
        /// </summary>
        /// <param name="proxy"> 格式如：“127.0.0.1:81”</param>
        internal static void SetProxy(string proxy)
        {
            foreach (string name in GetConnectionNames())
            {
                Set(proxy, name);
            }
        }
        private static void Set(string proxy, string connectionName)
        {
            if (!string.IsNullOrEmpty(proxy))
            {
                proxy = string.Format("http={0};https={0};", proxy);
            }
            try
            {
                ProxyOptionList oplist = new ProxyOptionList();
                ProxyOption[] optionArray = new ProxyOption[2];
                oplist.Connection = connectionName;
                oplist.OptionCount = optionArray.Length;
                oplist.OptionError = 0;
                optionArray[0] = new ProxyOption();
                optionArray[0].dwOption = 1;
                optionArray[0].Value.dwValue = string.IsNullOrEmpty(proxy) ? 1 : 3;
                optionArray[1] = new ProxyOption();
                optionArray[1].dwOption = 2;
                optionArray[1].Value.pszValue = Marshal.StringToHGlobalAnsi(proxy);
                //optionArray[2] = new ProxyOption();
                //optionArray[2].dwOption = 3;
                //optionArray[2].Value.pszValue = Marshal.StringToHGlobalAnsi("<-loopback>");
                //optionArray[3] = new ProxyOption();
                //optionArray[3].dwOption = 4;
                //optionArray[3].Value.pszValue = Marshal.StringToHGlobalAnsi("");
                //optionArray[4] = new ProxyOption();
                //optionArray[4].dwOption = 5;
                //optionArray[4].Value.dwValue = 0;

                int cb = 0;
                for (int i = 0; i < optionArray.Length; i++)
                {
                    cb += Marshal.SizeOf(optionArray[i]);
                }
                IntPtr ptr = Marshal.AllocCoTaskMem(cb);
                IntPtr ptr2 = ptr;
                for (int j = 0; j < optionArray.Length; j++)
                {
                    Marshal.StructureToPtr(optionArray[j], ptr2, false);
                    ptr2 = (IntPtr)(((long)ptr2) + Marshal.SizeOf(optionArray[j]));
                }
                oplist.pOptions = ptr;
                oplist.Size = Marshal.SizeOf(oplist);
                int size = oplist.Size;
                bool flag = InternetSetOptionList(IntPtr.Zero, 0x4b, ref oplist, size);
                //if (flag)
                //{
                //     InternetSetOption(IntPtr.Zero, 0x5f, IntPtr.Zero, 0);
                //}
                //Marshal.FreeHGlobal(optionArray[0].Value.pszValue);
                //Marshal.FreeHGlobal(optionArray[1].Value.pszValue);
                //Marshal.FreeHGlobal(optionArray[2].Value.pszValue);
                //Marshal.FreeCoTaskMem(ptr);
            }
            catch
            {

            }
        }

        internal static string[] GetConnectionNames()
        {
            int lpcb = Marshal.SizeOf(typeof(RASENTRYNAME));
            int lpcEntries = 0;
            RASENTRYNAME[] lprasentryname = new RASENTRYNAME[1];
            lprasentryname[0].dwSize = lpcb;
            uint num3 = RasEnumEntries(IntPtr.Zero, IntPtr.Zero, lprasentryname, ref lpcb, ref lpcEntries);
            if ((num3 != 0) && (0x25b != num3))
            {
                lpcEntries = 0;
            }
            string[] strArray = new string[lpcEntries + 1];
            strArray[0] = null;
            if (lpcEntries != 0)
            {
                lprasentryname = new RASENTRYNAME[lpcEntries];
                for (int i = 0; i < lpcEntries; i++)
                {
                    lprasentryname[i].dwSize = Marshal.SizeOf(typeof(RASENTRYNAME));
                }
                if (RasEnumEntries(IntPtr.Zero, IntPtr.Zero, lprasentryname, ref lpcb, ref lpcEntries) != 0)
                {
                    return strArray;
                }
                for (int j = 0; j < lpcEntries; j++)
                {
                    strArray[j + 1] = lprasentryname[j].szEntryName;
                }
            }
            return strArray;
        }

        [StructLayout(LayoutKind.Sequential)]
        private class ProxyOption
        {
            public int dwOption;
            public IEProxy.OptionUnion Value;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct ProxyOptionList
        {
            public int Size;
            public string Connection;
            public int OptionCount;
            public int OptionError;
            public IntPtr pOptions;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct OptionUnion
        {
            [FieldOffset(0)]
            public int dwValue;
            [FieldOffset(0)]
            public System.Runtime.InteropServices.ComTypes.FILETIME ftValue;
            [FieldOffset(0)]
            public IntPtr pszValue;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct RASENTRYNAME
        {
            public int dwSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x101)]
            public string szEntryName;
            public int dwFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x105)]
            public string szPhonebook;
        }
    }
}

