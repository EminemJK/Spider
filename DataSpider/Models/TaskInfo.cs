using DataSpider.BLL;
using DataSpider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banana.Uow.Models;
using Dapper.Contrib.Extensions;

namespace DataSpider.Models
{
    [Table("T_TaskList")]
    public class TaskInfo : BaseModel
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 任务标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public ERunCycle Type { get; set; }

        [Computed]
        [Write(false)]
        public ERunState RunState { get; set; } = ERunState.HangUp;

        /// <summary>
        /// 执行时间  HH:mm:ss
        /// </summary>
        public string RunTime { get; set; }

        /// <summary>
        /// 每x天执行
        /// </summary>
        public int RunDay { get; set; }

        /// <summary>
        /// 每月份执行
        /// </summary>
        public string RunMonth { get; set; }

        /// <summary>
        /// 这个月的y号执行
        /// </summary>
        public int RunMonthDay { get; set; }

        /// <summary>
        /// 这个月的每个星期z执行
        /// </summary>
        public int RunMonthWeek { get; set; }

        /// <summary>
        /// 每x周执行
        /// </summary>
        public int RunWeek { get; set; }

        /// <summary>
        /// 每周的星期x执行
        /// </summary>
        public string RunWeekDay { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public string StartDate { get; set; }

        [Computed]
        [Write(false)]
        public DateTime DTStartDate => Convert.ToDateTime(StartDate);

        /// <summary>
        /// 结束日期
        /// </summary>
        public string EndDate { get; set; }

        [Computed]
        [Write(false)]
        public DateTime DTEndDate => Convert.ToDateTime(EndDate);

        /// <summary>
        /// 1启用0禁用
        /// </summary>
        public int Enabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否有结束日期,0无1有
        /// </summary>
        public int IsEndDate { get; set; }

        /// <summary>
        /// 爬虫配置
        /// </summary>
        [Computed]
        [Write(false)]
        public SpiderConfig SpiderConfig { get; set; }

        /// <summary>
        /// 运行消息
        /// </summary>
        [Computed]
        [Write(false)]
        public RunMessage RunMessage { get; set; }

        [Computed]
        [Write(false)]
        public SpiderMan SpiderMan { get; set; }

        public TaskInfo()
        {
            SpiderConfig = new SpiderConfig();
            RunMessage = new RunMessage();
        }

 
        /// <summary>
        /// 运行入口
        /// </summary>
        public void Run(mainForm mainForm, Action callbackFunc, bool saveToDB = true)
        {
            if (SpiderMan == null)
                SpiderMan = new SpiderMan(mainForm);
            this.SpiderMan.Run(this, callbackFunc, saveToDB);
        }
    }
    

    public class RunMessage
    {
        public string LastRunningTimeStr => JavaDate.GetDateTime(IRunTime).ToString("yyyy-MM-dd HH:mm:ss");

        public int IRunTime
        {
            get; set;
        }

        public int ErrorCount { get; set; } = 0;

        public int RunCount { get; set; } = 0;
    }
}
