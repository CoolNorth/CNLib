using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CNLib.CNDbManager
{
    public interface IDbContext
    {
        object SelectTable(string tableName);




        /// <summary>
        /// JHS - 2022/01/13
        /// 执行SQL并返回影响到的行数
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns>影响到的行数</returns>
        int ExecuteNonQuery(string strSQL);

        /// <summary>
        /// JHS - 2022/01/13
        /// 执行SQL并返回结果列表
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns>数据列表</returns>
        DataTable ExecuteTable(string strSQL);


        /// <summary>
        /// JHS - 2022/01/13
        /// 执行SQL并返回第一行第一列的数据
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <returns>查询结果</returns>
        Object ExecuteScalar(string strSQL);
    }
}
