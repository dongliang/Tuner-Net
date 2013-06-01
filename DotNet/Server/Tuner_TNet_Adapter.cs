using TNet;
using TNet.Common;
using TNet.Server;

public class Tuner_TNet_Adapter : ITNetAdapter
{
    public void HandleMsg(System.Object state, TNetMsg msg)
    {
        TunerMessage.TMLogin temp = msg.DeSerializeProtocol<TunerMessage.TMLogin>();
        System.Console.WriteLine(msg.m_nMsgID.ToString() +"__"+ temp.username + "____" + temp.password);
        //send the message to the particular message system.

        //test send
        ClientAgent tempClient = TServer.Instance.getClient((int)state);
        TunerMessage.TMLoginOut tempMsg = new TunerMessage.TMLoginOut();
        tempMsg.nickname = "33";

        tempClient.SendNetMessage<TunerMessage.TMLoginOut>(7,tempMsg);
        

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