using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSpider.Models.DingTalkModel
{
    public class DTTextModel : DTBaseModel
    {
        public DTAtModel at { get; set; }

        public DTTextContextModel text { get; set; }

        public DTTextModel(string msg)
        {
            msgtype = "text";
            at = new DTAtModel();
            text = new DTTextContextModel() { content = msg };
        }

        public class DTTextContextModel
        {
            public string content { get; set; }
        }
    }
}
