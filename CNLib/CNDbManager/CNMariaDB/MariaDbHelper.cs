using CNLib.CNMessage;
using CNLib.CNSettings;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace CNLib.CNDbManager.CNMariaDB
{


    public class MariaDbHelper : IDbContext
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        private static string ConnStr = CNConfig.GetConn("MariaDB");

        /// <summary>
        /// 修改数据库连接名称
        /// </summary>
        /// <param name="DBName">连接名称</param>
        public static void SetConnection(string DBName)
        {
            ConnStr = CNConfig.GetConn(DBName);
        }

        /// <summary>
        /// JHS - 2022/01/13
        /// 执行SQL并返回影响到的行数
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <returns>影响到的行数</returns>
        public int ExecuteNonQuery(string strSQL)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    return cmd.ExecuteNonQuery();
                }   
            }
            catch (Exception ex)
            {
                throw CNLog.NewError("执行SQL失败", ex);
            }
        }

        /// <summary>
        /// JHS - 2022/01/13
        /// 执行SQL并返回第一行第一列的数据
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <returns>查询结果</returns>
        public object ExecuteScalar(string strSQL)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnStr))
                {
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw CNLog.NewError("查询结果失败", ex);
            }
        }

        /// <summary>
        /// JHS - 2022/01/13
        /// 执行SQL并返回结果列表
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <returns>数据列表</returns>
        public DataTable ExecuteTable(string strSQL)
        {
            DataTable? dt = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    dt = new DataTable();
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    int nRow = adapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw CNLog.NewError("查询列表失败", ex);
            }
        }

        public Object SelectTable(string tableName)
        {
            DataTable dt = new DataTable();
            string SQL = $"Select * from {tableName}";
            using (MySqlConnection conn = new MySqlConnection(ConnStr))
            {
                conn.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = new MySqlCommand(SQL, conn);
                adapter.Fill(dt);
            }
            return dt;
        }
    }
}
