using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace GenererEtiquettes
{
    public class IniFile
    {
        private readonly string _filePath;

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern int WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string defaultValue,
            StringBuilder retVal, int size, string filePath);

        public IniFile(string filePath)
        {
            _filePath = filePath;
            var dir = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            if (!File.Exists(_filePath))
                File.Create(_filePath).Close();
        }

        public void Write(string section, string key, string value) =>
            WritePrivateProfileString(section, key, value, _filePath);

        public string Read(string section, string key, string defaultValue = "")
        {
            var sb = new StringBuilder(512);
            GetPrivateProfileString(section, key, defaultValue, sb, sb.Capacity, _filePath);
            return sb.ToString();
        }

        public void WriteInt(string section, string key, int value) => Write(section, key, value.ToString());
        public int ReadInt(string section, string key, int @default) =>
            int.TryParse(Read(section, key, @default.ToString()), out var v) ? v : @default;
    }
}
