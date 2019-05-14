using SocketLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace SanJing.Tcp
{
    /// <summary>
    /// TCP连接
    /// </summary>
    public sealed class TcpConnected : IDisposable
    {
        private ConnectedSocket ConnectedSocket { get; set; }
        internal TcpConnected(ConnectedSocket connectedSocket, Encoding encoding, TcpServer tcpServer)
        {
            ConnectedSocket = connectedSocket;
            Encoding = encoding;
            TcpServer = tcpServer;
        }
        private TcpServer TcpServer { get; set; }
        private Encoding Encoding { get; set; }
        /// <summary>
        /// Socket
        /// </summary>
        public Socket UnderlyingSocket { get { return ConnectedSocket.UnderlyingSocket; } }
        /// <summary>
        /// 发送字符串
        /// </summary>
        /// <param name="data"></param>
        public void Send(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentException("Is Null", nameof(data));
            }
            string bsae64 = Convert.ToBase64String(Encoding.GetBytes(data));
            ConnectedSocket.Send(bsae64.Length.ToString("d10") + bsae64);
        }
        /// <summary>
        /// 接受全部字符串
        /// </summary>
        /// <param name="bufferSize">缓存（大小决可定速度）</param>
        /// <returns></returns>
        public string Receive(int bufferSize = 1024)
        {
            int resultLegth = Convert.ToInt32(ConnectedSocket.Receive(10));
            string result = ConnectedSocket.Receive(bufferSize);
            while (result.Length != resultLegth) { result += ConnectedSocket.Receive(bufferSize); }
            result = Encoding.GetString(Convert.FromBase64String(result));
            if (result == TcpServer._SHUTDOWNSERVER)
            {
                TcpServer.ContinueService = false;
                return string.Empty;
            }
            return result;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            ConnectedSocket.Dispose();
        }
    }
}
