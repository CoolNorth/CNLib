using System;
using System.Collections.Generic;
using System.Text;

namespace CNLib.CNMessage
{
    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// 详细信息
        /// </summary>
        /// <param name="message">详细内容</param>
        /// <param name="ex">异常</param>
        void Trace(string message, Exception ex = null);
        
        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message">调试内容</param>
        /// <param name="ex">异常</param>
        void Debug(string message, Exception ex = null);

        /// <summary>
        /// 消息信息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="ex">异常</param>
        void Info(string message, Exception ex = null);

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="message">警告内容</param>
        /// <param name="ex">异常</param>
        void Warn(string message, Exception ex = null);

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="message">错误内容</param>
        /// <param name="ex">异常</param>
        void Error(string message, Exception ex = null);

        /// <summary>
        /// 严重错误信息
        /// </summary>
        /// <param name="message">严重错误内容</param>
        /// <param name="ex">异常</param>
        void Fatal(string message, Exception ex = null);
    }
}
