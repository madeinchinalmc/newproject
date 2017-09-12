using Model;
using Model.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace LogApplication.Helper
{
    public static class MyLog
    {
        public static object myobj = new object();
        private static string s_logPath = LogPath.CreateLogPath().MyPath;
        static string fileName = Path.Combine(s_logPath, $"{DateTime.Now.ToString("yyyy-MM-dd")}Log.txt");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="fileName"></param>
        /// <param name="cts"></param>
        public static void WriteLog(Queue<string> QueEx)
        {
            while (QueEx.Count > 0)
            {
                string s = QueEx.Dequeue();
                using (StreamWriter sw = File.AppendText(fileName))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}:{s}");
                    sw.BaseStream.Write(bytes, 0, bytes.Length);
                    sw.WriteLine();
                    sw.Flush();
                }
            }
        }
    }
       
}
