using System;
using System.Collections.Generic;
using System.Text;
using TNet.Server;

    class Program
    {
        static void Main(string[] args)
        {
            TServer server =TServer.Instance;
            server.Start();
            Console.Read();
        }
    }