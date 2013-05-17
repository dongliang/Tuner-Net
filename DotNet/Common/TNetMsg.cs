/*
   Tunner Net -  Data interception use ProtoBuf in game Development. 
   e-mail : dongliang17@126.com
   project: https://github.com/dongliang/Tuner-Net
*/

using System.IO;
namespace TNet.Common
{
    public class TNetMsg
    {
        public int m_nMsgID = 0;
        public MemoryStream m_DataMsg;
        //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        public T DeSerializeProtocol<T>()
        {
            m_DataMsg.Position = 0;
            return ProtoBuf.Serializer.Deserialize<T>(m_DataMsg);
        }
    }
}