using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace DataSpider.DAL
{
    public static class SqlMapperUtil
    {
        public static string strConnetion = Properties.Settings.Default.strConnetion;

        /// <summary>
        /// sqlserver
        /// </summary>
        /// <param name="name">数据库连接字符串.</param>
        /// <returns></returns>
        //public static DbConnection OpenConnection(string name = null)
        //{
        //    string connString = string.IsNullOrWhiteSpace(name)
        //         ? ConfigurationManager.ConnectionStrings["connStr"].ConnectionString
        //         : name;
        //    var connection = new SqlConnection(connString);
        //    connection.Open();
        //    return connection;
        //}

        public static IDbConnection OpenConnection(string strConn = null)
        {
            string connString = string.IsNullOrWhiteSpace(strConn) ? strConnetion : strConn;
            var connection = new SqlConnection(connString);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// mysql
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        //public static DbConnection OpenConnection(string name = null)
        //{
        //    string connString = string.IsNullOrWhiteSpace(name)
        //         ? ConfigurationManager.ConnectionStrings["mySqlConnStr"].ConnectionString
        //         : name;
        //    MySqlConnection connection = new MySqlConnection(connString);
        //    connection.Open();
        //    return connection;
        //}

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="parms">参数</param>
        /// <returns></returns>
        public static T QuerySingle<T>(string sql, dynamic parms = null, string connectionnString = null)
        {
            using (IDbConnection conn = OpenConnection(connectionnString))
            {
                return conn.Query<T>(sql, (object)parms).FirstOrDefault();
            }
        }

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="parms">参数</param>
        /// <returns></returns>
        public static T QuerySingle<T>(IDbConnection conn, string sql, dynamic parms = null, string connectionnString = null)
        {
            return conn.Query<T>(sql, (object)parms).FirstOrDefault();

        }

        /// <summary>
        /// 执行存储过程查询单值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procname">存储过程名.</param>
        /// <param name="parms">参数.</param>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public static T QuerySingleByStoredProc<T>(string procname, dynamic parms = null, string connectionString = null)
        {
            using (IDbConnection connection = OpenConnection(connectionString))
            {
                return connection.Query<T>(procname, (object)parms, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        /// <summary>
        /// 查询集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">查询sql语句</param>
        /// <param name="parms">参数</param>
        /// <param name="connectionnString">连接字符串</param>
        /// <returns></returns>
        public static List<T> QueryList<T>(string sql, dynamic parms = null, string connectionnString = null)
        {
            using (IDbConnection conn = OpenConnection(connectionnString))
            {
                return conn.Query<T>(sql, (object)parms).ToList();
            }
        }

        /// <summary>
        /// 执行存储过程查询集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procname">存储过程名.</param>
        /// <param name="parms">参数.</param>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public static List<T> QueryListByStoredProc<T>(string procname, dynamic parms = null, string connectionString = null)
        {
            using (IDbConnection connection = OpenConnection(connectionString))
            {
                return connection.Query<T>(procname, (object)parms, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">带插入的表</param>
        /// <param name="entity">待插入的数据实体</param>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public static int Insert<T>(string sql, T entity, string connectionString = null) where T : class, new()
        {
            using (IDbConnection conn = OpenConnection(connectionString))
            {
                return conn.Execute(sql, entity);
            }
        }

        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">带插入的表</param>
        /// <param name="entity">待插入的数据实体</param>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public static int Insert<T>(IDbConnection conn, string sql, T entity, string connectionString = null) where T : class, new()
        {
            return conn.Execute(sql, entity);
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="entities">待插入的实体集合</param>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public static int InsertBatch<T>(string sql, IEnumerable<T> entities, string connectionString = null) where T : class, new()
        {
            using (IDbConnection conn = OpenConnection(connectionString))
            {
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    int res = conn.Execute(sql, entities, transaction: trans);
                    trans.Commit();

                    return res;
                }
            }
        }

        /// <summary>
        /// Insert update or delete SQL.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static int InsertUpdateOrDeleteSql(string sql, dynamic parms, string connectionString = null)
        {
            using (IDbConnection connection = OpenConnection(connectionString))
            {
                return connection.Execute(sql, (object)parms);
            }
        }

        /// <summary>
        /// Insert update or delete SQL.
        /// </summary>
        /// <param name="conn">The conn.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static int InsertUpdateOrDeleteSql(IDbConnection conn, string sql, dynamic parms, string connectionString = null)
        {
            return conn.Execute(sql, (object)parms);
        }
    }
}
