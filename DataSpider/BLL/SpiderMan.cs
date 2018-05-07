using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataSpider.Common;
using DataSpider.Models;
using HtmlAgilityPack;

namespace DataSpider.BLL
{
    public class SpiderMan : BaseSpider
    {
        public SpiderMan(mainForm mainForm) : base(mainForm)
        {
        }

        protected override void GetTitleList(HtmlNodeCollection htmlNodes)
        {  
            foreach (var data in htmlNodes)
            {
                HtmlNode node = HtmlNode.CreateNode(data.OuterHtml);
                HtmlNode a = node.SelectSingleNode(TaskInfo.SpiderConfig.ListTitleTag);
                if (a == null )
                {
                    if (node.OuterHtml.Contains("</a>"))
                    {
                        ReportInfo($"获取文章A标签失败，xpath：{ TaskInfo.SpiderConfig.ListTitleTag}", true);
                    }
                    continue;
                }
                string txt = IsContains(a.InnerText);
                if (!string.IsNullOrWhiteSpace(txt) && !ResultDataDic.ContainsKey(a.InnerText))
                {
                    Context context = new Context();
                    context.KeyWord = txt;
                    context.Title = a.InnerText;
                    string url = TaskInfo.SpiderConfig.ListTitleSpliceUrl;
                    if (string.IsNullOrEmpty(url))
                    {
                        url = TaskInfo.SpiderConfig.ListUrl;
                    }
                    //获取标题的超链接，以访问文章具体内容
                    context.Url = GetUrl(url, a.Attributes["href"].Value); 
                    if (string.IsNullOrEmpty(TaskInfo.SpiderConfig.ListDateTag))
                    {
                        //如没有设置日期的xpath，尝试从标题中提取日期
                        context.PublishDate = GetDateFromString(a.InnerText);
                    }
                    else
                    { 
                        HtmlNode date = node.SelectSingleNode(TaskInfo.SpiderConfig.ListDateTag);
                        if (date != null)
                            context.PublishDate = GetDateFromString(date.InnerText.Trim());
                    }
                    if (string.IsNullOrEmpty(context.PublishDate))
                    { 
                        ReportInfo($"获取文章时间标签失败，xpath：{ TaskInfo.SpiderConfig.ListDateTag }", true);
                        context.PublishDate = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                   
                    if (context.PublishDateTime >= DateTime.Now.AddDays(-TaskInfo.SpiderConfig.ScanLastDay))
                    {
                        context.WebSite = TaskInfo.Title;
                        ResultDataDic.Add(context.Title, context);
                    }
                }
            }
        }

        protected override string GetNextPage(int pageNum)
        {
            string url = "";
            if (TaskInfo.SpiderConfig.FirstPageFile.ToLower().Contains("cmd"))
            {
                //约定： cmd $ajax或form $请求地址 $请求方式 $contentType $参数
                //例子： cmd $ajax $http://a.com/add/person $post $application/json $name=jack&age=18
                string pagesTurning = string.Format(TaskInfo.SpiderConfig.FirstPageFile, pageNum);
                string[] cmd = pagesTurning.Split('$');
                string type = cmd[1],
                     reqUrl = cmd[2],
                     reqType = cmd[3],
                     contentType = cmd[4],
                     param = cmd[5];
                //form提交翻页的情况，首页可以通过默认浏览的方式获取
                if (pageNum == 0 && type=="form")
                    return TaskInfo.SpiderConfig.ListUrl;
                
                if (cmd.Length < 5)
                { 
                    ReportInfo("使用命令行错误，参数项应达到6位，请检查格式：cmd $ajax/form $请求地址 $请求方式:get/post $contentType $参数", true);
                    return url;
                }
                if (reqType.Equals("post"))
                {
                    return HttpHelper.PostData(reqUrl, param, contentType);
                }
                else
                {
                    ReportInfo("使用命令行错误，仅支持ajax或form提交，其他方式未实现");
                    return url;
                }
            }
               
            if (pageNum == 0)
                url = GetUrl(TaskInfo.SpiderConfig.ListUrl, TaskInfo.SpiderConfig.FirstPageFile);
            else
                url = string.Format(GetUrl(TaskInfo.SpiderConfig.ListUrl, TaskInfo.SpiderConfig.NextPageFile), pageNum);
            return url;
        }

        protected override void GetContext()
        {
            //有些列表和详细页使用的字符编码并不一样，所以需要查看好
            if (TaskInfo.SpiderConfig.Charset.Contains("/"))
            {
                htmlWeb.OverrideEncoding = Encoding.GetEncoding(TaskInfo.SpiderConfig.Charset.Split('/')[1]);
            }
            foreach (var newList in ResultDataDic.Values)
            {
                ReportInfo($"《{newList.Title}》");
                var htmlNode = GetHtmlDoc(htmlWeb, newList.Url);
                if (htmlNode == null)
                {
                    ReportInfo($"创建文章内容读取器失败，URL：{ newList.Url }", true);
                    continue;
                }
                
                var row = htmlNode.SelectSingleNode(TaskInfo.SpiderConfig.ContextTag);
                if (row == null)
                {  
                    //如没有检测到标题，说明有可能是乱码，尝试改变编码再次加载
                    if (!htmlNode.OuterHtml.Contains(newList.Title.Replace(".", "")))
                    {
                        htmlNode = GetHtmlDoc(htmlWeb, newList.Url);
                        if (!htmlNode.OuterHtml.Contains(newList.Title.Replace(".", "")))
                        {
                            //政府网站目前只用到这两种编码其一
                            htmlWeb.OverrideEncoding = htmlWeb.OverrideEncoding == Encoding.UTF8 ? Encoding.GetEncoding("gb2312") : Encoding.UTF8;
                            htmlNode = GetHtmlDoc(htmlWeb, newList.Url);
                        }
                        if (htmlNode != null)
                            row = htmlNode.SelectSingleNode(TaskInfo.SpiderConfig.ContextTag); 
                    }
                }

                if (row == null)
                {
                    if (newList.Url.EndsWith(".pdf") || newList.Url.EndsWith(".doc") || newList.Url.EndsWith(".xls")|| !CheckThisWebSite(newList.Url))
                    {
                        newList.HtmlContext = $"<a href='{newList.Url}'>{newList.Title}</a>";
                    }
                    else
                    { 
                        ReportInfo($"加载可能出现了乱码，读取文章内容标签失败，xpath：{ TaskInfo.SpiderConfig.ContextTag}", true);
                    }
                    continue;
                }
                row = HtmlNode.CreateNode(row.OuterHtml);
                if (!string.IsNullOrEmpty(TaskInfo.SpiderConfig.ContextTitleTag))
                {
                    var titleNodes = row.SelectNodes(TaskInfo.SpiderConfig.ContextTitleTag);
                    if (titleNodes != null && titleNodes.Count > 0)
                    {
                        newList.Title = titleNodes[0].InnerText;
                    }
                    else
                    {
                        ReportInfo($"读取《{  newList.Title}》内容标题标签失败，xpath：{ TaskInfo.SpiderConfig.ContextTitleTag}", true);
                    }
                }
                var contentNodes = row.SelectNodes(TaskInfo.SpiderConfig.ContextDetailTag);
                if (contentNodes != null && contentNodes.Count > 0)
                {
                    newList.HtmlContext = contentNodes[0].InnerHtml;
                }
                else
                {
                    ReportInfo($"读取文章具体内容标签失败，xpath：{ TaskInfo.SpiderConfig.ContextDetailTag}\r\n文章Url:{ newList.Url}", true);
                }
            }
        }

        /// <summary>
        /// 检查文章的超链接是否是该网站的文章
        /// </summary>
        private bool CheckThisWebSite(string url)
        {
            Uri currentUri = new Uri(TaskInfo.SpiderConfig.ListUrl);
            Uri herfUri = new Uri(url);
            return currentUri.Host.Equals(herfUri.Host);
        }
    }
}
