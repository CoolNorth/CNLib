using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CNLib.CNModel
{
    /// <summary>
    /// JHS - 2021/11/24
    /// 模型助手类，可以对结构体和字节数组进行转换
    /// </summary>
    public class CNModelHelper
    {
        /// <summary>
        /// 私有 - 追加数组
        /// </summary>
        private static byte[] _arr_data = new byte[] { };

        /// <summary>
        /// 获取追加数据，获取之后就会清空
        /// </summary>
        public static byte[] ArrData 
        {
            get
            {
                byte[] tmp = _arr_data;
                _arr_data = new byte[] { };
                return tmp; 
            } 
        }


        /// <summary>
        /// JHS - 2021/11/24
        /// 追加数组
        /// </summary>
        /// <param name="bytes">需要追加的数据</param>
        public static void AppendData(byte[] bytes)
        {
            if (_arr_data == null)
            {
                _arr_data = bytes;
                return;
            }
            _arr_data = _arr_data.Concat(bytes).ToArray();
        }

        /// <summary>
        /// JHS - 2021/11/24
        /// 传入结构体转换为字节数组
        /// </summary>
        /// <typeparam name="T">结构体类型</typeparam>
        /// <param name="objStruct">结构体对象</param>
        /// <returns>结构体字节数组</returns>
        public static byte[] StructToBytes<T>(T structObj)
        {
            byte[]? Buffer = null;
            int nSize = Marshal.SizeOf(typeof(T));
            IntPtr bufferPtr = Marshal.AllocHGlobal(nSize);
            try
            {
                Marshal.StructureToPtr(structObj, bufferPtr, false);
                Buffer = new byte[nSize];
                Marshal.Copy(bufferPtr, Buffer, 0, nSize);
            }
            catch (Exception ex)
            {
                throw new Exception("结构转字符数组失败:\n" + ex.Message);
            }
            finally
            {
                Marshal.FreeHGlobal(bufferPtr);
            }
            return Buffer;
        }

        /// <summary>
        /// JHS - 2021/11/24
        /// 字符数组转换为结构体
        /// </summary>
        /// <param name="bytes">字符数组</param>
        /// <param name="type">结构体类型</param>
        /// <returns>结构体实例</returns>
        public static object BytesToStruct(byte[] bytes, Type type)
        {
            object? objStruct = null;
            int nSize = Marshal.SizeOf(type);
            if (nSize > bytes.Length)
            {
                return null;
            }
            IntPtr structPtr = Marshal.AllocHGlobal(nSize);
            try
            {
                Marshal.Copy(bytes, 0, structPtr, nSize);
                objStruct = Marshal.PtrToStructure(structPtr, type);
            }
            catch (Exception ex)
            {
                throw new Exception("字符数组转结构失败:\n" + ex.Message);
            }
            finally
            {
                Marshal.FreeHGlobal(structPtr);
            }
            return objStruct;
        }

    }
}
