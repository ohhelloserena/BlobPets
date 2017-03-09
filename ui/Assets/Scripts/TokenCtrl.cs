using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using SimpleJSON;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System.ComponentModel;
using System.Runtime.Remoting;
using UnityEngine.Networking;

public class TokenCtrl : MonoBehaviour {

	public int blobId; 
	public string token;
	public string url = "http://104.131.144.86/api/blobs/";

	// Use this for initialization
	void Start () {
		FeedPoopCtrl fpc = new FeedPoopCtrl();
		blobId = fpc.blobId0;
		string strBlobId = blobId.ToString();

		WWW www = new WWW (url + strBlobId); 
		StartCoroutine (GetToken (www, blobId));

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator GetToken(WWW www, int blobId) {
		yield return www;

		if (www.error == null) {
			
			Debug.Log ("WWW Ok!: " + www.text);
		} else {
			Debug.Log ("WWW Error: " + www.error);
		}    
		



	}
}
