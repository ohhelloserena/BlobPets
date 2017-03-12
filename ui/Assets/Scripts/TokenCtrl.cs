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

public class TokenCtrl : MonoBehaviour
{

	public string token = "";
	public string userId = "1";
	public string email = "ryanchenkie@gmail.com";
	public string password = "secret";

	// Use this for initialization
	void Start ()
	{
		GetToken ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void GetToken ()
	{
		string tokenUrl = "http://104.131.144.86/api/users/authenticate";
		WWWForm form = new WWWForm ();
		form.AddField ("email", email);
		form.AddField ("password", password);
		WWW www = new WWW (tokenUrl, form);
		StartCoroutine (WaitForRequest (www));
	}

	IEnumerator WaitForRequest (WWW www)
	{
		yield return www;

		// check for errors
		if (www.error == null) {
			//Debug.Log ("Token: " + www.text);
			JSONNode N = JSON.Parse (www.text);
			ParseJson (N);


		} else {
			Debug.Log ("WWW Error: " + www.error);
			if (www.error == "400 Bad Request") {
				// alert for duplicate email address
			}
		}    
	}

	public void ParseJson(JSONNode data) 
	{
		token = data ["token"].Value;
		//Debug.Log ("Parsed, token is: " + token);
	}

	/*
	public string GetBlobId ()
	{
		FeedPoopCtrl fpc = new FeedPoopCtrl ();
		int blobId = fpc.blobId0;
		return blobId.ToString ();
	}
	*/


}
