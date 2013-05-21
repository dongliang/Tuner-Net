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
    public class TServer
    {
        Socket mSocket;
        List<ClientAgent> mClientAgents = new List<ClientAgent>();

        public ITNetReader m_Reader = null;
        public ITNetWriter m_Writer = null;
        public ITNetAdapter m_Adapter = null;
        public List<ClientAgent> mClosedAgents = new List<ClientAgent>();
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

        public void loop()
        {
            while (true)
            {
                Thread.Sleep(100);
                Update();
                foreach (ClientAgent item in mClosedAgents)
                {
                    item.DisConnect();
                    mClientAgents.Remove(item);
                }
                mClosedAgents.Clear();
            }
        }

        public void Update()
        {
            foreach (ClientAgent item in mClientAgents)
            {
                item.Update();
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {

            Socket client = ((Socket)ar.AsyncState).EndAccept(ar);
            ClientAgent agent = new ClientAgent(this, client);
            agent.Receive();
            Console.WriteLine("client connect!");
            lock (mClientAgents)
            {

                mClientAgents.Add(agent);
            }
            mSocket.BeginAccept(AcceptCallback, mSocket);
        }
    }
}
