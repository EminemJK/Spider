using Banana.Uow;
using Dapper;
using DataSpider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSpider.DAL
{
    public static class DBOHelper
    {
        /// <summary>
        /// 保存爬取内容
        /// </summary>
        /// <param name="dataList"></param>
        public static ContextCollection SaveData(List<Context> dataList)
        {
            ContextCollection context = new ContextCollection();

            var contextRepo = new Repository<Context>();
            var contextDataRepo = new Repository<ContextData>();
            foreach (var data in dataList)
            {
                var oldModel = contextRepo.QueryList("CipherText=@CipherText", new { CipherText = data.CipherText }).FirstOrDefault();
                if (oldModel == null || oldModel.Id == 0)
                {
                    data.Id = (int)contextRepo.Insert(data);
                    contextDataRepo.Insert(new ContextData() { DeclareID = data.Id, HtmlContext = data.HtmlContext });
                    context.NewContexts.Add(data);
                }
                else if (oldModel.Url != data.Url)
                {
                    data.Id = oldModel.Id;
                    contextRepo.Update(data);
                    var oldHtml = contextDataRepo.QueryList("DeclareID=@DeclareID", new { DeclareID = data.Id }).FirstOrDefault();
                    if (oldHtml != null && (string.IsNullOrEmpty(oldHtml.HtmlContext) || oldHtml.HtmlContext != data.HtmlContext))
                    {
                        oldHtml.HtmlContext = data.HtmlContext;
                        contextDataRepo.Update(oldHtml);
                    }
                    context.UpdateContexts.Add(data);
                }
            }
            return context;
        }


        /// <summary>
        /// 获取任务列表
        /// </summary> 
        public static Dictionary<int,TaskInfo> GetTaskInfos()
        {
            Dictionary<int, TaskInfo> taskInfos = new Dictionary<int, TaskInfo>();
            var taskRepo = new Repository<TaskInfo>();
            var spiderRepo = new Repository<SpiderConfig>();

            var taskAll = taskRepo.QueryList();
            var spiderAll = spiderRepo.QueryList();

            foreach (var task in taskAll)
            {
                if (task.Enabled == 0)
                    task.RunState = ERunState.Stop;
                else
                    task.RunState = ERunState.HangUp;
                var sp = spiderAll.Find(s => s.TaskId == task.Id);
                task.SpiderConfig = sp;
                taskInfos.Add(task.Id, task);
                spiderAll.Remove(sp);
            }

            return taskInfos;
        }

        /// <summary>
        /// 保存新任务
        /// </summary> 
        /// <returns>任务id</returns>
        public static bool SaveNewTask(TaskInfo taskInfo)
        {
            using (var uow = new UnitOfWork())
            {
                var taskRepo = uow.Repository<TaskInfo>();
                taskInfo.Id = (int)taskRepo.Insert(taskInfo);
                if (taskInfo.Id > 0)
                {
                    taskInfo.SpiderConfig.TaskId = taskInfo.Id;
                    var spiderRepo = uow.Repository<SpiderConfig>();
                    taskInfo.SpiderConfig.Id = (int)spiderRepo.Insert(taskInfo.SpiderConfig);
                    uow.Commit();
                    return true;
                }
                else
                {
                    uow.Rollback();
                    return false;
                }
            }
        }
        
        /// <summary>
        /// 更新任务
        /// </summary> 
        public static bool UpdateTask(TaskInfo taskInfo)
        {
            bool res = UpdateTaskState(taskInfo);
            if (res)
            {
                var spiderRepo = new Repository<SpiderConfig>();
                spiderRepo.Update(taskInfo.SpiderConfig);
            }
            return res;
        }

        /// <summary>
        /// 更新任务状态
        /// </summary>
        /// <param name="taskInfo"></param>
        /// <returns></returns>
        public static bool UpdateTaskState(TaskInfo taskInfo)
        {
            var taskRepo = new Repository<TaskInfo>();
            return taskRepo.Update(taskInfo);
        }

        /// <summary>
        /// 更新任务状态
        /// </summary>
        /// <param name="taskInfo"></param>
        /// <returns></returns>
        public static bool DeleteTask(int taskId)
        {
            var taskRepo = new Repository<TaskInfo>();
            bool b = taskRepo.Delete(new TaskInfo() { Id = taskId });

            var spiderRepo = new Repository<SpiderConfig>();
            spiderRepo.Delete("TaskId =@TaskId", new { TaskId = taskId });
            return b;
        }
    }
}
