using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CYQ.VisualierSetup
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string runPath = AppDomain.CurrentDomain.BaseDirectory;
                List<string> cd = new List<string>();
                cd.Add("C:\\Program Files");
                cd.Add("D:\\Program Files");
                cd.Add("E:\\Program Files");
                cd.Add("F:\\Program Files");
                cd.Add("G:\\Program Files");
                cd.Add("H:\\Program Files");

                cd.Add("C:\\Program Files (x86)");
                cd.Add("D:\\Program Files (x86)");
                cd.Add("E:\\Program Files (x86)");
                cd.Add("F:\\Program Files (x86)");
                cd.Add("G:\\Program Files (x86)");
                cd.Add("H:\\Program Files (x86)");
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("2005", "\\Microsoft Visual Studio 8");
                dic.Add("2008", "\\Microsoft Visual Studio 9");
                dic.Add("2012", "\\Microsoft Visual Studio 11.0");
                dic.Add("2015", "\\Microsoft Visual Studio 14");
                //读取VS安装路径
                string vPath = "\\Common7\\Packages\\Debugger\\Visualizers";
                string mPath = "\\VC#\\Snippets\\2052\\Visual C#";
                foreach (string item in cd)
                {
                    foreach (KeyValuePair<string, string> kv in dic)
                    {
                        string vFolder = item + kv.Value + vPath;
                        if (Directory.Exists(vFolder))
                        {
                            string dll = runPath + kv.Key + "\\CYQ.Visualizer.dll";
                            if (File.Exists(dll))
                            {
                                File.Copy(dll, vFolder + "\\CYQ.Visualizer.dll", true);
                                Console.WriteLine("To：" + vFolder + "\\CYQ.Visualizer.dll");
                            }
                        }
                        string mFoler = item + kv.Value + mPath;
                        if (Directory.Exists(mFoler) && Directory.Exists(runPath + "\\snippet"))
                        {
                            string[] files = Directory.GetFiles(runPath + "\\snippet", "*.snippet");
                            foreach (string file in files)
                            {
                                File.Copy(file, mFoler + "\\" + Path.GetFileName(file), true);
                                Console.WriteLine("To：" + mFoler + "\\" + Path.GetFileName(file));
                            }
                        }
                    }
                }
                Console.WriteLine("Completed!");
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
            Console.Read();
        }
    }
}
