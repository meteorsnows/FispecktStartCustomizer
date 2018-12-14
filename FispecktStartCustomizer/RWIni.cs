using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FispecktStartCustomizer
{
    public class RWIni
    {
        String Path;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder returnedString, int Size, string FilePath);

        public RWIni(string Path)
        {
            this.Path = Path;
        }

        public string Read(string Key, string Section)
        {
            var returnedString = new StringBuilder(255);
            GetPrivateProfileString(Section, Key, "", returnedString, 255, Path);
            return returnedString.ToString();
        }

        public void Write(string Key, string Value, string Section)
        {
            WritePrivateProfileString(Section, Key, Value, Path);
        }

        public void DeleteKey(string Key, string Section)
        {
            Write(Key, null, Section);
        }

        public void DeleteSection(string Section)
        {
            Write(null, null, Section);
        }

        public bool GetKey(string Key, string Section)
        {
            return Read(Key, Section).Length > 0;
        }
    }
}
