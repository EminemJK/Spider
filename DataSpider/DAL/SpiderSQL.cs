using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSpider.DAL
{
    public class SpiderSQL
    {
        /// <summary>
        /// 保存爬取列表内容
        /// </summary>
        public const string Insert_DeclareData = @"  IF NOT EXISTS(SELECT id FROM T_DeclareData WHERE CipherText=@CipherText)
                                                      BEGIN
	                                                    INSERT INTO T_DeclareData(CreateTime,Title,Url,PublishDate,KeyWord,WebSite,CipherText) VALUES(GETDATE(),@Title,@Url,@PublishDate,@KeyWord,@WebSite,@CipherText)  Select scope_identity()
                                                      END
                                                      ELSE
                                                      BEGIN
	                                                    SELECT 0
                                                      END";
        /// <summary>
        /// 查询旧数据
        /// </summary>
        public const string Select_DeclareData = @"Select * from T_DeclareData WHERE CipherText=@CipherText";

        /// <summary>
        /// 查询旧内容
        /// </summary>
        public const string Select_Html = @"select HtmlContext from T_ContextData where DeclareID=@DeclareID";

        /// <summary>
        /// 更新
        /// </summary>
        public const string Update_DeclareData = @"update T_DeclareData set Url=@url where id=@id";

        /// <summary>
        /// 更新内容
        /// </summary>
        public const string Update_Html = @"update T_ContextData set HtmlContext=@HtmlContext where DeclareID=@DeclareID";

        /// <summary>
        /// 保存爬取详细内容
        /// </summary>
        public const string Insert_Context = @"INSERT INTO T_ContextData(DeclareID,HtmlContext) VALUES(@DeclareID,@HtmlContext)";

        /// <summary>
        /// 查询爬虫配置
        /// </summary>
        public const string Select_SpiderConfig = @"SELECT * FROM T_SpiderConfig WITH(NOLOCK)";

        /// <summary>
        /// 查询任务配置
        /// </summary>
        public const string Select_TaskList = @"SELECT t.*,'' sp,c.*
	                                            FROM T_TaskList t WITH(NOLOCK) LEFT JOIN T_SpiderConfig c WITH(NOLOCK) ON t.Id=c.TaskId";
        /// <summary>
        /// 新任务
        /// </summary>
        public const string Insert_NewTask = @"INSERT INTO T_TaskList(Title,Type,RunTime,RunDay,RunMonth,RunMonthDay,RunMonthWeek,RunWeek,RunWeekDay,StartDate,EndDate,Enabled,Remark,IsEndDate) 
                                                VALUES(@Title,@Type,@RunTime,@RunDay,@RunMonth,@RunMonthDay,@RunMonthWeek,@RunWeek,@RunWeekDay,@StartDate,@EndDate,@Enabled,@Remark,@IsEndDate)  Select scope_identity()";

        /// <summary>
        /// 新配置项
        /// </summary>
        public const string Insert_NewSpiderConfig = @"INSERT INTO T_SpiderConfig(TaskId,ListUrl,KeyWords,PageCount,ScanLastDay,ListTag,ListTitleTag,ListDateTag,ListTitleSpliceUrl,FirstPageFile,NextPageFile,PageStartInx,ContextTag,ContextTitleTag,ContextDetailTag,Charset)
                                                        VALUES(@TaskId,@ListUrl,@KeyWords,@PageCount,@ScanLastDay,@ListTag,@ListTitleTag,@ListDateTag,@ListTitleSpliceUrl,@FirstPageFile,@NextPageFile,@PageStartInx,@ContextTag,@ContextTitleTag,@ContextDetailTag,@Charset)  Select scope_identity()";

        /// <summary>
        /// 更新任务
        /// </summary>
        public const string Update_TaskInfo = @"UPDATE T_TaskList
                                               SET Title = @Title
                                                  ,Type = @Type
                                                  ,RunTime = @RunTime
                                                  ,RunDay = @RunDay
                                                  ,RunMonth = @RunMonth
                                                  ,RunMonthDay = @RunMonthDay
                                                  ,RunMonthWeek = @RunMonthWeek
                                                  ,RunWeek = @RunWeek
                                                  ,RunWeekDay = @RunWeekDay
                                                  ,StartDate = @StartDate
                                                  ,EndDate = @EndDate
                                                  ,Enabled = @Enabled
                                                  ,Remark = @Remark
                                                  ,IsEndDate =@IsEndDate
                                             WHERE  Id =@Id";

        /// <summary>
        /// 更新爬虫配置
        /// </summary>
        public const string Update_SpiderConfig = @"UPDATE T_SpiderConfig
                                                   SET ListUrl = @ListUrl
                                                      ,KeyWords = @KeyWords
                                                      ,PageCount = @PageCount
                                                      ,ScanLastDay = @ScanLastDay
                                                      ,ListTag = @ListTag
                                                      ,ListTitleTag = @ListTitleTag
                                                      ,ListDateTag = @ListDateTag
                                                      ,ListTitleSpliceUrl = @ListTitleSpliceUrl
                                                      ,FirstPageFile = @FirstPageFile
                                                      ,NextPageFile = @NextPageFile
                                                      ,PageStartInx = @PageStartInx
                                                      ,ContextTag = @ContextTag
                                                      ,ContextTitleTag = @ContextTitleTag
                                                      ,ContextDetailTag = @ContextDetailTag
                                                      ,Charset = @Charset
                                                 WHERE TaskId = @TaskId";

        /// <summary>
        /// 更新任务
        /// </summary>
        public const string Update_TaskInfoState = @"UPDATE T_TaskList
                                               SET  Enabled = @Enabled 
                                             WHERE  Id =@Id";

        /// <summary>
        /// 删除任务
        /// </summary>
        public const string Delete_TaskInfo = @"Delete T_TaskList WHERE  Id =@Id";

        /// <summary>
        /// 删除爬虫配置
        /// </summary>
        public const string Delete_SpiderConfig = @"Delete T_SpiderConfig WHERE  TaskId =@TaskId";
    }
}
