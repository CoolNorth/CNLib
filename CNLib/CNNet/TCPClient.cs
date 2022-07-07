using CNLib.CNMessage;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CNLib.CNNet
{
    /// <summary>
    /// JHS - 2021/11/17
    /// Tcp客户端
    /// </summary>
    public class TCPClient
    {

        /// <summary>
        /// JHS - 2021/11/21
        /// 通信服务日志事件
        /// </summary>
        public event DelegateLog OnLog;

        /// <summary>
        /// JHS - 2021/11/21 接收数据事件
        /// </summary>
        public event DelegateClientMessage OnData;

        /// <summary>
        /// 通讯对象
        /// </summary>
        Socket _client = null;

        /// <summary>
        /// 私有 - 地址
        /// </summary>
        string _strip = null;
        
        /// <summary>
        /// 私有 - 端口号
        /// </summary>
        int? _port = null;

        /// <summary>
        /// 日志类
        /// </summary>
        CNLog logger = new CNLog();

        /// <summary>
        /// JHS - 2021/11/21
        /// TCP通讯客户端
        /// </summary>
        /// <param name="strIP">连接地址</param>
        /// <param name="nPort">连接端口号</param>
        public TCPClient(string strIP, int nPort)
        {
            try
            {
                this._strip = strIP;
                this._port = nPort;
            }
            catch (Exception ex)
            {
                OnLog?.Invoke(ex.Message);
            }

        }


        /// <summary>
        /// JHS - 2021/11/21
        /// 连接服务端
        /// </summary>
        public bool ConnServer()
        {
            try
            {
                _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint point = new IPEndPoint(IPAddress.Parse(_strip), _port.Value);
                _client.Connect(point);

                Thread thread = new Thread(RecvData);
                thread.IsBackground = true;
                thread.Start();
                return true;
            }
            catch (Exception ex)
            {
                OnLog?.Invoke(ex.Message, ex);
                return false;
            }
            
        }

        /// <summary>
        /// JHS - 2021/11/21
        /// 接收消息线程
        /// </summary>
        /// <param name="obj"></param>
        private void RecvData(object obj)
        {
            while (true)
            {
                try
                {
                    if (_client.Connected)
                    {
                        byte[] buffer = new byte[1024 * 10];
                        int nLength = _client.Receive(buffer);
                        OnData?.Invoke(buffer);
                    }
                    else
                    {
                        OnLog?.Invoke("请先连接到服务器");
                        break;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("接收消息异常", ex);
                    if (_client.Connected == false)
                    {
                        OnLog?.Invoke("服务器已关闭", ex);
                    }

                }
                
            }
        }

        /// <summary>
        /// JHS - 2021/11/21
        /// 发送数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public bool SendData(byte[] buffer)
        {
            try
            {
                IPEndPoint point = new IPEndPoint(IPAddress.Parse(this._strip), this._port.Value);
                int nLen = this._client.SendTo(buffer, point as EndPoint); ;
                if (nLen == buffer.Length)
                {
                    OnLog?.Invoke($"发送成功 发送量为:{ nLen }");
                    return true;
                }
                OnLog?.Invoke($"发送失败 发送量为:{ nLen }");
                return false;
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("发送数据异常", ex);
                return false;
            }

        }

    }
}
