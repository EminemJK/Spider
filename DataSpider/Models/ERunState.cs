using DataSpider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSpider.Models
{
    /// <summary>
    /// 运行状态
    /// </summary>
    public enum ERunState
    {
        [EnumDescription("运行中")]
        Running = 0,

        [EnumDescription("已停止")]
        Stop = 1,

        [EnumDescription("挂起中")]
        HangUp = 2
    }
}
