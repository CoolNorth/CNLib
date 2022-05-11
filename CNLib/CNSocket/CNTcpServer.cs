using CNLib.CNMessage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CNLib.CNSocket
{


    public class CNTcpServer
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
        /// 私有 - 用户连接
        /// </summary>
        private Socket? _clientSock;
        /// <summary>
        /// 私有 - 监听端口
        /// </summary>
        private int? _port = null;
        /// <summary>
        /// 私有 - 服务地址
        /// </summary>
        private string _strip = string.Empty;

        private CNLog logger = new CNLog();


        /// <summary>
        /// JHS - 2021/11/21
        /// 构造通讯服务 默认本机地址
        /// </summary>
        /// <param name="nPort">监听端口号</param>
        public CNTcpServer(int nPort)
        {
            this._strip = SockHelper.GetIP();
            this._port = nPort;

        }

        /// <summary>
        /// JHS - 2021/11/21
        /// 构造通讯服务端 并指定端口
        /// </summary>
        /// <param name="strIP">监听地址</param>
        /// <param name="nPort">监听端口号</param>
        public CNTcpServer(string strIP, int nPort)
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
                OnSocketLog($"成功监听{ this._port }端口");

                // 启动监听线程
                Thread listenThread = new Thread(Listener);
                listenThread.IsBackground = true;
                listenThread.Start();

            }
            catch (Exception ex)
            {
                if (OnSocketLog != null)
                    OnSocketLog(ex.Message);
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
                    OnSocketLog($"用户{ clientSocket.RemoteEndPoint } 已连接");

                    Thread threadAccept = new Thread(AcceptClient);
                    threadAccept.IsBackground = true;
                    threadAccept.Start(clientSocket);

                    Thread threadRecvice = new Thread(RecviveClient);
                    threadRecvice.IsBackground = true;
                    threadRecvice.Start(clientSocket);
                }
                catch (Exception ex)
                {
                    OnSocketLog(ex.Message);
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

            while (true)
            {

                byte[] temp = new byte[1024];
                _clientSock = clientObj as Socket;
                try
                {
                    int nLen = _clientSock.Receive(temp);
                    if (nLen == 0)
                    {
                        continue;
                    }

                    // 查看是否满足一整包


                    byte[] buffer = new byte[nLen];
                    Array.Copy(temp, 0, buffer, 0, nLen);
                    OnDataMsg?.Invoke(buffer, _clientSock);
                }
                catch (Exception ex)
                {
                    logger.Error("接收消息异常", ex);
                    OnSocketLog($"用户{_clientSock.RemoteEndPoint} 已离线");
                    _clientSock.Dispose();
                    this.lstClient.Remove(_clientSock);
                    break;
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

            // 检查是否已存在在列表中
            Socket socket = this.lstClient.Find(sock =>
            {
                if (!sock.Connected)
                {
                    OnSocketLog($"用户{sock.RemoteEndPoint} 已离线");
                }
                return sock.LocalEndPoint == client.RemoteEndPoint; 
            });

            if (socket == null)
            {
                this.lstClient.Add(client);
            }
            else
            {
                int nIndex = this.lstClient.IndexOf(socket);
                this.lstClient[nIndex] = socket;
            }

        }

        /// <summary>
        /// JHS - 2021/12/24
        /// 获取所有用户连接
        /// </summary>
        /// <returns></returns>
        public Socket GetAllClients()
        {
            return _clientSock;
        }
    }
}
