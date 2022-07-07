using CNLib.CNMessage;
using CNLib.CNNet.Tools;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CNLib.CNNet
{
    /// <summary>
    /// JHS - 2021/11/14
    /// 封装UDP对象
    /// 服务端：在构造时必须指定监听的端口号
    /// 客户端：在构造时可传入null值
    /// </summary>
    public class UDP
    {
        /// <summary>
        /// 事件 - 收到UDP消息
        /// </summary>
        public event DelegateDUPMessage? OnUdpMessage;

        UdpClient? udpClient = null;

        CNLog logger = null;

        /// <summary>
        /// 构造UDP协议
        /// </summary>
        /// <param name="nPort">监听的端口</param>
        public UDP(int? nPort)
        {
            logger = new CNLog();

            if (nPort == null)
            {
                udpClient = new UdpClient();
            }
            else
            {
                IPEndPoint point = NetHelper.GetIP((int)nPort);
                udpClient = new UdpClient(point);
            }

        }

        /// <summary>
        /// JHS - 2021/11/14
        /// 启动读取器
        /// </summary>
        public void StartListen()
        {
            try
            {
                Thread thread = new Thread(RecvMessage);
                thread.IsBackground = true;
                thread.Start();

            }
            catch (Exception ex)
            {
                throw new Exception("\n监听异常:\n" + ex.Message);
            }
        }

        /// <summary>
        /// JHS - 2021/11/14
        /// 接收消息线程
        /// </summary>
        /// <param name="obj"></param>
        private void RecvMessage(object obj)
        {
            byte[]? buffer = null;
            IPEndPoint point = new IPEndPoint(IPAddress.Any, 0);

            while (true)
            {
                try
                {
                    buffer = udpClient.Receive(ref point);
                    if (buffer != null)
                    {
                        OnUdpMessage?.Invoke(buffer, point);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// JHS - 2021/11/14
        /// 发送数据
        /// </summary>
        /// <param name="point">目标地址</param>
        /// <param name="buffer">数据</param>
        public void Send(IPEndPoint point, byte[] buffer)
        {
            this.udpClient.SendAsync(buffer, buffer.Length, point);
        }


    }
}
