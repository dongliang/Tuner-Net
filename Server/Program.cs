using System;
using Tuner.Net;

class MainClass
{
	public static void Main (string[] args)
	{
		ITNetAdapter adapter = new Tuner_TNet_Adapter ();
		Server server = Server.Instance;
		server.Init(adapter);
		server.Start ();
		Console.Read ();
	}
}