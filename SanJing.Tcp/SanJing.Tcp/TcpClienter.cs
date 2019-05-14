using SocketLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SanJing.Tcp
{
    /// <summary>
    /// TCP客户端
    /// </summary>
    public sealed class TcpClienter : IDisposable
    {
        
        private ConnectedSocket ConnectedSocket { get; set; }
        /// <summary>
        /// 初始化（UTF-8通信）
        /// </summary>
        /// <param name="host">IP地址</param>
        /// <param name="port">端口</param>
        public TcpClienter(string host, int port)
        {
            ConnectedSocket = new ConnectedSocket(host, port);
            Encoding = Encoding.UTF8;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="host">IP地址</param>
        /// <param name="port">端口</param>
        /// <param name="encoding">通信编码</param>
        public TcpClienter(string host, int port, Encoding encoding)
        {
            ConnectedSocket = new ConnectedSocket(host, port);
            Encoding = encoding;
        }
        /// <summary>
        /// 通信编码
        /// </summary>
        public Encoding Encoding { get; private set; }
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
            return Encoding.GetString(Convert.FromBase64String(result));
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
