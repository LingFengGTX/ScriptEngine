using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptEngine
{
    class File
    {
        
        public static void WriteInConfigure(string[] Value)
        {
            InSystem.Win_API.WritePrivateProfileString(Value[0], Value[1], Value[2], Value[3]);
        }

        public static bool ReadInConfigure(string[] Value)
        {
            StringBuilder tempBuffer = new StringBuilder();
            InSystem.Win_API.GetPrivateProfileString(Value[0],Value[1],"null",tempBuffer,255,Value[3]);
            if (tempBuffer.ToString() == "null")
            {
                return false;
            }
            else if (tempBuffer.ToString() == Value[2])
            {
                return true;
            }
            else {
                return false;
            }
        }
        public static void mkLink(string Value,string[] Values) {
            if (Values.Count() == 2) {
                mkLinkLaunch(Values[0], Values[1]);
            }
        }
        
        //使用mklink创建符号链接 -此函数需要提升应用程序的管理员权限。
        public static void mkLinkLaunch(string sourceFile, string targetLink)
        {
            if (System.IO.File.Exists(targetLink))
            {
                InSystem.CmdCommand("cmd.exe", "/c del \"" + targetLink + "\"", true);
            }
            InSystem.CmdCommand("cmd.exe", " /c mklink \"" + targetLink + "\" \"" + sourceFile + "\"", true);//设置blk文件

        }

    }
}
