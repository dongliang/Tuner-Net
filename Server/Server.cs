using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Tuner.Net;
using Tuner;
using System.Threading;

namespace Tuner.Net
{
	public class Server:AgentHolder
	{
		Socket mSocket;
		Dictionary<int,Agent> mClientAgents = new Dictionary<int,Agent> ();
		public int mSessionNumber = 0;
		public List<int> mClosedAgents = new List<int> ();


		protected static Server instance = new Server();
		public static Server Instance
		{
			get { return instance; }
		}


		public override void Init (ITNetAdapter adapter)
		{
			base.Init (adapter);
		}

		public void Start ()
		{
			mSocket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			mSocket.Bind (new IPEndPoint (IPAddress.Any, 9298));
			mSocket.Listen (4);
			mSocket.BeginAccept (AcceptCallback, mSocket);
			Console.WriteLine ("Tuner Server Start!");
			loop ();
		}

		public Agent getClient (int sessionId)
		{
			Agent temp;
			mClientAgents.TryGetValue (sessionId, out temp);
			return temp;
		}

		int generateSessionID ()
		{
			return ++mSessionNumber;
		}

		public void loop ()
		{
			while (true) {
				Thread.Sleep (100);
				Update ();
			}
		}

		public override void Update ()
		{
			base.Update ();
			foreach (KeyValuePair<int, Agent> item in mClientAgents) {
				item.Value.Update ();
			}
			foreach (int item in mClosedAgents) {
				mClientAgents [item].Release ();
				mClientAgents.Remove (item);
				Adapter.Debug ("delete" + item.ToString ());
			}
			mClosedAgents.Clear ();
		}

		public void AcceptCallback (IAsyncResult ar)
		{
			int sessionId = generateSessionID ();
			Socket client = ((Socket)ar.AsyncState).EndAccept (ar);
			Agent agent = new Agent (this, client, sessionId);
			agent.Receive ();
			Console.WriteLine ("client connect!");
			lock (mClientAgents) {
				mClientAgents.Add (sessionId, agent);
			}
			mSocket.BeginAccept (AcceptCallback, mSocket);
		}

		public override void closeAgent (Agent agent)
		{
			base.closeAgent (agent);
			lock (mClosedAgents) {
				mClosedAgents.Add (agent.mSessionId);
			}

		}
	}
}
