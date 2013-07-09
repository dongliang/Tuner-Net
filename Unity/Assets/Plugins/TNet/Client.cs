using System;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace Tuner.Net
{
		public class Client:AgentHolder
		{
				string m_strRomoteIP = "127.0.0.1";
				ushort m_uRemotePort = 0;
				Agent mMainAgent = null;
				bool isReConnect = false;
				protected static Client instance = new Client ();

				public static Client Instance {
						get { return instance; }
				}

				public void Connect (string a_strRomoteIP, ushort a_uPort)
				{
						if (mMainAgent != null) {

								closeAgent (mMainAgent);
						}

						try {
								Socket clientSocket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
								IPAddress ip = IPAddress.Parse (a_strRomoteIP);
								clientSocket.BeginConnect (ip, a_uPort, new AsyncCallback (ConnectCallback), clientSocket);
								m_strRomoteIP = a_strRomoteIP;
								m_uRemotePort = a_uPort;

						} catch (Exception e) {
					
								mAdapter.Debug (e.Message);
						}
				}

				private void ConnectCallback (IAsyncResult ar)
				{
						try {
								ar.AsyncWaitHandle.Close ();

								Socket socket = (Socket)ar.AsyncState;
								socket.EndConnect (ar);
								socket.Blocking = false;
								mMainAgent = new Agent (this, socket, 0);
								mMainAgent.Receive ();

								if (!isReConnect) {
										mAdapter.onConnected ();
								} else {
										Adapter.onReConnected ();
										isReConnect = false;
								}
						} catch (Exception e) {
								
								mAdapter.Debug (e.Message);
								mAdapter.onNetWorkFailed ();
						}
				}

				public override void Update ()
				{
						base.Update ();
						if (mMainAgent != null) {
								mMainAgent.Update ();
						}
					
				}

				public bool ReConnect ()
				{
						if (m_strRomoteIP != null) {
								DisConnect ();
								
								isReConnect = true;
								Adapter.onReConnectBegin ();
								//First Release Socket Resource
								Connect (m_strRomoteIP, m_uRemotePort);
								return true;
						} else {
								return false;
						}
				}

				public bool IsConnected ()
				{
						return mMainAgent != null ? mMainAgent.IsConnected () : false;
				}

				public override void closeAgent (Agent agent)
				{
						if (agent != null) {
								base.closeAgent (agent);
								mMainAgent = null;
						}
				}

				public bool SendMessage (int msgID, MemoryStream data)
				{
						return mMainAgent.SendMessage (msgID, data);
				}

				public void DisConnect ()
				{
						closeAgent (mMainAgent);
				}
				
				public void SetSocketSendNoDeley (bool nodelay)
				{
						mMainAgent.SetSocketSendNoDeley (nodelay);
				}
		}
}