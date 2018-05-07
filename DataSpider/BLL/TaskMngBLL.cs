using DataSpider.Common;
using DataSpider.DAL;
using DataSpider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSpider.BLL
{
    public class TaskMngBLL
    {
        /// <summary>
        /// 按任务ID来存储任务清单
        /// </summary>
        private Dictionary<int, TaskInfo> taskInfos;

        public List<TaskInfo> TaskInfos => taskInfos.Values.ToList();

        private TaskMngBLL()
        {
            taskInfos = DBOHelper.GetTaskInfos();
        }

        private static readonly object obj = new object();
        private static TaskMngBLL instance = null;
        public static TaskMngBLL GetInstance()
        {
            if (instance == null)
            {
                lock (obj)
                {
                    if (instance == null)
                    {
                        instance = new TaskMngBLL();
                    }
                }
            }
            return instance;
        }
         
        /// <summary>
        /// 保存新的任务
        /// </summary> 
        public int AddNewTask(TaskInfo taskInfo)
        {
            int taskId = DBOHelper.SaveNewTask(taskInfo);
            if (taskId > 0)
            {
                taskInfo.Id = taskId;
                taskInfos.Add(taskId, taskInfo);
            }
            return taskId;
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        public TaskInfo GetTask(int taskId)
        {
            if (!taskInfos.ContainsKey(taskId))
                return null;
            return taskInfos[taskId];
        }

        /// <summary>
        /// 更新任务
        /// </summary>
        public int UpdateTask(TaskInfo taskInfo)
        {
            int res = DBOHelper.UpdateTask(taskInfo);
            if (res > 0)
            {
                if (!taskInfos.ContainsKey(taskInfo.Id))
                    taskInfos.Add(taskInfo.Id, taskInfo);
                else
                    taskInfos[taskInfo.Id] = taskInfo;
            }
            return res;
        }

        /// <summary>
        /// 启用1，禁用0，删除-1
        /// </summary>
        public string ChangeState(int taskId,int state)
        { 
            var task = GetTask(taskId);
            if (task.RunState == ERunState.Running)
            {
                return "任务运行中，不能修改状态";
            }
            else if (state == 1)
            {
                task.RunState = ERunState.HangUp;
                if (task.Enabled == 1)
                    return "";
                task.Enabled = 1; 
                DBOHelper.UpdateTaskState(task);
            }
            else if (state == 0)
            {
                if (task.Enabled == 0)
                    return "";
                task.Enabled = 0;
                task.RunState = ERunState.Stop;
                DBOHelper.UpdateTaskState(task);
            }
            else if (state == -1)
            {
                taskInfos.Remove(taskId);
                DBOHelper.DeleteTask(taskId);
            }
            return "";
        }

        /// <summary>
        /// 运行计划任务
        /// </summary>
        public bool Run(mainForm mainForm)
        {
            DateTime dateNow = DateTime.Now;
            dateNow = dateNow.AddSeconds(-dateNow.Second); //去掉秒，精确到分钟，秒容易遗漏 
            string yyyyMMdd = dateNow.ToString("yyyy-MM-dd");
            string HHmmss = dateNow.ToString("HH:mm:ss"); 

            int nowTimeRun = JavaDate.GetUnixTime(dateNow);
            int nowWeekIdx = JavaDate.GetWeekIndex(dateNow);
            bool isNeedUpdateForm = false;
            foreach (TaskInfo task in TaskInfos)
            {
                if (task.Enabled == 1 && task.IsEndDate == 1 && task.DTEndDate < dateNow)
                {
                    task.RunState = ERunState.Stop;
                    continue;
                }
                if (task.Enabled == 1 && task.RunState == ERunState.HangUp && task.RunMessage.IRunTime != nowTimeRun 
                    && task.RunTime == HHmmss 
                    && dateNow >=task.DTStartDate)
                {
                    switch (task.Type)
                    {
                        case ERunCycle.OnlyOnce:
                            if (task.StartDate == yyyyMMdd)
                            {
                                task.Run(mainForm, () =>
                                {
                                    task.RunState = ERunState.Stop;
                                    mainForm.RefreshDataGridView(task);
                                }, true);
                                isNeedUpdateForm = true;
                            }
                            break;
                        case ERunCycle.EveryDay:
                            if ((dateNow - task.DTStartDate).Days % task.RunDay == 0)
                            {
                                task.Run(mainForm, () =>
                                {
                                    mainForm.RefreshDataGridView(task);
                                }, true);
                                isNeedUpdateForm = true;
                            }
                            break;
                        case ERunCycle.EveryWeek:
                            string[] spWeek = task.RunWeekDay.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            if (nowWeekIdx % task.RunWeek == 0 && spWeek[(int)dateNow.DayOfWeek] == "1")
                            {
                                task.Run(mainForm, () =>
                                {
                                    mainForm.RefreshDataGridView(task);
                                }, true);
                                isNeedUpdateForm = true;
                            }
                            break;
                        case ERunCycle.EveryMonth:
                            string[] spMonth = task.RunMonth.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            if (spMonth[dateNow.Month - 1] == "1")
                            {
                                if (task.RunMonthWeek >= 0 && dateNow.DayOfWeek == (DayOfWeek)task.RunMonthWeek)
                                {
                                    //这个月的星期X运行一次
                                    task.Run(mainForm, () =>
                                    {
                                        mainForm.RefreshDataGridView(task);
                                    }, true);
                                    isNeedUpdateForm = true;
                                }
                                else if (task.RunMonthDay == dateNow.Day)
                                {
                                    task.Run(mainForm, () =>
                                    {
                                        mainForm.RefreshDataGridView(task);
                                    }, true);
                                    isNeedUpdateForm = true;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    task.RunMessage.IRunTime = nowTimeRun;
                }
            }
            return isNeedUpdateForm;
        }
    }
}
