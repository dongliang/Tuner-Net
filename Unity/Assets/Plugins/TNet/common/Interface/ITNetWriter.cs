/*
   Tunner Net -  Data interception use ProtoBuf in game Development. 
   e-mail : dongliang17@126.com
   project: https://github.com/dongliang/Tuner-Net
*/
using System.IO;
namespace Tuner.Net
{
    public interface ITNetWriter
    {
        byte[] MakeStream(int msgID, MemoryStream data);

        void Reset();
        MemoryStream Serialize<T>(T instance);
    }
}
