using TNet;
using TNet.Common;

public class Tuner_TNet_Adapter : ITNetAdapter
{
    public void HandleMsg(TNetMsg msg)
    {
      TunerMessage.PBString temp =  msg.DeSerializeProtocol<TunerMessage.PBString>();
        System.Console.WriteLine(msg.m_nMsgID.ToString() + temp.str_value);
        //send the message to the particular message system.
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