using System;

namespace CNLib.CNNet.Frame
{
    public class FrameDetection : IFrameDetection
    {
        /// <summary>
        /// 私有 - 帧数组
        /// </summary>
        private byte[] buffer;

        /// <summary>
        /// 私有 - 帧头
        /// </summary>
        private byte head = 0xFF;

        /// <summary>
        /// 私有 - 帧长
        /// </summary>
        private int length;

        /// <summary>
        /// 只读 - 帧长
        /// </summary>
        public int Length {
            get { return length; }
            private set { length = value; }
        }


        /// <summary>
        /// JHS - 2022/07/05
        /// 设置帧数组
        /// </summary>
        /// <param name="buffer">帧数组</param>
        public void SetFrame(byte[] buffer)
        {
            this.buffer = buffer;
        }


        /// <summary>
        /// JHS - 2022/07/05
        /// 帧头检测
        /// </summary>
        /// <param name="index">标识位</param>
        /// <returns>是否成功</returns>
        public bool CheckHead(ref int index)
        {
            if (buffer.Length > 4)
            {
                for (index = 0; index < 4; index++)
                {
                    if (buffer[index] != head)
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
        public int GetLength(ref int index)
        {
            if (buffer.Length > 8)
            {
                index = 4;
                Length = BitConverter.ToInt32(buffer, index);
                index += 4;
                return Length;
            }
            return -1;
        }
    }
}
