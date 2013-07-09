using System;
using System.Net.Sockets;

using System.Collections.Generic;
using System.IO;

namespace Tuner.Net
{
	public class Agent
	{

		//common
		Socket mSocket = null;
		AgentHolder mHolder = null;
		public 	int mSessionId = 0;
		//recv
		public bool mIsRecving = false;
		IAsyncResult m_ar_Recv = null;
		public const int m_RecBufferSize = 65536;
		byte[] m_RecBuffer = new byte[m_RecBufferSize];
		int mRecvHead = 0;//point the first valid address 
		int mRecvTail = 0;//point to the last valid address 's next address.

		//send
		private IAsyncResult m_ar_Send = null;
		private Queue<byte[]> m_SendQueue = new Queue<byte[]> ();

		public Agent (AgentHolder holder, Socket socket, int sessionId)
		{
			mSessionId = sessionId;
			mHolder = holder;
			mSocket = socket;
		}

		public void Receive ()
		{
			try {
				if (m_RecBufferSize == mRecvTail) {
					System.Buffer.BlockCopy (m_RecBuffer, mRecvHead, m_RecBuffer, 0, mRecvTail - mRecvHead);
					mRecvTail -= mRecvHead;
					mRecvHead = 0;
				}
				
				if (m_RecBufferSize == mRecvTail) {
					throw new Exception ("Recieve buffer is full");
				}
				m_ar_Recv = mSocket.BeginReceive (m_RecBuffer, mRecvTail, m_RecBufferSize - mRecvTail, 0, new AsyncCallback (ReceiveCallback), 0);
				mIsRecving = true;
			} catch (Exception e) {
				mHolder.Adapter.Debug (e.Message);
			}
		}

		public void SetSocketSendNoDeley (bool nodelay)
		{
			if (mSocket != null) {
				mSocket.SetSocketOption (SocketOptionLevel.Tcp, SocketOptionName.NoDelay, nodelay ? 1 : 0);
			}
		}
		
		private void ReceiveCallback (IAsyncResult ar)
		{
			try {
				ar.AsyncWaitHandle.Close ();
				m_ar_Recv = null;
				Socket client = mSocket;
				int bytesRead = client.EndReceive (ar);
				if (bytesRead > 0) {
					mRecvTail += bytesRead;
					mIsRecving = false;
				} else {
					mHolder.Adapter.Debug ("Network Shutdown");
					mHolder.closeAgent (this);
				}
			} catch (Exception e) {
				mHolder.Adapter.Debug (e.Message);
			}
		}
				
		public void Update ()
		{
			UpdateRecv ();
		}

		void UpdateRecv ()
		{
			if (IsConnected () && !mIsRecving) {
				if (mHolder.Reader != null) {
					mRecvHead = mHolder.Reader.DidReadData (m_RecBuffer, mRecvHead, mRecvTail, mHolder.Adapter.HandleMsg, mSessionId);
				}
				Receive ();
			}
		}

		public bool IsConnected ()
		{
			return mSocket != null ? mSocket.Connected : false;
		}

		public bool SendMessage (int msgID, MemoryStream data)
		{
			if (mHolder.Writer != null) {
				byte[] stream = mHolder.Writer.MakeStream (msgID, data);
				lock (m_SendQueue) {
					if (m_SendQueue.Count == 0) {
						return Send (stream);
					} else {
						m_SendQueue.Enqueue (stream);
						return true;
					}
				}
			}
			return false;
		}
		
		private bool Send (byte[] byteData)
		{
			try {
				m_ar_Send = mSocket.BeginSend (byteData, 0, byteData.Length, 0, new AsyncCallback (SendCallback), 0);
				return true;
			} catch (Exception e) {
				mHolder.Adapter.Debug (e.Message);
			}
			return false;
		}
		
		private void SendCallback (IAsyncResult ar)
		{
			try {
				ar.AsyncWaitHandle.Close ();
				m_ar_Send = null;
				Socket client = mSocket;
				client.EndSend (ar);
				lock (m_SendQueue) {
					if (m_SendQueue.Count > 0) {
						Send (m_SendQueue.Dequeue ());
					}
				}
			} catch (Exception e) {
				mHolder.Adapter.Debug (e.Message);
			}
		}

		public void Release ()
		{
			if (m_ar_Recv != null)
				m_ar_Recv.AsyncWaitHandle.Close ();
			if (m_ar_Send != null)
				m_ar_Send.AsyncWaitHandle.Close ();
						
			if (mSocket != null) {
				try {
					mSocket.Shutdown (SocketShutdown.Both);
				} catch (Exception e) {
					mHolder.Adapter.Debug (e.Message);
				} finally {
					mSocket.Close ();
					mSocket = null;
				}
			}
		}













	}
}
