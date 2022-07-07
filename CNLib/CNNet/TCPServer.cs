using CNLib.CNMessage;
using CNLib.CNNet.Tools;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CNLib.CNNet
{


    public class TCPServer
    {

        /// <summary>
        /// JHS - 2021/11/21
        /// 订阅 - 通信服务日志事件
        /// </summary>
        public event DelegateSocketLog? OnSocketLog;

        /// <summary>
        /// JHS - 2021/11/21
        /// 订阅 - 消息处理事件
        /// </summary>
        public event DelegateServerMessage? OnDataMsg;

        /// <summary>
        /// 用户连接列表
        /// </summary>
        private List<Socket> lstClient = new List<Socket>();


        /// <summary>
        /// 私有通讯服务
        /// </summary>
        private Socket? sockServer;

        /// <summary>
        /// 私有 - 监听端口
        /// </summary>
        private int? _port = null;
        /// <summary>
        /// 私有 - 服务地址
        /// </summary>
        private string _strip = string.Empty;

        /// <summary>
        /// 私有 - 日志服务
        /// </summary>
        private CNLog logger = new CNLog();


        /// <summary>
        /// JHS - 2021/11/21
        /// 构造通讯服务 默认本机地址
        /// </summary>
        /// <param name="nPort">监听端口号</param>
        public TCPServer(int nPort)
        {
            this._strip = NetHelper.GetIP();
            this._port = nPort;

        }

        /// <summary>
        /// JHS - 2021/11/21
        /// 构造通讯服务端 并指定端口
        /// </summary>
        /// <param name="strIP">监听地址</param>
        /// <param name="nPort">监听端口号</param>
        public TCPServer(string strIP, int nPort)
        {
            this._strip = strIP;
            this._port = nPort;
        }

        /// <summary>
        /// JHS - 2021/11/21
        /// 启动监听函数
        /// </summary>
        public void StartListen()
        {
            try
            {
                // 判断是否设置了端口号
                if (!this._port.HasValue)
                {
                    throw new Exception("端口号未设置");
                }
                this.sockServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint point = new IPEndPoint(IPAddress.Parse(this._strip), this._port.Value);
                this.sockServer.Bind(point);
                this.sockServer.Listen(int.MaxValue);
                logger.Info($"Listener {this._port} Is Open.");

                // 启动监听线程
                Thread listenThread = new Thread(Listener);
                listenThread.IsBackground = true;
                listenThread.Start();
            }
            catch (Exception ex)
            {
                logger.Info($"Listener {this._port} Not Open.\nError: ", ex);
            }
        }

        /// <summary>
        /// JHS - 2021/11/21
        /// 监听线程
        /// </summary>
        /// <param name="obj"></param>
        private void Listener(object obj)
        {
            while (true)
            {
                try
                {
                    //Socket创建的新连接
                    Socket clientSocket = this.sockServer.Accept();
                    logger.Info($"Client {clientSocket.RemoteEndPoint.ToString()} Connected.");

                    Thread threadAccept = new Thread(AcceptClient);
                    threadAccept.IsBackground = true;
                    threadAccept.Start(clientSocket);

                    Thread threadRecvice = new Thread(RecviveClient);
                    threadRecvice.IsBackground = true;
                    threadRecvice.Start(clientSocket);
                }
                catch (Exception ex)
                {
                    //OnSocketLog?.Invoke(ex.Message);
                    logger.Error("监听启动失败", ex);
                }
            }
        }


        /// <summary>
        /// JHS - 2021/11/21
        /// 接收消息线程
        /// </summary>
        /// <param name="clientObj"></param>
        private void RecviveClient(object clientObj)
        {
            Socket _clientSock = null;
            while (true)
            {
                try
                {
                    byte[] temp = new byte[1024];
                    _clientSock = clientObj as Socket;
                    int length = _clientSock.Receive(temp);
                    if (length == 0)
                    {
                        break;
                    }
                    if (!_clientSock.Connected)
                    {
                        throw new Exception($"Client {_clientSock.RemoteEndPoint.ToString()} Is Not Connected.");
                    }
                    byte[] buffer = new byte[length];
                    Array.Copy(temp, 0, buffer, 0, length);
                    OnDataMsg?.Invoke(_clientSock, buffer);
                }
                catch (Exception ex)
                {
                    logger.Error($"Recvive Error:", ex);
                }
            }
        }

        /// <summary>
        /// JHS - 2021/11/21
        /// 连接处理线程
        /// </summary>
        /// <param name="obj">正在连接的客户端</param>
        private void AcceptClient(object clientSocket)
        {
            Socket? client = clientSocket as Socket;

            lock (this.lstClient)
            {
                List<Socket> list = new List<Socket>(); 
                // 记录无效连接
                foreach (Socket item in this.lstClient)
                {
                    if(item.Poll(1000, SelectMode.SelectRead))
                    {
                        logger.Info($"Client {item.RemoteEndPoint.ToString()} Off Line.");
                        list.Add(item); 
                    }
                }
                foreach (Socket item in list)
                {
                    lstClient.Remove(item);
                }
                this.lstClient.Add(client);
            }
        }

        /// <summary>
        /// JHS - 2021/12/24
        /// 获取所有用户连接
        /// </summary>
        /// <returns></returns>
        public List<Socket> GetAllClients()
        {
            return this.lstClient;
        }
    }
}
