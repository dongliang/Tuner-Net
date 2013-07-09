/*
   Tunner Net -  Data interception use ProtoBuf in game Development. 
   e-mail : dongliang17@126.com
   project: https://github.com/dongliang/Tuner-Net
*/
namespace Tuner.Net
{
		public delegate void CallBackEvtDel ();

		public interface ITNetAdapter
		{
				void HandleMsg (System.Object state, TNetMsg msg);

				void Debug (object message);

				ITNetWriter createMsgWriter ();

				ITNetReader createMsgReader ();
				
				void onConnected ();

				void onAccepted ();
				
				void onReConnected ();
				
				void onNetWorkFailed ();

				void onReConnectBegin ();
		}
}