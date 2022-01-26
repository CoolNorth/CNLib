using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CNLib.CNMessage
{

    /// <summary>
    /// 日志等级
    /// </summary>
    public enum CNLogLevel
    {
        /// <summary>
        /// 顶级异常
        /// </summary>
        TopError,
        /// <summary>
        /// 一般异常
        /// </summary>
        SimpleError,
        /// <summary>
        /// 异常
        /// </summary>
        Error,
        /// <summary>
        /// 日志
        /// </summary>
        Log,
        /// <summary>
        /// 重要日志
        /// </summary>
        MatterLog
    }

    /// <summary>
    /// JHS - 2022/01/04
    /// 日志类
    /// </summary>
    public class CNLogInfo
    {
        /// <summary>
        /// 日志路径
        /// </summary>
        public static string StrPath = string.Empty;

        /// <summary>
        /// 文件写入流
        /// </summary>
        private static StreamWriter m_swLog = new StreamWriter(string.Empty);

        /// <summary>
        /// JHS - 2022/01/04
        /// 初始化日志工具
        /// </summary>
        /// <param name="strFilePath">日志文件</param>
        /// <returns>是否成功</returns>
        public static bool InitLog(string strFilePath)
        {
            try
            {
                // 判断文件是否存在
                FileInfo fileInfo = new FileInfo(strFilePath);
                if (!fileInfo.Exists || fileInfo.Extension != "cnlog") 
                {
                    throw new IOException("日志文件不存在或不合法请从新指定");
                }
                StrPath = fileInfo.FullName;
                m_swLog = new StreamWriter(StrPath);
            }
            catch (Exception ex)
            {
                CNLog.LogError($"初始化日志文件: {ex.Message}");
                return false;
            }
            return true;
         }

        /// <summary>
        /// JHS - 2021/09/01
        /// 自检
        /// </summary>
        /// <returns></returns>
        private static bool ERRFile()
        {
            try
            {
                if (StrPath == string.Empty || !File.Exists(StrPath))
                {
                    throw new IOException("日志文件不合法，请检查是否初始化。");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// JHS - 2021/09/01
        /// 日志写入
        /// </summary>
        public static void WriteLog(string strMsg, CNLogLevel logLevel = CNLogLevel.Error)
        {
            try
            {
                // 自检
                ERRFile();

                string strHeader = string.Empty;

                // 时间： 日志内容(strMsg)
                switch (logLevel)
                {
                    case CNLogLevel.TopError:
                        strHeader = "TopError";
                        break;
                    case CNLogLevel.SimpleError:
                        strHeader = "SimpleError";
                        break;
                    case CNLogLevel.Error:
                        strHeader = "Error";
                        break;
                    case CNLogLevel.Log:
                        strHeader = "Log";
                        break;
                    case CNLogLevel.MatterLog:
                        strHeader = "MatterLog";
                        break;
                    default:
                        break;
                }

                strMsg = $"{DateTime.Now.ToString("G")} - {strHeader}: {strMsg}";
                CNLog.LogMsg(strMsg);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Log(string strMsg)
        {
            m_swLog.WriteLine(strMsg);
        }


    }

}
