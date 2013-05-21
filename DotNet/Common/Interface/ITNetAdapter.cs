/*
   Tunner Net -  Data interception use ProtoBuf in game Development. 
   e-mail : dongliang17@126.com
   project: https://github.com/dongliang/Tuner-Net
*/
namespace TNet.Common
{
    public interface ITNetAdapter
    {
        void HandleMsg(TNetMsg msg);
        void Debug(object message);
        ITNetWriter GetMsgWriter();
        ITNetReader GetMsgReader();
    }
}