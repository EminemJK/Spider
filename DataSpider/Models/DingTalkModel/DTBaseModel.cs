using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSpider.Models.DingTalkModel
{
    /// <summary>
    /// 开发文档：https://open-doc.dingtalk.com/docs/doc.htm?spm=a219a.7629140.0.0.ece6g3&treeId=257&articleId=105735&docType=1
    /// </summary>
    public abstract class DTBaseModel
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public string msgtype { get; set; }
    }

    /// <summary>
    /// 艾特
    /// </summary>
    public class DTAtModel
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public List<string> atMobiles { get; set; } = new List<string>();
        
        /// <summary>
        /// 是否艾特群里全部伙伴
        /// </summary>
        public bool isAtAll { get; set; } = false;
    }
}
