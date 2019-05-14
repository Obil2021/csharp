
using SocketLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SanJing.Tcp
{
    /// <summary>
    /// TCP服务端
    /// </summary>
    public sealed class TcpServer : IDisposable
    {
        internal const string _SHUTDOWNSERVER = "/SHUTDOWNSERVER/";
        private SocketListener SocketListener { get; set; }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="host">IP地址</param>
        /// <param name="port">端口</param>
        /// <param name="encoding">通信编码</param>
        /// <param name="backlog"></param>
        public TcpServer(string host, int port, Encoding encoding, int backlog = 10)
        {
            SocketListener = new SocketListener(port, host, backlog);
            Encoding = encoding;
            ContinueService = true;
        }
        private string Host { get; set; }
        private int Port { get; set; }
        private Encoding Encoding { get; set; }
        private TcpClienter TcpClienter { get; set; }
        /// <summary>
        /// 继续服务
        /// </summary>
        public bool ContinueService { get; internal set; }
        /// <summary>
        /// Socket
        /// </summary>
        public Socket UnderlyingSocket { get { return SocketListener.UnderlyingSocket; } }
        /// <summary>
        /// 获取客户端请求（阻塞）
        /// </summary>
        /// <returns></returns>
        public TcpConnected Accept() { return new TcpConnected(SocketListener.Accept(), Encoding, this); }
        /// <summary>
        /// 关闭服务器
        /// </summary>
        public void ShutdownServer()
        {
            TcpClienter tcpClient = new TcpClienter(Host, Port, Encoding);
            tcpClient.Send(_SHUTDOWNSERVER);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            SocketListener.Dispose();
        }
    }
}
