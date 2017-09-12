using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogApplication.Helper
{
    /// <summary>
    /// 设置文件路径
    /// </summary>
    public class LogPath
    {
        public string MyPath { get; set; }//设置路径
        private LogPath()
        {
            this.MyPath = AppDomain.CurrentDomain.BaseDirectory;//这里设置为文件根目录，也可以读取配置文件
        }
        private static LogPath MyLogPath = new LogPath();
        public static LogPath CreateLogPath()
        {
            return MyLogPath;
        }

    }
}
