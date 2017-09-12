using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MySpider.Helper
{
      /// <summary>
      /// 爬虫工具类
      /// </summary>
    public class MySpiderOne
    {
        private static object obj = new object();
        /// <summary>
        /// 获取页面html
        /// </summary>
        public static string GetHtml(string url, Encoding encode)
        {
            //返回html页
            string html = string.Empty;
            try
            {
                // 设置参数
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.104 Safari/537.36";
                request.Timeout = 30 * 1000; //30s超时时间
                request.ContentType = "text/html; charset=utf-8";
                //发送请求并获取相应回应数据
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        //日志记录，多线程程序不适合中断
                        //logger.Warn(string.Format("抓取{0}地址返回失败,response.StatusCode为{1}", url, response.StatusCode));
                    }
                    else
                    {
                        try
                        {
                            //直到request.GetResponse()程序才开始向目标网页发送Post请求
                            StreamReader sr = new StreamReader(response.GetResponseStream(), encode);
                            html = sr.ReadToEnd();//读取数据
                            sr.Close();
                        }
                        catch (Exception ex)
                        {
                            //日志记录
                            //logger.Error(string.Format($"DownloadHtml抓取{url}失败"), ex);
                            html = null;
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Message.Equals("远程服务器返回错误: (306)。"))
                {
                    //记录日志
                    //logger.Error("远程服务器返回错误: (306)。", ex);
                    html = null;
                }
            }
            catch (Exception ex)
            {
                //logger.Error(string.Format("DownloadHtml抓取{0}出现异常", url), ex);
                html = null;
            }
            return html;
        }
        /// <summary>
        /// Http下载文件
        /// </summary>
        public static void HttpDownloadFile(string url)  // async await//,Queue<MemoryStream> qMSteam
        {

            int pos = url.LastIndexOf("/") + 1;
            string fileName = url.Substring(pos);
            string path = @"D:\Study\ProgramTestC#\Down\download";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filePathName = path + "\\" + fileName;
            if (File.Exists(filePathName)) return;

            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.104 Safari/537.36";
            request.Proxy = null;
            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();

            #region//新的
            //MemoryStream ms = new MemoryStream();
            //byte[] buffer = new byte[1024];
            //while (true)
            //{
            //    int sz = responseStream.Read(buffer, 0, 1024);
            //    if (sz == 0) break;
            //    ms.Write(buffer, 0, sz);
            //}
            //qMSteam.Enqueue(ms);
            //Task.Run(() =>
            //{
            //    ImgSave(qMSteam, fileName);
            //});
            #endregion
            #region//原来

            //创建本地文件写入流
            
            Stream stream = new FileStream(filePathName, FileMode.Create);

            byte[] bArr = new byte[1024];
            int size = responseStream.Read(bArr, 0, (int)bArr.Length);
            while (size > 0)
            {
                stream.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, (int)bArr.Length);
            }
            stream.Close();
            responseStream.Close();
            #endregion
        }
        public static void ImgSave(Queue<MemoryStream> qMSteam,string fileName)
        {
            string path = @"D:\Study\ProgramTestC#\Down\download";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filePathName = path + "\\" + fileName;
            if (File.Exists(filePathName)) return;
            lock (obj)
            {
                Stream stream = new FileStream(filePathName, FileMode.Create);
                byte[] bArr = new byte[1024];
                foreach (var sMem in qMSteam)
                {
                    int size = sMem.Read(bArr, 0, (int)bArr.Length);
                    while (size > 0)
                    {
                        stream.Write(bArr, 0, size);
                        size = sMem.Read(bArr, 0, (int)bArr.Length);
                    }
                    stream.Close();
                    sMem.Close();
                }
            }
        }

        /// <summary>
        /// Http写入内存流
        /// </summary>
        public static Queue<MemoryStream> HttpDownloadMemoryStream(Queue<string> urls,ref string fileName)  // async await
        {
            Queue<MemoryStream> qMSteam = new Queue<MemoryStream>();
            while (urls.Count > 0)
            {
                string url = urls.Dequeue();
                int pos = url.LastIndexOf("/") + 1;
                fileName = url.Substring(pos);
                //string path = @"D:\Study\ProgramTestC#\RuanMaoStudy\HomeWork\HomeWork6\HomeWork6\Down\download";
                //if (!Directory.Exists(path))
                //{
                //    Directory.CreateDirectory(path);
                //}
                //string filePathName = path + "\\" + fileName;
                //if (File.Exists(filePathName)) return null;

                // 设置参数
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.104 Safari/537.36";
                request.Proxy = null;
                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream responseStream = response.GetResponseStream();

                //新的
                MemoryStream ms = new MemoryStream();
                byte[] buffer = new byte[1024];
                while (true)
                {
                    int sz = responseStream.Read(buffer, 0, 1024);
                    if (sz == 0) break;
                    ms.Write(buffer, 0, sz);
                }
                qMSteam.Enqueue(ms);
            }
            return qMSteam;
        }
    }
}
