using DataSpider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSpider.Models
{
    public enum ERunCycle
    {
        /// <summary>
        /// 运行一次
        /// </summary>
        [EnumDescription("一次")]
        OnlyOnce = 1,

        /// <summary>
        /// 每天
        /// </summary>
        [EnumDescription("每天")]
        EveryDay =2,

        /// <summary>
        /// 每周
        /// </summary>
        [EnumDescription("每周")]
        EveryWeek =3,

        /// <summary>
        /// 每月
        /// </summary>
        [EnumDescription("每月")]
        EveryMonth =4
    }
}
