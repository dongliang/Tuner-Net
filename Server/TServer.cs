using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TNet.Common;
using System.Threading;

namespace TNet.Server
{
    public class TServer:Singleton<TServer>
    {
        Socket mSocket;
        Dictionary<int,ClientAgent> mClientAgents = new Dictionary<int,ClientAgent>();

        public ITNetReader m_Reader = null;
        public ITNetWriter m_Writer = null;
        public ITNetAdapter m_Adapter = null;
        public int mSessionNumber = 0;
        public List<int> mClosedAgents = new List<int>();
        public void Start()
        {
            m_Reader = new TNetReader();
            m_Writer = new TNetWriter();
            m_Adapter = new Tuner_TNet_Adapter();
            mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mSocket.Bind(new IPEndPoint(IPAddress.Any, 9298));
            mSocket.Listen(4);
            mSocket.BeginAccept(AcceptCallback, mSocket);
            Console.WriteLine("Tuner Server Start!");
            loop();
        }

        public ClientAgent getClient(int sessionId)
        {
            ClientAgent temp;
            mClientAgents.TryGetValue(sessionId, out temp);
            return temp;
        }



        int generateSessionID()
        {
            return ++mSessionNumber;
        }

        public void loop()
        {
            while (true)
            {
                Thread.Sleep(100);
                Update();
                foreach (int item in mClosedAgents)
                {
                    mClientAgents[item].DisConnect();
                    mClientAgents.Remove(item);
                }
                mClosedAgents.Clear();
            }
        }

        public void Update()
        {
            foreach (KeyValuePair<int, ClientAgent> item in mClientAgents)
            {
                item.Value.Update();
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            int sessionId = generateSessionID();
            Socket client = ((Socket)ar.AsyncState).EndAccept(ar);
            ClientAgent agent = new ClientAgent(this, client, sessionId);
            agent.Receive();
            Console.WriteLine("client connect!");
            lock (mClientAgents)
            {
                mClientAgents.Add(sessionId,agent);
            }
            mSocket.BeginAccept(AcceptCallback, mSocket);
        }
    }
}
