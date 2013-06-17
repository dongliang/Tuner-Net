using TNet.Common;

public class DP_TNet_Adapter : ITNetAdapter
{
    public void HandleMsg(System.Object state, TNetMsg msg)
    {
        
        //send the message to the particular message system.
//         TunerSerializer temp = new TunerSerializer();
//         TunerMessage.PBString str;
//         str = temp.Deserialize(msg.m_DataMsg, null, typeof(TunerMessage.PBString)) as TunerMessage.PBString;
        
      //  UnityEngine.Debug.Log(msg.m_nMsgID.ToString() +str.str_value);



       TunerMessage.TMLoginOut temp =  TunerMessage.TMLoginOut.Deserialize(msg.m_DataMsg);

       UnityEngine.Debug.Log(temp.Nickname); 
       
        
    }
    public void Debug(object message)
    {
        UnityEngine.Debug.Log(message);
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