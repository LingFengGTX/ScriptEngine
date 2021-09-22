using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptEngine
{
    class Directory
    {
        public static void MakeDir(string[] Value)
        {
            if (!System.IO.Directory.Exists(Value[0])) {
                System.IO.Directory.CreateDirectory(Value[0]);
            }
        }
    }
}
