using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNLib.CNNet.Frame
{
    /// <summary>
    /// 帧格式检测
    /// </summary>
    public interface IFrameDetection
    {
        /// <summary>
        /// JHS - 2022/07/05
        /// 设置帧数组
        /// </summary>
        /// <param name="buffer">帧数组</param>
        public void SetFrame(byte[] buffer);

        /// <summary>
        /// JHS - 2022/07/05
        /// 帧头检测
        /// </summary>
        /// <param name="buffer">帧数组</param>
        /// <param name="index">标识位</param>
        /// <returns>是否成功</returns>
        public bool CheckHead(ref int index);

        /// <summary>
        /// JHS - 2022/07/05
        /// 帧长读取
        /// </summary>
        /// <param name="buffer">帧数组</param>
        /// <param name="index">标识位</param>
        /// <returns>帧长</returns>
        public int GetLength(ref int index);

    }

}
