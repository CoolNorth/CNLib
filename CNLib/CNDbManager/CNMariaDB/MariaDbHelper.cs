using CNLib.CNMessage;
using CNLib.CNNet;
using CNLib.CNSettings;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace CNLib.CNDbManager.CNMariaDB
{

    /// <summary>
    /// JHS - 2022/07/07
    /// MariaDB数据库帮助类
    /// </summary>
    public class MariaDbHelper
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        private static string ConnStr = CNConfig.GetConn("MariaDB");

        /// <summary>
        /// 委托 - 日志
        /// </summary>
        public static event DelegateLog OnLog;

        /// <summary>
        /// 修改数据库连接名称
        /// </summary>
        /// <param name="DBName">连接名称</param>
        public static void SetConnection(string DBName)
        {
            ConnStr = CNConfig.GetConn(DBName);
            if(ConnStr == String.Empty)
            {
                OnLog?.Invoke($"Get {DBName} Connect String Failed, Please Check Config.");
            }
            else
            {
                OnLog?.Invoke($"Get Connect Succeed Connect String: \n{ConnStr}.");
            }
        }

        /// <summary>
        /// JHS - 2022/01/13
        /// 执行SQL并返回影响到的行数
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <returns>影响到的行数</returns>
        public static int ExecuteNonQuery(string strSQL)
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
                OnLog?.Invoke("ExecuteNonQuery Failed!", ex);
            }

            return -1;
        }

        /// <summary>
        /// JHS - 2022/01/13
        /// 执行SQL并返回第一行第一列的数据
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <returns>查询结果</returns>
        public static object ExecuteScalar(string strSQL)
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
                OnLog?.Invoke("ExecuteScalar Failed!", ex);
            }

            return null;
        }

        /// <summary>
        /// JHS - 2022/01/13
        /// 执行SQL并返回结果列表
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <returns>数据列表</returns>
        public static DataTable ExecuteTable(string tableName)
        {
            DataTable dt = new DataTable();
            try
            {
                string strSQL = $"Select * from {tableName}";
                using (MySqlConnection conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    int nRow = adapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("ExecuteTable Failed!", ex);
            }

            return dt;
        }
    }
}
