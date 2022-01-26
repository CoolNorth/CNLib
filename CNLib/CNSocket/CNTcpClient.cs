using CNLib.CNMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CNLib.CNSocket
{
    /// <summary>
    /// JHS - 2021/11/17
    /// Tcp客户端
    /// </summary>
    public class CNTcpClient
    {

        /// <summary>
        /// JHS - 2021/11/21
        /// 通信服务日志事件
        /// </summary>
        public event DelegateSocketLog OnSocketLog;

        /// <summary>
        /// JHS - 2021/11/21 接收数据事件
        /// </summary>
        public event DelegateClientMessage OnDataMsg;

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
        /// JHS - 2021/11/21
        /// TCP通讯客户端
        /// </summary>
        /// <param name="strIP">连接地址</param>
        /// <param name="nPort">连接端口号</param>
        public CNTcpClient(string strIP, int nPort)
        {
            try
            {
                this._strip = strIP;
                this._port = nPort;
                InitDelegate();
            }
            catch (Exception ex)
            {
                OnSocketLog?.Invoke(ex.Message);
            }

        }


        /// <summary>
        /// JHS - 2021/11/21
        /// 初始化委托，避免未订阅异常
        /// </summary>
        private void InitDelegate()
        {

            OnSocketLog = new DelegateSocketLog(delegate (string str) {

            });

            OnDataMsg = new DelegateClientMessage(delegate (byte[] buffer) {

            });
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
                OnSocketLog(ex.Message);
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
                        OnDataMsg(buffer);
                    }
                    else
                    {
                        OnSocketLog("请先连接到服务器");
                        break;
                    }
                }
                catch (Exception ex)
                {
                    CNLog.LogError(ex);
                    if (_client.Connected == false)
                    {
                        OnSocketLog("服务器已关闭");
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
                    OnSocketLog($"发送成功 发送量为:{ nLen }");
                    return true;
                }
                OnSocketLog($"发送失败 发送量为:{ nLen }");
                return false;
            }
            catch (Exception ex)
            {
                OnSocketLog(ex.Message);
                return false;
            }

        }

    }
}
