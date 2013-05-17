using UnityEngine;
using System.Collections;
using TNet.Client;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {

        TNetMgr.Instance.Init(new DP_TNet_Adapter());
        TNetMgr.Instance.Connect("127.0.0.1", 9298);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
