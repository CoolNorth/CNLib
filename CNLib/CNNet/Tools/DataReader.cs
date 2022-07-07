using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CNLib.CNNet.Tools
{

    /// <summary>
    /// 数据类型
    /// </summary>
    public enum FrameType
    {
        FByte,
        FChar,
        FInt32,
        FInt64,
        FDouble,
        FDateTime,
        FFile,
        FString
    }

    public class DataReader
    {
        /// <summary>
        /// 下标
        /// </summary>
        private int Index;

        /// <summary>
        /// 数据数组
        /// </summary>
        private byte[] Buffer;

        /// <summary>
        /// 帧数据流
        /// </summary>
        private MemoryStream FrameStream;

        /// <summary>
        /// 二进制读取
        /// </summary>
        private BinaryReader reader;


        public DataReader()
        {
            Index = 0;
        }


        /// <summary>
        /// JHS - 2022/07/07
        /// 设置数据流
        /// </summary>
        /// <param name="buffer"></param>
        public void SetBuffer(byte[] buffer)
        {
            this.Buffer = buffer;
            FrameStream = new MemoryStream(buffer);
            reader = new BinaryReader(FrameStream);
        }

        /// <summary>
        /// JHS - 2022/07/07
        /// 移动下标到指定位置
        /// </summary>
        /// <param name="index">下标</param>
        public void Offset(int index)
        {
            this.Index = index;
        }

        /// <summary>
        /// JHS - 2022/07/07
        /// 读取数据
        /// </summary>
        /// <param name="type">读取类型</param>
        /// <returns>读取数据</returns>
        public object Read(FrameType type)
        {
            object value = null;
            int Length = 0;
            switch (type)
            {
                case FrameType.FByte:
                    value = Buffer[Index];
                    Index += 1;
                    break;
                case FrameType.FChar:
                    value = BitConverter.ToChar(Buffer, Index);
                    Index += 2;
                    break;
                case FrameType.FInt32:
                    value = BitConverter.ToInt32(Buffer, Index);
                    Index += 4;
                    break;
                case FrameType.FInt64:
                    value = BitConverter.ToInt64(Buffer, Index);    
                    Index += 8;
                    break;
                case FrameType.FDouble:
                    value = BitConverter.ToDouble(Buffer, Index);   
                    Index += 8;
                    break;
                case FrameType.FDateTime:
                    long ticks = BitConverter.ToInt64(Buffer, Index);
                    value = new DateTime(ticks);
                    Index += 8;
                    break;
                case FrameType.FFile:
                    Length = BitConverter.ToInt32(Buffer, Index);
                    Index += 4;

                    
                    break;
                case FrameType.FString:
                    break;
                default:
                    break;
            }

            return value;
        }

        /// <summary>
        /// JHS - 2022/07/07
        /// 关闭流释放内存
        /// </summary>
        public void Close()
        {
            if (this.FrameStream != null)
            {
                reader.Close(); 
                FrameStream.Close();
            }

            reader = null;
            FrameStream = null;
        }
    }
}
