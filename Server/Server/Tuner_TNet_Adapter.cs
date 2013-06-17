using TNet;
using TNet.Common;
using TNet.Server;
using System.IO;

public class Tuner_TNet_Adapter : ITNetAdapter
{
    public void HandleMsg(System.Object state, TNetMsg msg)
    {

		TunerMessage.TMLogin temp = TunerMessage.TMLogin.Deserialize (msg.m_DataMsg);
        System.Console.WriteLine(msg.m_nMsgID.ToString() +"__"+ temp.Username + "____" + temp.Password);
        //send the message to the particular message system.

        //test send
        ClientAgent tempClient = TServer.Instance.getClient((int)state);
        TunerMessage.TMLoginOut tempMsg = new TunerMessage.TMLoginOut();
        tempMsg.Nickname = "yyss";
		MemoryStream mem = new MemoryStream ();
		TunerMessage.TMLoginOut.Serialize(mem,tempMsg);

		tempClient.SendNetMessage(7,mem);
        

    }
    public void Debug(object message)
    {
        System.Console.WriteLine(message);        
    }

    public ITNetWriter GetMsgWriter()
    {

        return new TNet.Common.TNetWriter();
     //   return new Common.

    }
    public ITNetReader GetMsgReader()
    {
        return new TNet.Common.TNetReader();
        //return new Tnet();
    }
}