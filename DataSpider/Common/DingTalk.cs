using DataSpider.Models;
using DataSpider.Models.DingTalkModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSpider.Common
{
    /// <summary>
    /// 发生钉钉通知
    /// 开发文档=> https://open-doc.dingtalk.com/docs/doc.htm?spm=a219a.7629140.0.0.ece6g3&treeId=257&articleId=105735&docType=1#
    /// </summary>
    public static class DingTalk
    {
        public static void Send_Morkdown(string title, List<Context> contexts)
        {
            var msg = new DTMarkdownModel("您有新的消息");
            StringBuilder sb = new StringBuilder();
            sb.Append("## " + title + "\n\n");
            int idx = 1;
            foreach (var data in contexts)
            {
                string url = data.Url;
                if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.appArticleUrl))
                    url = string.Format(Properties.Settings.Default.appArticleUrl, data.Id);
                sb.Append(string.Format("#### （{0}） [{1}]({2})\n\n", idx++, data.Title, url));
            }
            msg.AddText(sb.ToString());
            SendMsg(msg);
        }

        public static async void SendMsg(DTBaseModel context)
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.dingTalkToken))
                await HttpHelper.Post(Properties.Settings.Default.dingTalkToken, context);
        }
    }
}
