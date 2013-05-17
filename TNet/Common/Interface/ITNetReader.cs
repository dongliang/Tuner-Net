/*
   Tunner Net -  Data interception use ProtoBuf in game Development. 
   e-mail : dongliang17@126.com
   project: https://github.com/dongliang/Tuner-Net
*/
namespace TNet.Common
{
    public interface ITNetReader
    {
        /// <summary>
        /// read the data and send message to the client adapter.
        /// </summary>
        /// <param name="data">buffer</param>
        /// <param name="size">size</param>
        /// <param name="start">start</param>
        /// <returns>new start</returns>
        int DidReadData(byte[] data, int start, int tail, ITNetAdapter adapter);
        void Reset();
        
    }
}
