
using Tuner.Net;
using Tuner;
using System.IO;

public class Tuner_TNet_Adapter : ITNetAdapter
{
    public void HandleMsg(System.Object state, TNetMsg msg)
    {

		TunerMessage.TMLogin temp = TunerMessage.TMLogin.Deserialize (msg.m_DataMsg);
        System.Console.WriteLine(msg.m_nMsgID.ToString() +"__"+ temp.Username + "____" + temp.Password);
        //send the message to the particular message system.


        //test send
        Agent tempClient = Server.Instance.getClient((int)state);
        TunerMessage.TMLoginOut tempMsg = new TunerMessage.TMLoginOut();
        tempMsg.Nickname = "yyss";
		MemoryStream mem = new MemoryStream ();
		TunerMessage.TMLoginOut.Serialize(mem,tempMsg);

		tempClient.SendMessage(7,mem); 
        

    }
    public void Debug(object message)
    {
        System.Console.WriteLine(message);        
    }

	public ITNetWriter createMsgWriter()
    {

        return new TNetWriter();
     //   return new Common.

    }
	public ITNetReader createMsgReader()
    {
        return new TNetReader();
        //return new Tnet();
    }

	
	public void onConnected (){}
	
	public void onAccepted (){}
	
	public void onReConnected (){}
	
	public void onNetWorkFailed (){}
	
	public void onReConnectBegin (){}
}