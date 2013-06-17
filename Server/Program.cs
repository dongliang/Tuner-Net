using System;
using TNet.Server;
namespace Server
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			TServer server =TServer.Instance;
			server.Start();
			Console.Read();
		}
	}
}
