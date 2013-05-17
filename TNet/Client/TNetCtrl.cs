/*
   Tunner Net -  Data interception use ProtoBuf in game Development. 
   e-mail : dongliang17@126.com
   project: https://github.com/dongliang/Tuner-Net
*/
using System;
using System.Net.Sockets;
using System.IO;
using System.Collections.Generic;
using System.Net;
using TNet.Common;
namespace TNet.Client
{
    public interface INetMessageReader
    {
        void DidReadData(byte[] data, int size);
        void Reset();
    }
    public class TNetCtrl
    {
        private IAsyncResult m_ar_Recv = null;
        private IAsyncResult m_ar_Send = null;
        private IAsyncResult m_ar_Connect = null;
        private Socket m_ClientSocket = null;
        private string m_strRomoteIP = "127.0.0.1";
        private ITNetReader m_Reader = null;
        private ITNetWriter m_Writer = null;
        private ushort m_uRemotePort = 0;
        public const int m_RecBufferSize = 65536;
        private byte[] m_RecBuffer = new byte[m_RecBufferSize];
        private Queue<byte[]> m_SendQueue = new Queue<byte[]>();
        public bool mIsRecving = false;
        int mRecvHead = 0;//point the first valid address 
        int mRecvTail = 0;//point to the last valid address 's next address.
        public ITNetAdapter m_Adapter = null;

        public void Update()
        {

            if (IsConnected() && !mIsRecving)
            {
                if (m_Reader != null)
                {
                    mRecvHead = m_Reader.DidReadData(m_RecBuffer, mRecvHead, mRecvTail,m_Adapter);
                }
                Receive();
            }
        }

        public bool Connect(string a_strRomoteIP, ushort a_uPort)
        {
            if (m_ClientSocket == null)
            {
                try
                {
                    m_ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }
                catch (Exception e)
                {

                    m_Adapter.Debug(e.Message);
                }

                IPAddress ip = IPAddress.Parse(a_strRomoteIP);
                m_ar_Connect = m_ClientSocket.BeginConnect(ip, a_uPort, new AsyncCallback(ConnectCallback), m_ClientSocket);
                m_strRomoteIP = a_strRomoteIP;
                m_uRemotePort = a_uPort;
                return true;
            }
            return false;
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                ar.AsyncWaitHandle.Close();
                m_ar_Connect = null;
                Socket client = (Socket)ar.AsyncState;
                client.EndConnect(ar);
                client.Blocking = false;
                Receive();
            }
            catch (Exception e)
            {
                m_Adapter.Debug(e.Message);
            }
        }

        public bool IsConnected()
        {
            return m_ClientSocket != null ? m_ClientSocket.Connected : false;
        }

        private void Receive()
        {
            try
            {
                if (m_RecBufferSize == mRecvTail)
                {
                    System.Buffer.BlockCopy(m_RecBuffer, mRecvHead, m_RecBuffer, 0, mRecvTail - mRecvHead);
                    mRecvTail -= mRecvHead;
                    mRecvHead = 0;
                }

                if (m_RecBufferSize == mRecvTail)
                {
                    throw new Exception("Recieve buffer is full");
                }
                m_ar_Recv = m_ClientSocket.BeginReceive(m_RecBuffer, mRecvTail, m_RecBufferSize - mRecvTail, 0, new AsyncCallback(ReceiveCallback), 0);
                mIsRecving = true;
            }
            catch (Exception e)
            {
                m_Adapter.Debug(e.Message);
            }
        }

        public void SetSocketSendNoDeley(bool nodelay)
        {
            if (m_ClientSocket != null)
            {
                m_ClientSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, nodelay ? 1 : 0);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                ar.AsyncWaitHandle.Close();
                m_ar_Recv = null;
                Socket client = m_ClientSocket;
                int bytesRead = client.EndReceive(ar);
                if (bytesRead > 0)
                {
                    mRecvTail += bytesRead;
                    mIsRecving = false;
                }
                else
                {
                    m_Adapter.Debug("Network Shutdown");
                }
            }
            catch (Exception e)
            {
                m_Adapter.Debug(e.Message);
            }
        }

        public bool SendNetMessage<T>(int msgID, T data)
        {
            return _SendMessage(msgID, m_Writer.Serialize<T>(data));
        }

        public bool _SendMessage(int msgID, MemoryStream data)
        {
            if (m_Writer != null)
            {
                byte[] stream = m_Writer.MakeStream(msgID, data);
                lock (m_SendQueue)
                {
                    if (m_SendQueue.Count == 0)
                    {
                        return Send(stream);
                    }
                    else
                    {
                        m_SendQueue.Enqueue(stream);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool Send(byte[] byteData)
        {
            try
            {
                m_ar_Send = m_ClientSocket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), 0);
                return true;
            }
            catch (Exception e)
            {
                m_Adapter.Debug(e.Message);
            }
            return false;
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                ar.AsyncWaitHandle.Close();
                m_ar_Send = null;
                Socket client = m_ClientSocket;
                client.EndSend(ar);
                lock (m_SendQueue)
                {
                    if (m_SendQueue.Count > 0)
                    {
                        Send(m_SendQueue.Dequeue());
                    }
                }
            }
            catch (Exception e)
            {
                m_Adapter.Debug(e.Message);
            }
        }

        public bool DisConnect()
        {
            ReleaseSocket();
            return true;
        }

        public bool ReConnect()
        {
            if (m_strRomoteIP != null)
            {
                ReleaseSocket();
                //First Release Socket Resource
                return Connect(m_strRomoteIP, m_uRemotePort);
            }
            return false;
        }

        public void ReleaseSocket()
        {
            if (m_ar_Recv != null)
                m_ar_Recv.AsyncWaitHandle.Close();
            if (m_ar_Send != null)
                m_ar_Send.AsyncWaitHandle.Close();
            if (m_ar_Connect != null)
                m_ar_Connect.AsyncWaitHandle.Close();
            if (m_ClientSocket != null)
            {
                try
                {
                    m_ClientSocket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception e)
                {
                    m_Adapter.Debug(e.Message);
                }
                finally
                {
                    m_ClientSocket.Close();
                    m_ClientSocket = null;
                }
            }
            if (m_Reader != null)
                m_Reader.Reset();
            if (m_Writer != null)
                m_Writer.Reset();
        }
    }
}