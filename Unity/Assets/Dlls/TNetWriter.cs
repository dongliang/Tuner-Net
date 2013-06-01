/*
   Tunner Net -  Data interception use ProtoBuf in game Development. 
   e-mail : dongliang17@126.com
   project: https://github.com/dongliang/Tuner-Net
*/
using System.IO;
using System.Net;
using System;
namespace TNet.Common
{

    public class TNetWriter : ITNetWriter
    {
        private MemoryStream m_Buffer = new MemoryStream();
        private byte[] sendBuffer = new byte[6];
        public byte[] MakeStream(int msgID, MemoryStream data)
        {


            m_Buffer.SetLength(0);
            int net_data_size = data != null ? (int)data.Length : 0;


            if (BitConverter.IsLittleEndian)
            {
                net_data_size = IPAddress.HostToNetworkOrder(net_data_size);
                msgID = IPAddress.HostToNetworkOrder(msgID);
            }

            byte[] msgID_Bytes =  BitConverter.GetBytes(msgID);
            byte[] net_data_size_Bytes = BitConverter.GetBytes(net_data_size);

            m_Buffer.Write(msgID_Bytes, 0, 4);
            m_Buffer.Write(net_data_size_Bytes, 0, 4);
            if (data != null)
                m_Buffer.Write(data.GetBuffer(), 0, (int)data.Length);
            return m_Buffer.ToArray();
        }

        public void Reset()
        {
            m_Buffer.SetLength(0);
        }

        private MemoryStream m_streamBuff = new MemoryStream();
        //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        public MemoryStream Serialize<T>(T instance)
        {
            m_streamBuff.SetLength(0);
         //   TunerSerializer temp = new TunerSerializer();
        //   temp.Serialize(m_streamBuff, instance); 
            return m_streamBuff;
        }
    }
}