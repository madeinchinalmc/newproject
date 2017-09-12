using HtmlAgilityPack;
using ILibValueService;
using LogApplication.Helper;
using Model;
using Model.Dto;
using MySpider;
using MySpider.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Console;

namespace WinFormSpider
{
    public partial class FrmSpider : Form
    {
        public FrmSpider()
        {
            InitializeComponent();
        }
        private IPTitleService _iPTitleService = BLLContainer.Container.Resolve<IPTitleService>();
        public static Queue qImg = new Queue();                                         //队列保存所有图片url
        public static List<Task> myTaskList = new List<Task>();
        Task tskState;                                                                  //暂停状态返回的Task
        private int i_Num;                                                              //下载词条数
        CancellationTokenSource cts = new CancellationTokenSource();                    //cts全局变量
        private static bool isSuspend = false;                                          //全局变量标识是否暂停
        List<string> listHref = new List<string>();                                     //保存所有子链接
        List<string> imgListHref = new List<string>();                                  //保存所有图片链接
        private static Queue<string> QueEx = new Queue<string>();                       //异常队列               
        private void button1_Click(object sender, EventArgs e)
        {
            #region 爬取首页图片
            //string sUrl = ConfigurationManager.AppSettings["BihuUrl"];
            //string myHtml = MySpiderOne.GetHtml(sUrl, Encoding.UTF8);
            ////解析Html HAP框架Xpath方式处理
            //var doc = new HtmlAgilityPack.HtmlDocument();
            //doc.LoadHtml(myHtml);
            ////获取域名
            //int len = sUrl.LastIndexOf("/") + 1;
            //string bhUrl = sUrl.Substring(0, len);
            //string sImgDiv = "/html/body/div[7]/div/div/div/ul/li";
            //HtmlNodeCollection imgNodelist = doc.DocumentNode.SelectNodes(sImgDiv);
            //foreach(var imgNode in imgNodelist)
            //{
            //    //二次解析
            //    HtmlAgilityPack.HtmlDocument docChild = new HtmlAgilityPack.HtmlDocument();
            //    docChild.LoadHtml(imgNode.InnerHtml);
            //    string sImg = "//div/img";
            //    //这里拿到图片的url
            //    var img = docChild.DocumentNode
            //        .SelectSingleNode(sImg)
            //        .Attributes["src"].Value;
            //    //WriteLine(bhUrl.Insert(bhUrl.Length, img));
            //    //拼出真正的图片url然后下载
            //    MySpiderOne.HttpDownloadFile(bhUrl.Insert(bhUrl.Length, img));
            //}
            #endregion

            #region 搜索页内连接继续爬取  单线程爬取
            //Stopwatch sp = new Stopwatch();
            //sp.Start();
            //string sUrl = ConfigurationManager.AppSettings["BihuUrl"];
            //string myHtml = MySpiderOne.GetHtml(sUrl, Encoding.UTF8);
            ////解析Html HAP框架Xpath方式处理
            //var doc = new HtmlAgilityPack.HtmlDocument();
            //doc.LoadHtml(myHtml);           //解析html
            //int len = sUrl.LastIndexOf("/") + 1;    //获取根链接位置
            //string bhUrl = sUrl.Substring(0, len);  
            //string path = "//a[not(@href='#' or @href='javascript:;' or target='_blank')]"; //第一次解析 获取到所有一级子页面
            //string pathText = "//p[contains(@class,'wen_all')]";//获取页面文字
            //HtmlNodeCollection nodePList = doc.DocumentNode.SelectNodes(pathText);
            //HtmlNodeCollection nodeList = doc.DocumentNode.SelectNodes(path);               //一级子页面list
            //List<string> listHref = new List<string>();//保存所有子链接
            //List<string> imgListHref = new List<string>();//保存所有图片链接
            //List<PTitle> pList = new List<PTitle>();


            //foreach(var nodeP in nodePList)
            //{
            //    PTitle pt = new PTitle();
            //    pt.className = nodeP.Attributes["class"].Value;
            //    pt.showVale = nodeP.InnerText;
            //    pt.labelName = nodeP.Name;
            //    pList.Add(pt);
            //}
            //foreach (var nodeA in nodeList)
            //{
            //    listHref.Add(bhUrl.Insert(bhUrl.Length, nodeA.Attributes["href"].Value));   //拼接url
            //}
            ////此时listHref 里面保存的是所有a的链接 进行二次访问
            //if(listHref.Count > 0)
            //{
            //    foreach(var aHref in listHref)
            //    {
            //        string myHtmlChild = MySpiderOne.GetHtml(aHref, Encoding.UTF8);
            //        if (string.IsNullOrEmpty(myHtmlChild))
            //            continue;
            //        var docChild = new HtmlAgilityPack.HtmlDocument();
            //        docChild.LoadHtml(myHtmlChild);
            //        string pathChild = "//img";  //抓取图片
            //        HtmlNodeCollection nodeChildList = docChild.DocumentNode.SelectNodes(pathChild);
            //        string pathChildIna = "//a[@href='#']";
            //        HtmlNodeCollection nodeAChildList = docChild.DocumentNode.SelectNodes(pathChildIna);
            //        foreach (var imgNode in nodeChildList)
            //        {
            //            string sImgPath = string.Empty;
            //            if (imgNode.Attributes["src"] != null)
            //            {
            //                if (!string.IsNullOrEmpty(imgNode.Attributes["src"].Value))
            //                {
            //                    sImgPath = imgNode.Attributes["src"].Value;
            //                    imgListHref.Add(bhUrl.Insert(bhUrl.Length, sImgPath));
            //                    //WriteLine(bhUrl.Insert(bhUrl.Length, sImgPath));
            //                }
            //            }
            //        }
            //        foreach(var imgNode in nodeAChildList)
            //        {
            //            string sImgPath = string.Empty;
            //            if (imgNode.Attributes["style"] != null)
            //            {
            //                sImgPath = imgNode.Attributes["style"].Value;
            //                Regex reg = new Regex(@"url\((images/.+?)\)");
            //                string aImgPath = reg.Match(sImgPath).Groups[1].Value;
            //                imgListHref.Add(bhUrl.Insert(bhUrl.Length, aImgPath));
            //                //WriteLine(bhUrl.Insert(bhUrl.Length, aImgPath));
            //            }

            //        }
            //        //WriteLine(aHref);
            //    }
            //}
            //List<string> imgallUrl = new List<string>();
            //if (imgListHref.Count > 0)
            //{
            //    for (int i = 0; i < imgListHref.Count(); i++)
            //    {
            //        MySpiderOne.HttpDownloadFile(imgListHref[i]);
            //    }            
            //}

            //sp.Stop();
            //WriteLine($"单线程爬虫总共运行时间为{sp.ElapsedMilliseconds}");
            //ReadLine();
            #endregion

            #region  多线程爬取数据
            try
            {
                cts = new CancellationTokenSource();
                i_Num = 0;
                button1.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = false;
                button4.Enabled = true;
                string url = ConfigurationManager.AppSettings["BihuUrl"];
                this.RunSpider(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion
        }
        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = true;
            button4.Enabled = false;
            isSuspend = true;
            //cts.Cancel();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = false;
            button4.Enabled = true;
            isSuspend = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            cts.Cancel();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 爬取逻辑
        /// </summary>
        private void RunSpider(string sUrl)
        {
            #region 搜索页内连接继续爬取
            Stopwatch sp = new Stopwatch();
            sp.Start();

             
            string myHtml = MySpiderOne.GetHtml(sUrl, Encoding.UTF8);

            //解析Html HAP框架Xpath方式处理
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(myHtml);                                                           //解析html
            int len = sUrl.LastIndexOf("/") + 1;                                            //获取根链接位置
            //int iNum = 0;
            string bhUrl = sUrl.Substring(0, len);
            string path = "//a[not(@href='#' or @href='javascript:;' or target='_blank')]"; //第一次解析 获取到所有一级子页面
            string pathText = "//p";                                                        //获取页面文字  //[contains(@class,'wen_all')]
            HtmlNodeCollection nodePList = doc.DocumentNode.SelectNodes(pathText);
            HtmlNodeCollection nodeList = doc.DocumentNode.SelectNodes(path);               //一级子页面list
            List<PTitle> pList = new List<PTitle>();
            foreach (var nodeP in nodePList)
            {
                PTitle pt = new PTitle();
                pt.className = nodeP.Attributes["class"].Value;
                pt.showVale = nodeP.InnerText;
                pt.labelName = nodeP.Name;
                pList.Add(pt);
            }
            foreach (var nodeA in nodeList)
            {
                listHref.Add(bhUrl.Insert(bhUrl.Length, nodeA.Attributes["href"].Value));   //拼接url
            }
            //此时listHref 里面保存的是所有a的链接 进行二次访问
            if (listHref.Count > 0)
            {
                foreach (var aHref in listHref)
                {
                    string myHtmlChild = MySpiderOne.GetHtml(aHref, Encoding.UTF8);
                    if (string.IsNullOrEmpty(myHtmlChild))
                        continue;
                    var docChild = new HtmlAgilityPack.HtmlDocument();
                    docChild.LoadHtml(myHtmlChild);
                    string pathChild = "//img";  //抓取图片
                    HtmlNodeCollection nodeChildList = docChild.DocumentNode.SelectNodes(pathChild);
                    string pathChildIna = "//a[@href='#']";
                    HtmlNodeCollection nodeAChildList = docChild.DocumentNode.SelectNodes(pathChildIna);
                    foreach (var imgNode in nodeChildList)
                    {
                        //
                        string sImgPath = string.Empty;
                        if (imgNode.Attributes["src"] != null)
                        {
                            if (!string.IsNullOrEmpty(imgNode.Attributes["src"].Value))
                            {
                                sImgPath = imgNode.Attributes["src"].Value;
                                imgListHref.Add(bhUrl.Insert(bhUrl.Length, sImgPath));

                                PTitle pt = new PTitle();
                                if (imgNode.Attributes["class"] == null)
                                    pt.className = "";
                                else
                                    pt.className = imgNode.Attributes["class"].Value;
                                if (imgNode.Attributes["src"] == null)
                                    pt.showVale = "";
                                else
                                    pt.showVale = imgNode.Attributes["src"].Value;
                                pt.labelName = imgNode.Name;
                                pList.Add(pt);
                            }
                        }
                    }
                    foreach (var imgNode in nodeAChildList)
                    {
                        string sImgPath = string.Empty;
                        if (imgNode.Attributes["style"] != null)
                        {
                            sImgPath = imgNode.Attributes["style"].Value;
                            Regex reg = new Regex(@"url\((images/.+?)\)");
                            string aImgPath = reg.Match(sImgPath).Groups[1].Value;
                            imgListHref.Add(bhUrl.Insert(bhUrl.Length, aImgPath));
                        }
                    }
                }
            }
            ParallelOptions option = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 3
            };
            if (imgListHref.Count > 0)
            {
                imgListHref = imgListHref.Distinct().ToList();
                Action<string> actPar = p =>
                {
                    Action<string> actKid = t =>
                    {
                        this.lbImg.Text =$"下载图片的url为:{t}";
                    };
                    this.lbImg.Invoke(actKid, p);
                };
                Action<int> actPNum = p =>
                {
                    Action<int> actKNum = t =>
                    {
                        this.lblNum.Text = $"下载词条数目为:{t.ToString()}";
                    };
                    this.lbImg.Invoke(actKNum, p);
                };
                tskState = Task.Factory.StartNew(() =>
                {
                    Parallel.ForEach(imgListHref, option, (item,state) =>
                    {
                        try
                        {
                            if (cts.IsCancellationRequested)
                            {
                                state.Stop();
                            }
                            else
                            {
                                if (isSuspend)
                                {
                                    while (isSuspend)
                                    {
                                        Thread.Sleep(1000);
                                    }
                                }
                                Thread.Sleep(50);
                                lock (this)
                                {
                                    i_Num++;                //因为这里有重复数据。List 去重复了，所以只有89
                                }
                                actPar(item);
                                actPNum(i_Num);
                                MySpiderOne.HttpDownloadFile(item);
                            }
                        }
                        catch (AggregateException ex)
                        {
                            foreach (var exItem in ex.InnerExceptions)
                            {
                                QueEx.Enqueue(exItem.Message);
                                MyLog.WriteLog(QueEx);
                                MessageBox.Show(exItem.Message);
                            }
                        }
                        #region 使用cts时候 把没跑完的任务放入一个List<Task>
                        //if (cts.IsCancellationRequested)
                        //{
                        //    Task.Factory.ContinueWhenAny(myTaskList.ToArray(),t=> {
                        //        myTaskList = new List<Task>();
                        //        myTaskList.Add(t);
                        //    });
                        //}
                        //else
                        //{
                        //    Thread.Sleep(50);
                        //    lock (this)
                        //    {
                        //        i_Num++;                //因为这里有重复数据。List 去重复了，所以只有89
                        //    }
                        //    actPar(item);
                        //    actPNum(i_Num);
                        //    MySpiderOne.HttpDownloadFile(item);
                        //}
                        #endregion
                        
                    });
                },cts.Token);
                myTaskList.Add(tskState);
            }
            Task.Factory.ContinueWhenAll(myTaskList.ToArray(), t =>
             {
                 int i = _iPTitleService.Add(pList);
                 if (i == pList.Count())
                 {
                     MessageBox.Show($"数据插入完成，共插入{i}条数据");
                 }
                 sp.Stop();
                 MessageBox.Show($"多线程爬虫总共运行时间为{sp.ElapsedMilliseconds}");
             });
            #endregion
        }
        #region//递归获取所有Url

        List<string> allUrlList = new List<string>() { ConfigurationManager.AppSettings["BihuUrl"] };
        public void TextGetUrls()
        {
            this.GetUrls(allUrlList[0]);
        }
        private void GetUrls(string url)
        {
            string myHtml = MySpiderOne.GetHtml(url, Encoding.UTF8);
            //解析Html HAP框架Xpath方式处理
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(myHtml);           //解析html
            int len = url.LastIndexOf("/") + 1;    //获取根链接位置
            string bhUrl = url.Substring(0, len);
            string path = "//a[not(@href='#' or @href='javascript:;' or target='_blank' or @href='index.htm')]"; //第一次解析 获取到所有一级子页面
            HtmlNodeCollection nodeList = doc.DocumentNode.SelectNodes(path);               //一级子页面list
            foreach (var nodeA in nodeList)
            {
                allUrlList.Add(bhUrl.Insert(bhUrl.Length, nodeA.Attributes["href"].Value));   //拼接url
            }
            if(allUrlList.Count() > 1000)
            {
                return;
            }
            //存在子节点
            if (nodeList.Count() > 0)
            {
                foreach (var nodeA in nodeList)
                {
                    GetUrls(bhUrl.Insert(bhUrl.Length, nodeA.Attributes["href"].Value));
                }
            }
        }
        #endregion
    }
}
