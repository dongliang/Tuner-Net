using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using TNet.Common;


namespace TNet.Server
{
    public class ClientAgent
    {
        Socket mClientSocket;
        TServer mServer;
        private IAsyncResult m_ar_Recv = null;
        private IAsyncResult m_ar_Send = null;
        public const int m_RecBufferSize = 65536;
        private byte[] m_RecBuffer = new byte[m_RecBufferSize];
        private Queue<byte[]> m_SendQueue = new Queue<byte[]>();
        public bool mIsRecving = false;
        int mRecvHead = 0;//point the first valid address 
        int mRecvTail = 0;//point to the last valid address 's next address.


        public ClientAgent(TServer server, Socket clientSocket)
        {
            mServer = server;
            mClientSocket = clientSocket;
        }

        public void Update()
        {

            if (!mIsRecving)
            {
                if (mServer.m_Reader != null)
                {
                    mRecvHead = mServer.m_Reader.DidReadData(m_RecBuffer, mRecvHead, mRecvTail, mServer.m_Adapter);
                }
                Receive();
            }

        }

        public void Receive()
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
                mIsRecving = true;
                m_ar_Recv = mClientSocket.BeginReceive(m_RecBuffer, mRecvTail, m_RecBufferSize - mRecvTail, 0, new AsyncCallback(ReceiveCallback), 0);

                mServer.m_Adapter.Debug("recving__" + mIsRecving.ToString());
            }
            catch (Exception e)
            {
                mServer.m_Adapter.Debug(e.Message);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                ar.AsyncWaitHandle.Close();
                m_ar_Recv = null;
                Socket client = mClientSocket;
                int bytesRead = client.EndReceive(ar);
                if (bytesRead > 0)
                {
                    mRecvTail += bytesRead;
                    mIsRecving = false;
                    mServer.m_Adapter.Debug("recving__" + mIsRecving.ToString());
                }
                else
                {
                    mServer.m_Adapter.Debug("Network Shutdown");
                    Receive();
                }
            }
            catch (Exception e)
            {
                mServer.m_Adapter.Debug(e.Message);
            }
        }

        public bool SendNetMessage<T>(int msgID, T data)
        {
            return _SendMessage(msgID, mServer.m_Writer.Serialize<T>(data));
        }

        public bool _SendMessage(int msgID, MemoryStream data)
        {
            if (mServer.m_Writer != null)
            {
                byte[] stream = mServer.m_Writer.MakeStream(msgID, data);
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
                m_ar_Send = mClientSocket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), 0);
                return true;
            }
            catch (Exception e)
            {
                mServer.m_Adapter.Debug(e.Message);
            }
            return false;
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                ar.AsyncWaitHandle.Close();
                m_ar_Send = null;
                Socket client = mClientSocket;
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
                mServer.m_Adapter.Debug(e.Message);
            }
        }

        public bool DisConnect()
        {
            ReleaseSocket();
            return true;
        }

        public void ReleaseSocket()
        {
            if (m_ar_Recv != null)
                m_ar_Recv.AsyncWaitHandle.Close();
            if (m_ar_Send != null)
                m_ar_Send.AsyncWaitHandle.Close();

            if (mClientSocket != null)
            {
                try
                {
                    mClientSocket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception e)
                {
                    mServer.m_Adapter.Debug(e.Message);
                }
                finally
                {
                    mClientSocket.Close();
                    mClientSocket = null;
                }
            }
        }
    }
}
