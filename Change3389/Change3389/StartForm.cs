using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Change3389
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (txtCode.Text == "cyqdata.com")
            {
                int port;
                if (int.TryParse(txtPort.Text, out port) && port > 1000 && port < 50000)
                {
                    Change(port);
                }
            }
            else
            {
                MessageBox.Show("Code Error");
            }
        }
        public static void Change(int port)
        {
            try
            {
                RegistryKey runKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server\Wds\rdpwd\Tds\tcp", true);
                if (runKey != null)
                {
                    runKey.SetValue("PortNumber", port);
                    runKey.Close();
                }
                runKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\ControlSet001\Control\Terminal Server\WinStations\RDP-Tcp", true);//win7
                if (runKey != null)
                {
                    runKey.SetValue("PortNumber", port);
                    runKey.Close();
                }
                runKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentContro1Set\Control\Tenninal Server\WinStations\RDP\Tcp", true);//win2003
                if (runKey != null)
                {
                    runKey.SetValue("PortNumber", port);
                    runKey.Close();
                }
                MessageBox.Show("修改成功，重启电脑后生效。");
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
    }
}