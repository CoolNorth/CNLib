using System;

namespace CNLib.CNNet.Frame
{
    /// <summary>
    /// 帧验证服务
    /// </summary>
    public class FrameDetection 
    {
        /// <summary>
        /// 私有 - 帧数组
        /// </summary>
        private static byte[] Buffer;

        /// <summary>
        /// 私有 - 帧头
        /// </summary>
        private const byte HEAD = 0xFF;

        /// <summary>
        /// 私有 - 帧长
        /// </summary>
        private static int length;

        /// <summary>
        /// 只读 - 帧长
        /// </summary>
        public static int Length {
            get { return length; }
            private set { length = value; }
        }


        /// <summary>
        /// JHS - 2022/07/05
        /// 设置帧数组
        /// </summary>
        /// <param name="buffer">帧数组</param>
        public static void SetFrame(byte[] buffer)
        {
            Buffer = buffer;
        }

        /// <summary>
        /// JHS - 2022/07/05
        /// 帧头检测
        /// </summary>
        /// <param name="index">标识位</param>
        /// <returns>是否成功</returns>
        public static bool CheckHead(ref int index)
        {
            if (Buffer.Length > 4)
            {
                for (index = 0; index < 4; index++)
                {
                    if (Buffer[index] != HEAD)
                    {
                        index += 1;
                        return false;
                    }
                }
                return true;
            }
            return false; 
        }

        /// <summary>
        /// JHS - 2022/07/05
        /// 帧长读取
        /// </summary>
        /// <param name="buffer">帧数组</param>
        /// <param name="index">标识位</param>
        /// <returns>帧长</returns>
        public static int GetLength(ref int index)
        {
            if (Buffer.Length > 8)
            {
                index = 4;
                Length = BitConverter.ToInt32(Buffer, index);
                index += 4;
                return Length;
            }
            return -1;
        }
    }
}
