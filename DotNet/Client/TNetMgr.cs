/*
   Tunner Net -  Data interception use ProtoBuf in game Development. 
   e-mail : dongliang17@126.com
   project: https://github.com/dongliang/Tuner-Net
*/
using TNet.Common;
namespace TNet.Client
{
    public class TNetMgr:Singleton<TNetMgr>
    {
        TNetCtrl m_Ctrl = null;
        public TNetMgr()
        {
            m_Ctrl =  new TNetCtrl();
        }
        public void Init(ITNetAdapter adapter)
        {
            m_Ctrl.m_Adapter = adapter;

        }
        public void Destroy()
        {

        }

        public void Update()
        {
            m_Ctrl.Update();
        }

        public bool Connect(string a_strRomoteIP, ushort a_uPort)
        {
           return  Connect(a_strRomoteIP, a_uPort, "", 0);
        }

        public bool Connect(string a_strRomoteIP, ushort a_uPort, string securityServer, ushort securityServerPort)
        {
            bool bResult = true;
#if UNITY_WEBPLAYER
		bResult = Security.PrefetchSocketPolicy(securityServer, securityServerPort);
#endif
            return bResult && m_Ctrl.Connect(a_strRomoteIP, a_uPort);
        }

        public bool DisConnect()
        {
            return m_Ctrl.DisConnect();
        }

        public bool SendNetMessage<T>(int msgID, T data)
        {
           return m_Ctrl.SendNetMessage<T>(msgID, data);
        }

        public void Close()
        {
            m_Ctrl.ReleaseSocket();
        }

        public void SetSocketSendNoDeley(bool nodelay)
        {
            m_Ctrl.SetSocketSendNoDeley(nodelay);
        }
    }
}