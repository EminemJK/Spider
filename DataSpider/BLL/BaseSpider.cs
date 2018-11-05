using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using DataSpider.Models;
using HtmlAgilityPack;
using DataSpider.DAL;
using System.ComponentModel;
using DataSpider.Common;
using System.Text.RegularExpressions;
using System.Threading;

namespace DataSpider.BLL
{
    public abstract class BaseSpider
    {
        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        protected HtmlWeb htmlWeb;
        protected BackgroundWorker backgroundWorker = null;
        protected mainForm mainForm = null;

        protected TaskInfo TaskInfo;
        private ERunState oldRunState;

        /// <summary>
        /// 是否存储到数据库
        /// </summary>
        protected bool saveToDB = true;

        /// <summary>
        /// 线程结束后做些事情
        /// </summary>
        private Action callbackFunc;

        /// <summary>
        /// 爬取到的数据，按标题存储
        /// </summary>
        public Dictionary<string, Context> ResultDataDic;

        /// <summary>
        /// 错误消息
        /// </summary>
        protected List<string> errorList;
        public string ErrorMsg => string.Join("；", errorList);

        public BaseSpider(mainForm mainForm)
        {
            this.mainForm = mainForm;
            ResultDataDic = new Dictionary<string, Context>();
            errorList = new List<string>();

            htmlWeb = new HtmlWeb();
            htmlWeb.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.19 Safari/537.36";

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = backgroundWorker.WorkerSupportsCancellation = true ;
            backgroundWorker.DoWork += doWork;
            backgroundWorker.ProgressChanged += progressChanged;
            backgroundWorker.RunWorkerCompleted += RunWorkerCompleted;
        }
          
        protected virtual void doWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                ReportInfo("正在爬取中..."); 
                TaskInfo.RunMessage.RunCount++;
                ResultDataDic.Clear();
                errorList.Clear();
                Thread.Sleep(500);

                for (int ipage = 0; ipage <= TaskInfo.SpiderConfig.PageCount; ipage++)
                {
                    ReportInfo($"正在获取第{ipage + 1}页的标题...");
                    HtmlNode row = null;
                    if (TaskInfo.SpiderConfig.FirstPageFile.ToLower().Contains("cmd"))
                    {
                        string reqHtml = GetNextPage(ipage);
                        try
                        {
                            if (reqHtml == TaskInfo.SpiderConfig.ListUrl)
                                row = GetHtmlDoc(htmlWeb, TaskInfo.SpiderConfig.ListUrl);
                            else
                                row = HtmlNode.CreateNode(reqHtml);
                        }
                        catch (Exception exA)
                        {
                            try
                            {
                                if (exA.Message.Contains("Multiple node elments can't be created"))
                                {
                                    //尝试给内容加上标签
                                    row = HtmlNode.CreateNode($"<div>{ reqHtml }</div>");
                                }
                            }
                            catch
                            {
                                ReportInfo(exA.Message, true);
                            }
                        }
                    }
                    else
                    {
                        string url = GetNextPage(ipage);
                        row = GetHtmlDoc(htmlWeb, url);
                    }

                    if (row == null)
                        continue;

                    var htmlNodes = row.SelectNodes(TaskInfo.SpiderConfig.ListTag);
                    if (htmlNodes == null)
                    { 
                        ReportInfo($"获取文章列表失败，URL：{ TaskInfo.SpiderConfig.ListUrl }，第{ ipage }页", true);
                        continue;
                    }
                    GetTitleList(htmlNodes);
                }
                if (ResultDataDic.Count > 0)
                {
                    GetContext();
                    ReportInfo($"本次运行结束，共获取到【{ ResultDataDic.Count }】条数据...");
                    if (saveToDB)
                        SaveToDB(); 
                }
                else
                {
                    ReportInfo("近期暂无相关数据...");
                }
                InformWarning();
            }
            catch (Exception ex)
            {
                ReportInfo(ex.Message, true);
            }
            finally
            {
                TaskInfo.RunState = oldRunState;
            }
        }

        /// <summary>
        /// 线程执行完毕
        /// </summary>
        protected virtual void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        { 
            callbackFunc?.Invoke(); 
        }

        /// <summary>
        /// 外部调用执行任务
        /// </summary>
        /// <param name="taskInfo"></param>
        /// <param name="callbackFunc">线程结束后回调</param>
        /// <param name="saveToDB">是否需要保持到数据库</param> 
        public bool Run(TaskInfo taskInfo, Action callbackFunc, bool saveToDB = true)
        {
            if (!backgroundWorker.IsBusy)
            {
                TaskInfo = taskInfo;
                this.htmlWeb.OverrideEncoding = Encoding.GetEncoding(taskInfo.SpiderConfig.Charset.Split('/')[0]);
                this.callbackFunc = callbackFunc;

                this.saveToDB = saveToDB;
                if (string.IsNullOrEmpty(Description))
                    Description = taskInfo.Title;
                 
                oldRunState = taskInfo.RunState;
                taskInfo.RunState = ERunState.Running;
                backgroundWorker.RunWorkerAsync();
                return true;
            }
            else
            {
                ReportInfo("已经在运行状态中...");
                return false;
            }
        }

        #region 消息输出
        protected void ReportInfo(string msg, bool error = false)
        {
            backgroundWorker.ReportProgress(30, Description + "：" + msg);
            if (error)
            {
                errorList.Add(msg);
                TaskInfo.RunMessage.ErrorCount++;
                Log.Error(Description + "   " + msg);
            }
        }

        private void progressChanged(object sender, ProgressChangedEventArgs e)
        {
            mainForm.ReportInfo(e.UserState.ToString());
        }
        #endregion
        
        /// <summary>
        /// 获取下一页的规则
        /// </summary>
        protected abstract string GetNextPage(int pageNum);
         
        /// <summary>
        /// 获取文章列表
        /// </summary>
        protected abstract void GetTitleList(HtmlNodeCollection htmlNodes);

        /// <summary>
        /// 获取文章内容
        /// </summary>
        protected abstract void GetContext();

        /// <summary>
        /// 保存至数据库
        /// </summary>
        protected virtual void SaveToDB()
        {
            var newContextList = DBOHelper.SaveData(ResultDataDic.Values.ToList());
            if (newContextList.Count > 0)
            {
                ReportInfo($"新数据有【{ newContextList.Count }】条，钉钉通知已发送...");
                //发送钉钉通知群里的小伙伴
                string title = string.Format("来自【{0}】最新发布的{1}", Description, TaskInfo.Remark.Replace("，", ",").Split(',')[0]);
                DingTalk.Send_Morkdown(title, newContextList);
            }
        }

        /// <summary>
        /// 报警
        /// </summary>
        protected virtual void InformWarning()
        {
            try
            {
                if (TaskInfo.RunMessage.ErrorCount % 3 == 0)
                {
                    if (Properties.Settings.Default.warningOn && !string.IsNullOrWhiteSpace(Properties.Settings.Default.warningPhone))
                    {
                        DingTalk.Send_Warning("管理员，" + Description + "又㕛叒叕改版啦，快去看看。");
                    }
                }
            }
            catch
            {
                Log.Error("InformWarning 报警方法出错。");
            }

        }

        #region Func

        public HtmlNode GetHtmlDoc(HtmlWeb htmlWeb, string url)
        {
            try
            {
                var doc = GetDoc(htmlWeb, url);
                if (doc == null)
                {
                    int againIdx = 0;
                    while (againIdx++ < 5)
                    {
                        //偶尔会浏览失败，多尝试几次
                        System.Threading.Thread.Sleep(1000);
                        doc = GetDoc(htmlWeb, url);
                        if (doc != null)
                            break;
                    }
                    if (doc == null)
                    {
                        var htmlData = HttpHelper.Get(url).Result;
                        return HtmlNode.CreateNode(htmlData);
                    }
                    else
                    {
                        return doc.DocumentNode;
                    }
                }
                return doc.DocumentNode;
            }
            catch
            {
                ReportInfo("未能正确访问地址：" + url, true);
                return null;
            }
        }

        /// <summary>
        /// 加载网页
        /// </summary>
        public HtmlDocument GetDoc(HtmlWeb htmlWeb, string url)
        {
            try
            {
                return htmlWeb.Load(url);
            }
            catch
            {
                return null;
            }
        }

        protected string GetUrl(string localhost, string url)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(url))
            {
                if (url.ToLower().IndexOf("http:") == 0)
                {
                    return url;
                }
                return new Uri(new Uri(localhost), url).ToString();
            }
            else
            {
                return localhost;
            }
        }

        private List<string> keyWordtmp { get; set; } = new List<string>();

        /// <summary>
        /// 存在符合关键字
        /// </summary>
        protected string IsContains(string context)
        { 
            keyWordtmp.Clear();
            foreach (string str in TaskInfo.SpiderConfig.KeyWords.Split('/'))
            {
                if (context.Contains(str))
                {
                    keyWordtmp.Add(str);
                }
            }
            if (keyWordtmp.Count == 0)
                return null;
            return string.Join("、", keyWordtmp);
        }

        /// <summary>
        /// 从标题中提取时间
        /// </summary>
        public static string GetDateFromString(string str)
        {
            string res = "";
            try
            {
                Regex regex = new Regex(@"(\d{4}[-\\]\d{2}[-\\]\d{2})", RegexOptions.IgnoreCase);
                Match match = regex.Match(str);
                if (match.Success)
                {
                    res = match.Value; 
                }
            }
            catch
            { }
            return res;
        }
 
        #endregion
    }
}
