using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSpider.Models.DingTalkModel
{
    /// <summary>
    /// Markdown格式
    /// </summary>
    public class DTMarkdownModel : DTBaseModel
    {
        public DTAtModel at { get; set; }

        public DTMarkdownContextModel markdown { get; set; }

        public DTMarkdownModel(string title)
        {
            msgtype = "markdown";
            at = new DTAtModel();
            markdown = new DTMarkdownContextModel();
            markdown.title = title;
        }

        public void AddText(string info)
        {
            markdown.text += info;
        }

        /// <summary>
        /// Markdown内容
        /// </summary>
        public class DTMarkdownContextModel
        {
            public string title { get; set; }
            public string text { get; set; }
        }
    }
}
