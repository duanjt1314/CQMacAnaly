using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CQMacAnaly
{
    static class LogWriter
    {
        private static object fileLock = new object();
        private static string sepratorFlag = "----------------------------------------------------------------------------";
        public static void WriteLog(string msg)
        {
            StreamWriter sw = null;
            StringBuilder result = new StringBuilder();

            result.Append("" + msg + "\r\n");
            result.Append(sepratorFlag + "\r\n");

            string path = Path.Combine(Application.StartupPath);
            lock (fileLock)
            {

                try
                {
                    sw = File.AppendText(Path.Combine(path, DateTime.Now.ToString("yyyyMMdd") + "_log") + ".log");
                    sw.Write(result.ToString());
                    sw.Close();
                }
                catch
                {

                }

            }
        }
    }
}
