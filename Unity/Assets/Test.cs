using UnityEngine;
using System.Collections;
using TNet.Client;

public class Test : MonoBehaviour {

	// Use this for 
    int id;
    void Awake()
    {
        id = Random.Range(1, 10);
        TNetMgr.Instance.Init(new DP_TNet_Adapter());
        TNetMgr.Instance.Connect("127.0.0.1", 9298);
      

    }
	void Start () {
        if (TNetMgr.Instance.IsConnected())
        {
            testSend();
        }
	}

    void testSend()
    {

        TunerMessage.TMLogin temp = new TunerMessage.TMLogin();
        temp.username = "cotngf";
        temp.password = "dlkeyf";
        TNetMgr.Instance.SendNetMessage<TunerMessage.TMLogin>(1, temp);
        
    }
	
	// Update is called once per frame
	void Update () {
        TNetMgr.Instance.Update();
      //
	}
}
