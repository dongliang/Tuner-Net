using TNet.Common;
using UnityEngine;
public class DP_TNet_Adapter : ITNetAdapter
{
    public void HandleMsg(System.Object state, TNetMsg msg)
    {
        
        //send the message to the particular message system.

        UnityEngine.Debug.Log(msg.m_nMsgID.ToString() + msg.DeSerializeProtocol<TunerMessage.PBString>().str_value);
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