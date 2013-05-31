/*
   Tunner Net -  Data interception use ProtoBuf in game Development. 
   e-mail : dongliang17@126.com
   project: https://github.com/dongliang/Tuner-Net
*/
using System;
using System.IO;
using System.Net;
namespace TNet.Common
{

    //Buffer
    public class TNetReader : ITNetReader
    {

        /// <summary>
        /// read the data and send message to the client adapter.
        /// </summary>
        /// <param name="data">buffer</param>
        /// <param name="size">size</param>
        /// <param name="start">start</param>
        /// <returns>new start</returns>
        public int DidReadData(byte[] data, int start, int tail, ITNetAdapter adapter,System.Object state)
        {
            int size = tail - start;
            int msg_id = 0;
            int data_size = 0;


            if (size > 8)
            {
                //read message head
                msg_id = BitConverter.ToInt32(data, start);
                data_size = BitConverter.ToInt32(data, start + 4);

                if (BitConverter.IsLittleEndian)
                {
                    msg_id = IPAddress.NetworkToHostOrder(msg_id);
                    data_size = IPAddress.NetworkToHostOrder(data_size);
                }
            }
            else
            {
                return start;
            }

            //check body
            if (size >= data_size + 8)
            {
                //send message to adapter

                //create message
                TNetMsg msg = new TNetMsg();

                MemoryStream msg_data_body = new MemoryStream();
                msg_data_body.SetLength(0);
                msg_data_body.Write(data, start + 8, data_size);

                msg.m_nMsgID = msg_id;
                msg.m_DataMsg = msg_data_body;

                adapter.HandleMsg(state,msg);
            }
            else
            {
                return start;
            }

            //recurrence passing the new start address.
            return DidReadData(data, start + data_size + 8, tail, adapter, state);
        }

        public void Reset()
        {

        }
    }
}