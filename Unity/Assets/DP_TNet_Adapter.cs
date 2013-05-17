using TNet.Common;
using UnityEngine;
public class DP_TNet_Adapter : ITNetAdapter
{
    public void HandleMsg(TNetMsg msg)
    {
        //send the message to the particular message system.
    }
    public void Debug(object message)
    {
        UnityEngine.Debug.Log(message);
    }
}