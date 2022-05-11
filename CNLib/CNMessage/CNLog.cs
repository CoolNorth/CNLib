
using System;

namespace CNLib.CNMessage
{
    /// <summary>
    /// JHS - 2021/08/25
    /// 对用户信息的输出
    /// </summary>
    public class CNLog : ILog
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public void Debug(string message, Exception ex = null)
        {
            logger?.Debug(message, ex);
        }

        public void Error(string message, Exception ex = null)
        {
            logger?.Error(message, ex);
        }

        public void Fatal(string message, Exception ex = null)
        {
            logger?.Fatal(message, ex);
        }

        public void Info(string message, Exception ex = null)
        {
            logger?.Info(message, ex);
        }

        public void Trace(string message, Exception ex = null)
        {
            logger?.Trace(message, ex);
        }

        public void Warn(string message, Exception ex = null)
        {
            logger?.Warn(message, ex);
        }



        /// <summary>
        /// JHS - 2022/01/13
        /// 生成一个新的异常
        /// </summary>
        /// <param name="v"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static Exception NewError(string strMsg, Exception? ex)
        {
            if (ex == null)
                return new Exception(strMsg);
            else
                return new Exception($"{strMsg} - " + ex.Message);
        }


        #region JHS - 2021/11/10 弃用Winfrom类

        ///// <summary>
        ///// JHS - 2021/08/31
        ///// 提示框 - 弹出对用户的提示
        ///// </summary>
        ///// <param name="strMsg">提示信息</param>
        ///// <returns>用户点击的按钮</returns>
        //public static DialogResult CnDlgMsg(string strMsg)
        //{
        //    return MessageBox.Show("JHS - Log:" + strMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //}

        ///// <summary>
        ///// JHS - 2021/08/31
        ///// 提示框 - 弹出对用户的提示（包含是和取消的）
        ///// </summary>
        ///// <param name="strMsg">提示信息</param>
        ///// <returns>用户点击的按钮</returns>
        //public static DialogResult CnDlgMsgOC(string strMsg)
        //{
        //    return MessageBox.Show("JHS - Log:" + strMsg, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        //}


        ///// <summary>
        ///// JHS - 2021/08/31
        ///// 提示框 - 弹出对用户的错误提示
        ///// </summary>
        ///// <param name="strError">错误信息</param>
        ///// <returns>用户点击的按钮</returns>
        //public static DialogResult CnDlgError(string strError)
        //{
        //    return MessageBox.Show("JHS - Error:" + strError, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //}

        #endregion

    }
}
