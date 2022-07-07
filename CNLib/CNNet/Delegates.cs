using System;
using System.Net;
using System.Net.Sockets;

namespace CNLib.CNNet
{
    /// <summary>
    /// JHS - 2021/11/21
    /// 服务中的消息委托，在订阅后可接收消息
    /// </summary>
    /// <param name="strMsg">消息内容</param>
    /// <param name="ex">异常(可空)</param>
    public delegate void DelegateLog(string strMsg, Exception ex = null);

    #region UDP协议委托


    /// <summary>
    /// JHS - 2021/11/21
    /// UDP通讯消息委托
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="point"></param>
    public delegate void DelegateDUPMessage(byte[] buffer, IPEndPoint point);

    #endregion

    #region TCP协议委托

    /// <summary>
    /// JHS - 2021/11/14
    /// TCP通讯消息委托
    /// </summary>
    /// <param name="buffer">收到的数据</param>
    /// <param name="point">发送者的网络信息</param>
    public delegate void DelegateServerMessage(object sender, byte[] buffer);

    /// <summary>
    /// JHS - 2021/11/14
    /// TCP通讯消息委托
    /// </summary>
    /// <param name="buffer">收到的数据</param>
    /// <param name="point">发送者的网络信息</param>
    public delegate void DelegateClientMessage(byte[] buffer);


    /// <summary>
    /// JHS - 2021/11/14
    /// 收到已连接用户信息
    /// </summary>
    /// <param name="client">用户</param>
    public delegate void DelegateSocketAccept(TcpClient client);

    #endregion  

}
