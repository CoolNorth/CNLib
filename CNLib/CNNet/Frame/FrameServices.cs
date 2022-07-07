namespace CNLib.CNNet.Frame
{

    /// <summary>
    /// 帧类型
    /// </summary>
    public enum ClsFrame
    {
        /// <summary>
        /// 字节 长度: 1
        /// </summary>
        FByte,
        /// <summary>
        /// 字符 长度: 2
        /// </summary>
        FChar,
        /// <summary>
        /// 整形 长度: 4
        /// </summary>
        FInt32,
        /// <summary>
        /// 文件 长度: 4
        /// </summary>
        FFie,
        /// <summary>
        /// 字符串 长度: 4
        /// </summary>
        FString,
        /// <summary>
        /// 长整型 长度: 8
        /// </summary>
        FInt64,
        /// <summary>
        /// 小数 长度: 8
        /// </summary>
        FDouble,
        /// <summary>
        /// 时间 长度: 8
        /// </summary>
        FDatetime
    }

    /// <summary>
    /// JHS - 2022/07/07
    /// 帧服务类
    /// </summary>
    public class FrameServices
    {
        /// <summary>
        /// 委托 - 日志
        /// </summary>
        public event DelegateLog OnLog;

        public FrameServices() { }

        public void StartServices()
        {
            OnLog?.Invoke("启动帧服务..");
        }


    }
}
