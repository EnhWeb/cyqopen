using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace AdKiller
{

    public delegate void HotkeyEventHandler(int HotKeyID);
    /// <summary>
    /// 快捷键。
    /// </summary>
    class SystemHotKey : System.Windows.Forms.IMessageFilter
    {
        List<UInt32> keyIDs = new List<UInt32>();
        IntPtr hWnd;

        public event HotkeyEventHandler OnHotkey;

        public enum KeyFlags
        {
            Alt = 0x1,
            Ctrl = 0x2,
            Shift = 0x4,
            Win = 0x8,
            //组合键等于值相加
            Alt_Ctrl = 0x3,
            Alt_Shift = 0x5,
            Ctrl_Shift = 0x6,
            Alt_Ctrl_Shift = 0x7
        }
        [DllImport("user32.dll")]
        public static extern UInt32 RegisterHotKey(IntPtr hWnd, UInt32 id, UInt32 fsModifiers, UInt32 vk);

        [DllImport("user32.dll")]
        public static extern UInt32 UnregisterHotKey(IntPtr hWnd, UInt32 id);

        [DllImport("kernel32.dll")]
        public static extern UInt32 GlobalAddAtom(String lpString);

        [DllImport("kernel32.dll")]
        public static extern UInt32 GlobalDeleteAtom(UInt32 nAtom);

        public SystemHotKey(IntPtr hWnd)
        {
            this.hWnd = hWnd;
        }

        public int RegisterHotkey(KeyFlags keyflags, System.Windows.Forms.Keys Key)
        {
            System.Windows.Forms.Application.AddMessageFilter(this);
            UInt32 hotkeyid = GlobalAddAtom(System.Guid.NewGuid().ToString());
            RegisterHotKey((IntPtr)hWnd, hotkeyid, (UInt32)keyflags, (UInt32)Key);
            keyIDs.Add(hotkeyid);
            return (int)hotkeyid;
        }

        public void UnregisterHotkeys()
        {
            if (keyIDs.Count > 0)
            {

                System.Windows.Forms.Application.RemoveMessageFilter(this);
                foreach (UInt32 key in keyIDs)
                {
                    UnregisterHotKey(hWnd, key);
                    GlobalDeleteAtom(key);
                }
                keyIDs.Clear();
            }
        }

        public bool PreFilterMessage(ref   System.Windows.Forms.Message m)
        {
            if (m.Msg == 0x312)
            {
                if (OnHotkey != null)
                {
                    foreach (UInt32 key in keyIDs)
                    {
                        if ((UInt32)m.WParam == key)
                        {
                            OnHotkey((int)m.WParam);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }

}
