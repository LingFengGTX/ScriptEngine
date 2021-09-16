using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ScriptEngine
{
    public class InSystem
    {
        public class Win_API {
            //获取文本类型的INI配置参数
            [DllImport("kernel32.dll")]
            public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

            //写入INI配置参数
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);
        }

        //该函数并未设置延迟，请确保使用的函数是短暂的。
        public static void CmdCommand(string Application, string Command, bool CanWait)
        {
            Process cmdProcess = new Process();
            cmdProcess.StartInfo.FileName = Application;
            cmdProcess.StartInfo.Arguments = Command;
            cmdProcess.StartInfo.CreateNoWindow = true;
            cmdProcess.StartInfo.UseShellExecute = false;
            cmdProcess.Start();
            if (!CanWait)
            {
                return;
            }
            //如果布尔值设置为真则进行无线循环等待。
            while (!cmdProcess.HasExited)
            {
                continue;
            }

        }
    }
}
