using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TNet.Server
{
    class TServer
    {
        Socket mSocket;
        List<Socket> mClients = new List<Socket>();
        public void Start()
        {
            mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mSocket.Bind(new IPEndPoint(IPAddress.Any, 9298));
            mSocket.Listen(4);
            mSocket.BeginAccept(AcceptCallback, mSocket);
            Console.WriteLine("Tuner Server Start!");
        }

        public void AcceptCallback(IAsyncResult ar)
        {

            Socket client = ((Socket)ar.AsyncState).EndAccept(ar);

            Console.WriteLine("client connect!");
            lock (mClients)
            {
                mClients.Add(client);
            }
           // MemoryStream temp_Buffer = new MemoryStream();
           // byte[] msgID_Bytes = BitConverter.GetBytes(5);
           // byte[] net_data_size_Bytes = BitConverter.GetBytes(4);
           // client.Send(temp_Buffer.GetBuffer());
        }


    }
}
