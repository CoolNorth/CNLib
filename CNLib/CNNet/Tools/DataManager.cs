using System;
using System.IO;
using System.Text;

namespace CNLib.CNNet.Tools
{
    /// <summary>
    /// JHS - 2021/11/19
    /// 对数据包的解析
    /// </summary>
    public class DataManager
    {
        /// <summary>
        /// 默认帧头
        /// </summary>
        private readonly byte[] _byHead = new byte[] { 0xA0, 0xA0, 0xA0, 0xA0 };

        /// <summary>
        /// 全局私有数据
        /// </summary>
        private byte[] _buffer = new byte[] { };

        /// <summary>
        /// 数据流
        /// </summary>
        private MemoryStream? _stream = null;

        /// <summary>
        /// 读取流
        /// </summary>
        private BinaryReader? _reader = null;

        /// <summary>
        /// 写入流
        /// </summary>
        private BinaryWriter? _writer = null;

        /// <summary>
        /// JHS - 2021/11/19
        /// 构造
        /// </summary>
        /// <param name="buffer">接收到的数据</param>
        public DataManager(byte[] buffer)
        {
            this._buffer = buffer;
            _stream = new MemoryStream(buffer);
            _reader = new BinaryReader(_stream);
            _writer = new BinaryWriter(_stream);
        }
        
        


        #region 读取方法

        /// <summary>
        /// JHS - 2021/11/19
        /// 判断帧头是否正确
        /// </summary>
        /// <returns>判断结果</returns>
        private bool ReadHead()
        {
            // 读取四字节 判断是否是帧头
            byte[] byHead = this._reader.ReadBytes(4);
            if (this._byHead == byHead)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// JHS - 2021/11/19
        /// 读取包总长度
        /// </summary>
        /// <returns>总长度</returns>
        private int ReadLen()
        {
            return this._reader.ReadInt32();
        }

        /// <summary>
        /// JHS - 2021/11/19
        /// 读取一个byte类型的数据
        /// </summary>
        /// <returns>解析结果</returns>
        public byte ReadByte()
        {
            return this._reader.ReadByte();
        }

        /// <summary>
        /// JHS - 2021/11/19
        /// 读取一个Int32类型的数据
        /// </summary>
        /// <returns>解析结果</returns>
        public int ReadInt()
        {
            return this._reader.ReadInt32();
        }

        /// <summary>
        /// JHS - 2021/11/19
        /// 读取一个UInt类型的数据
        /// </summary>
        /// <returns>解析结果</returns>
        public uint ReadUInt()
        {
            return _reader.ReadUInt32();
        }

        /// <summary>
        /// JHS - 2021/11/19
        /// 读取一个Short类型的数据
        /// </summary>
        /// <returns>解析结果</returns>
        public short ReadShort()
        {
            return this._reader.ReadInt16();
        }

        /// <summary>
        /// JHS - 2021/11/19
        /// 读取一个UShort类型的数据
        /// </summary>
        /// <returns>解析结果</returns>
        public ushort ReadUShort()
        {
            return this._reader.ReadUInt16();
        }

        /// <summary>
        /// JHS - 2021/11/19
        /// 读取一个Long类型的数据
        /// </summary>
        /// <returns>解析结果</returns>
        public long ReadLong()
        {
            return this._reader.ReadInt64();
        }

        /// <summary>
        /// JHS - 2021/11/19
        /// 读取一个ULong类型的数据
        /// </summary>
        /// <returns>解析结果</returns>
        public ulong ReadULong()
        {
            return this._reader.ReadUInt64();
        }

        /// <summary>
        /// JHS - 2021/11/19
        /// 读取一个Float类型的数据
        /// </summary>
        /// <returns>解析结果</returns>
        public float ReadFloat()
        {
            byte[] temp = BitConverter.GetBytes(this._reader.ReadSingle());
            Array.Reverse(temp);
            return BitConverter.ToSingle(temp, 0);
        }

        /// <summary>
        /// JHS - 2021/11/19
        /// 读取一个Double类型的数据
        /// </summary>
        /// <returns>解析结果</returns>
        public double ReadDouble()
        {
            byte[] temp = BitConverter.GetBytes(this._reader.ReadDouble());
            Array.Reverse(temp);
            return BitConverter.ToDouble(temp, 0);
        }

        /// <summary>
        /// JHS - 2021/11/19
        /// 读取一个String类型的数据
        /// 会先读取一个长度，在读取字符串
        /// </summary>
        /// <returns>解析结果</returns>
        public string ReadString()
        {
            ushort len = ReadUShort();
            byte[] buffer = new byte[len];
            buffer = this._reader.ReadBytes(len);
            return Encoding.UTF8.GetString(buffer);
        }

        /// <summary>
        /// JHS - 2021/11/19
        /// 读取一个字节数组类型的数据
        /// 会先读取一个长度，在读取字符串
        /// </summary>
        /// <returns>解析结果</returns>
        public byte[] ReadBytes()
        {
            int len = ReadInt();
            return this._reader.ReadBytes(len);
        }

        /// <summary>
        /// JHS - 2021/11/19
        /// 读取一个指定长度的字节数组类型的数据
        /// </summary>
        /// <returns>解析结果</returns>
        public byte[] ReadBytes(int nLen)
        {
            return this._reader.ReadBytes(nLen);
        }

        #endregion

        #region 写入方法




        #endregion

        /// <summary>
        /// JHS - 2021/11/19
        /// 关闭流并释放内存
        /// </summary>
        public void Close()
        {
            if (this._stream != null)
            {
                _reader.Close();
            }
            if (_stream != null)
            {
                _stream.Close();
            }
            _reader = null;
            _stream = null;
        }
    }
}
