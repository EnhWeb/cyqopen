using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ClearSVN
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("是否清除与此执行程序相同目录下的所有svn或vss文件？1-svn；2-vss；0-两者,其它-退出");
            string answer = Console.ReadLine();
            string path = string.Empty;
            if (args != null && args.Length > 0)
            {
                path = args[0];
            }
            else
            {
                path = System.Environment.CurrentDirectory;
            }
            if (answer == "1" || answer == "0")
            {
                DoSvn(path);
            }
            if (answer == "2" || answer == "0")
            {
                DoVss(path);
            }
        }
        private static void DoSvn(string path)
        {
            string[] folders = System.IO.Directory.GetDirectories(path);
            for (int i = 0; i < folders.Length; i++)
            {
                if (folders[i].Substring(folders[i].Length-4) == ".svn")
                {
                    try
                    {
                        //DirectoryInfo dInfo = new DirectoryInfo(folders[i]);
                        //dInfo.Attributes = FileAttributes.Normal;
                        //dInfo.Delete(true);
                        Directory.Delete(folders[i], true);
                    }
                    catch
                    {
                        Console.WriteLine("请先去掉文件夹只读属性");
                        Console.Read();
                        return;
                    }
                    Console.WriteLine("删除:"+folders[i]);
                }
                else
                {
                    DoSvn(folders[i]);
                }
            }
        }
        private static void DoVss(string path)
        {
            string[] folders = System.IO.Directory.GetFileSystemEntries(path);

            for (int i = 0; i < folders.Length; i++)
            {
                if(Directory.Exists(folders[i]))
                {
                    DoVss(folders[i]);
                }
                else if (folders[i].Substring(folders[i].Length - 4) == ".scc" || folders[i].Substring(folders[i].Length - 7) == ".vspscc")
                {
                    try
                    {
                        File.Delete(folders[i]);
                    }
                    catch
                    {
                        Console.WriteLine("请先去掉文件夹只读属性");
                        Console.Read();
                        return;
                    }
                    Console.WriteLine("删除:" + folders[i]);
                }
            }
        }
    }
}
