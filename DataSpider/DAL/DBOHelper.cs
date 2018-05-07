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
        public static List<Context> SaveData(List<Context> dataList)
        {
            List<Context> contexts = new List<Context>();
            using (var conn = SqlMapperUtil.OpenConnection())
            {
                foreach (var data in dataList)
                {
                    int id = conn.Query<int>(SpiderSQL.Insert_DeclareData, data).FirstOrDefault();
                    if (id > 0)
                    {
                        data.Id = id;
                        conn.Execute(SpiderSQL.Insert_Context, new { DeclareID = id, HtmlContext = data.HtmlContext });
                        contexts.Add(data);
                    }
                }
            }
            return contexts;
        }


        /// <summary>
        /// 获取任务列表
        /// </summary> 
        public static Dictionary<int,TaskInfo> GetTaskInfos()
        {
            Dictionary<int, TaskInfo> taskInfos = new Dictionary<int, TaskInfo>();
            using (var conn = SqlMapperUtil.OpenConnection())
            {
                var infos = conn.Query<TaskInfo, SpiderConfig, TaskInfo>(SpiderSQL.Select_TaskList, (t, s) =>
                   {
                       t.SpiderConfig = s;
                       if (t.Enabled == 0)
                           t.RunState = ERunState.Stop;
                       else
                           t.RunState = ERunState.HangUp;
                       taskInfos.Add(t.Id, t);
                       return t;
                   }, splitOn: "sp");
            }
            return taskInfos;
        }

        /// <summary>
        /// 保存新任务
        /// </summary> 
        /// <returns>任务id</returns>
        public static int SaveNewTask(TaskInfo taskInfo)
        {
            int taskId = SqlMapperUtil.QuerySingle<int>(SpiderSQL.Insert_NewTask, taskInfo);
            if (taskId > 0)
            {
                taskInfo.SpiderConfig.TaskId = taskId;
                int configId = SqlMapperUtil.QuerySingle<int>(SpiderSQL.Insert_NewSpiderConfig, taskInfo.SpiderConfig);
                taskInfo.SpiderConfig.Id = configId;
            }
            return taskId;
        }
        
        /// <summary>
        /// 更新任务
        /// </summary> 
        public static int UpdateTask(TaskInfo taskInfo)
        {
            int res = SqlMapperUtil.InsertUpdateOrDeleteSql(SpiderSQL.Update_TaskInfo, taskInfo);
            if (res > 0)
            {
                SqlMapperUtil.InsertUpdateOrDeleteSql(SpiderSQL.Update_SpiderConfig, taskInfo.SpiderConfig);
            }
            return res;
        }

        /// <summary>
        /// 更新任务状态
        /// </summary>
        /// <param name="taskInfo"></param>
        /// <returns></returns>
        public static int UpdateTaskState(TaskInfo taskInfo)
        {
            int res = SqlMapperUtil.InsertUpdateOrDeleteSql(SpiderSQL.Update_TaskInfoState, taskInfo); 
            return res;
        }

        /// <summary>
        /// 更新任务状态
        /// </summary>
        /// <param name="taskInfo"></param>
        /// <returns></returns>
        public static int DeleteTask(int taskId)
        {
            int res = SqlMapperUtil.InsertUpdateOrDeleteSql(SpiderSQL.Delete_TaskInfo, new { id = taskId });
            SqlMapperUtil.InsertUpdateOrDeleteSql(SpiderSQL.Delete_SpiderConfig, new { TaskId = taskId });
            return res;
        }
    }
}
