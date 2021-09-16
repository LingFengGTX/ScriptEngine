using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;//正则表达式引用

namespace ScriptEngine
{
    public class Script
    {
        private static string RegString = @"(\S+) : {([\s\S]+?)}";
        private static Dictionary<string, string> ColoMum = new Dictionary<string, string>();
        private static string[] Values = null;
        
        //引擎执行入口
        /// <summary>
        /// 使用脚本来执行一些制定操作操作
        /// </summary>
        /// <param name="ScriptContent">指定脚本标题</param>
        /// <param name="ScriptFileName">脚本文件名</param>
        public static void LaunchScriptFromFile(string ScriptIndex, string ScriptFileName)
        {
            System.IO.StreamReader FileReader = new System.IO.StreamReader(ScriptFileName);
            bool inScript = false;
            string tempString = FileReader.ReadLine();
            while (tempString != null)
            {

                //执行本体循环

                if (tempString != string.Empty && tempString[0] == ';') {
                    //自动忽略注释以提高读取效率
                    tempString = FileReader.ReadLine();
                    continue;
                }

                if (tempString != string.Empty && tempString[0] == '#')
                {
                    if (tempString.Substring(1, tempString.Length - 1) == ScriptIndex)
                    {
                        inScript = true;
                    }
                }

                if (inScript && tempString != string.Empty)
                {
                    try
                    {
                        Match DataConvert = Regex.Match(tempString, RegString);
                        if (DataConvert.Success) {
                            LaunchCommand(DataConvert.Groups[1].Value, FormatStringFromMap(DataConvert.Groups[2].Value));
                        }

                    }
                    catch
                    {
                        //如果文本数据出现错误时可使用捕获错误忽略该错误，并继续读取脚本内容
                        tempString = FileReader.ReadLine();
                        continue;
                    }
                }

                if (inScript && tempString != string.Empty && (tempString == "#End"))
                {
                    inScript = false;
                    FileReader.Close();
                    return;
                }
                tempString = FileReader.ReadLine();
            }
            FileReader.Close();
        }

        public static bool LaunchCheckScriptFromFile(string ScriptIndex, string ScriptFileName)
        {
            System.IO.StreamReader FileReader = new System.IO.StreamReader(ScriptFileName);
            bool inScript = false;
            string tempString = FileReader.ReadLine();
            while (tempString != null)
            {

                //执行本体循环

                if (tempString != string.Empty && tempString[0] == ';')
                {
                    //自动忽略注释以提高读取效率
                    tempString = FileReader.ReadLine();
                    continue;
                }

                if (tempString != string.Empty && tempString[0] == '#')
                {
                    if (tempString.Substring(1, tempString.Length - 1) == ScriptIndex)
                    {
                        inScript = true;
                    }
                }

                if (inScript && tempString != string.Empty)
                {
                    try
                    {
                        Match DataConvert = Regex.Match(tempString, RegString);
                        if (DataConvert.Success)
                        {
                            if (!LaunchCheckCommand(DataConvert.Groups[1].Value, FormatStringFromMap(DataConvert.Groups[2].Value)))
                            {
                                return false;
                            }
                        }

                    }
                    catch
                    {
                        //如果文本数据出现错误时可使用捕获错误忽略该错误，并继续读取脚本内容
                        tempString = FileReader.ReadLine();
                        continue;
                    }
                }

                if (inScript && tempString != string.Empty && (tempString == "#End"))
                {
                    inScript = false;
                    FileReader.Close();
                    return true;
                }
                tempString = FileReader.ReadLine();
            }
            FileReader.Close();
            //请注意如果未加 #End 为结尾则默认为false
            return false;
        }

        /// <summary>
        /// 运行指令
        /// </summary>
        /// <param name="cmd">指令代码</param>
        /// <param name="Value">参数，为|String</param>
        private static void LaunchCommand(string cmd,string Value) {

            Values = Value.Split('|');
            if (cmd == "link")
            {
                File.mkLink(Value, Values);

            } else if (cmd=="ifmove") {
                if (Values.Count() != 2)
                {
                    return;
                }
                switch (IfDirectoryOrFile(Values[0]))
                {
                    case 0: {
                            if (!System.IO.Directory.Exists(Values[1]))
                            {
                                System.IO.Directory.Move(Values[0], Values[1]);
                            }
                            else {
                                return;
                            } 
                        }; break;
                    case 1: {
                            if (!System.IO.File.Exists(Values[1]))
                            {
                                System.IO.File.Move(Values[0], Values[1]);
                            }
                            else
                            {
                                return;
                            }
                        }; break;
                    default: { return; }
                }
            }
            else if (cmd == "move")
            {
                if (Values.Count() != 2)
                {
                    return;
                }
                switch (IfDirectoryOrFile(Values[0])) {
                    case 0: { System.IO.Directory.Move(Values[0], Values[1]); }; break;
                    case 1: { System.IO.File.Move(Values[0], Values[1]); }; break;
                    default: { return; }
                }
            }
            else if (cmd == "del")
            {
                if (Values.Count() != 2)
                {
                    return;
                }
                switch (IfDirectoryOrFile(Values[0]))
                {
                    case 0: { System.IO.Directory.Delete(Values[0], true); }; break;
                    case 1: { System.IO.File.Delete(Values[0]); }; break;
                    default: { return; }
                }
            }
            else if (cmd == "cpfile")
            {
                if (Values.Count() != 2) {
                    return;
                }

                if (System.IO.File.Exists(Values[0])) {
                    System.IO.File.Copy(Values[0], Values[1]);
                }
            }
            else if (cmd == "ini")
            {
                File.WriteInConfigure(Values);
            }
            else {
                return;
            }
        }

        /// <summary>
        /// 运行检查指令
        /// </summary>
        /// <param name="cmd">指令代码</param>
        /// <param name="Value">参数，为|String</param>
        private static bool LaunchCheckCommand(string cmd, string Value)
        {

            Values = Value.Split('|');
            if (cmd == "inir") {
                if (!File.ReadInConfigure(Values)) {
                    return false;
                }
            }
            return true;
        }
        //替换字典中匹配的项目
        public static string FormatStringFromMap(string Value) {
            string tempString = Value;

            foreach (KeyValuePair<string,string> Items in ColoMum) {
                if (tempString.Contains(Items.Key)) {
                    tempString = tempString.Replace(Items.Key, Items.Value);
                }
            }
            return tempString.Replace("\"", "");
        
        }

        //此函数用于添加替换的目标时使用
       public static void InsertMapKey(string Key,string Value) {
            if (Key!=string.Empty&&Value!=string.Empty) {

                if (ColoMum.ContainsKey(Key))
                {
                    ColoMum.Remove(Key);
                }
                ColoMum.Add(Key, Value);
            }
        }
        public static int IfDirectoryOrFile(string Scource) {
            if (System.IO.File.Exists(Scource)) 
            {
                return 1;
            } else if (System.IO.Directory.Exists(Scource))
            {
                return 0;
            }
            else {
                return -1;
            }
        }
        
    }
}
