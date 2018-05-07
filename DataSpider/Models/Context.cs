using DataSpider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSpider.Models
{
    public class Context
    {
        public int Id { get; set; }

        private string title = "";
        public string Title
        {
            get => title;
            set => title = value.Replace(" ", "").Replace("\n", "").Replace("\t", "");
        }

        public string Url { get; set; }

        private string publishDate;
        public string PublishDate
        {
            get => publishDate;
            set => publishDate = value.Replace("(", "").Replace("（", "").Replace("）", "").Replace(")", "").Replace("[", "").Replace("]", "");
        }

        public string KeyWord { get; set; }
        public string HtmlContext { get; set; } = "";

        public string WebSite { get; set; }

        public DateTime PublishDateTime
        {
            get => string.IsNullOrEmpty(PublishDate) ? DateTime.MinValue : Convert.ToDateTime(PublishDate);
        }

        public string CipherText
        {
            get
            {
                return MD5.Encrypt(Title);
            }
        }
    }
}
