/*
   Tunner Net -  Data interception use ProtoBuf in game Development. 
   e-mail : dongliang17@126.com
   project: https://github.com/dongliang/Tuner-Net
*/
namespace TNet
{
    public interface ITNetAdapter
    {
        void SendMsg(TNetMsg msg);
    }
}