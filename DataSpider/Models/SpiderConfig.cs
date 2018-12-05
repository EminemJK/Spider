using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banana.Uow.Models;
using Dapper.Contrib.Extensions;

namespace DataSpider.Models
{
    [Table("T_SpiderConfig")]
    public class SpiderConfig : BaseModel
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 所属任务ID
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// 文章列表地址
        /// </summary>
        public string ListUrl { get; set; }

        /// <summary>
        /// 爬取关键字，用 / 分隔
        /// </summary>
        public string KeyWords { get; set; }

        /// <summary>
        /// 爬取页数
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 爬取最近天数
        /// </summary>
        public int ScanLastDay { get; set; }

        /// <summary>
        /// 列表标签  xpath
        /// </summary>
        public string ListTag { get; set; }

        /// <summary>
        /// 列表标题标签 xpath
        /// </summary>
        public string ListTitleTag { get; set; }

        /// <summary>
        /// 列表时间标签 xpath
        /// </summary>
        public string ListDateTag { get; set; }

        /// <summary>
        /// 标题拼接地址
        /// </summary>
        public string ListTitleSpliceUrl { get; set; }

        /// <summary>
        /// 第一页的文件名
        /// </summary>
        public string FirstPageFile { get; set; }

        /// <summary>
        /// 下一页文件名
        /// </summary>
        public string NextPageFile { get; set; }

        /// <summary>
        /// 页码起始index
        /// </summary>
        public int PageStartInx { get; set; }

        /// <summary>
        /// 正文的标签	xpath
        /// </summary>
        public string ContextTag { get; set; }

        /// <summary>
        /// 正文标题的标签	xpath
        /// </summary>
        public string ContextTitleTag { get; set; }

        /// <summary>
        /// 正文内容标签	xpath
        /// </summary>
        public string ContextDetailTag { get; set; }

        /// <summary>
        /// 网站编码
        /// </summary>
        public string Charset { get; set; }
    }
}
