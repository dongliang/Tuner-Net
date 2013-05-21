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
      
	}

    void testSend()
    {
     
        TunerMessage.PBString temp = new TunerMessage.PBString();
        temp.str_value =id.ToString() + "___"+ Time.time.ToString();
        if (TNetMgr.Instance == null)
        {
            Debug.Log(222);
        }
        TNetMgr.Instance.SendNetMessage<TunerMessage.PBString>(6,temp);
        Debug.Log(temp.str_value);
    }
	
	// Update is called once per frame
	void Update () {
      if (TNetMgr.Instance.IsConnected())
      {
          testSend();
      }
      // 
	
	}
}
