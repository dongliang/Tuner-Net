using UnityEngine;
using System.Collections;
using Tuner.Net;
using System.IO;

public class Test : MonoBehaviour
{

		// Use this for 
		int id;

		void Awake ()
		{
				id = Random.Range (1, 10);
				Client.Instance.Init (new DP_TNet_Adapter ());
				Client.Instance.Connect ("127.0.0.1", 9298);
      
        
		}

		void Start ()
		{
				if (Client.Instance.IsConnected ()) {
						testSend ();
				}
		}

		void testSend ()
		{
// 
//  TunerMessage.TMLogin temp = new TunerMessage.TMLogin();
//  temp.username = "cotngf";
//   temp.password = "dlkeyf";
//    TNetMgr.Instance.SendNetMessage<TunerMessage.TMLogin>(1, temp);

				TunerMessage.TMLogin temp = new TunerMessage.TMLogin ();
				temp.Username = "cotngf1";
				temp.Password = "dlkeyf1";

				MemoryStream tempStream = new MemoryStream ();
				TunerMessage.TMLogin.Serialize (tempStream, temp);



				Client.Instance.SendMessage (1, tempStream);
        
				// TunerMessage.TMLogin.Serialize()
		}
	
		// Update is called once per frame
		void Update ()
		{
				Client.Instance.Update ();
				//
		}
}
