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

// 2017-03-07 21:14:17

// server uses UTC time 
using UnityEngine.Rendering;

public class FeedPoopCtrl : MonoBehaviour
{
	// JSON
	public JSONNode N;
	public string nextCleanTime;
	public string nextFeedTime;
	public string level;
	public string cleanlinessLevel;
	public string healthLevel;


	// API URL
	public string url = "http://104.131.144.86/api/blobs/";
	public int blobId0 = 1;
	//public int blobId1 = 5;
	public string blobName0 = "";
	public string blobName1 = "";

	public string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOjEsImlzcyI6Imh0dHA6XC9cLzEwNC4xMzEuMTQ0Ljg2XC9hcGlcL3VzZXJzXC9hdXRoZW50aWNhdGUiLCJpYXQiOjE0ODkwNDE5NTAsImV4cCI6MTQ4OTA0NTU1MCwibmJmIjoxNDg5MDQxOTUwLCJqdGkiOiI2M2JjOTQxZWJjMTc1MDUwODE2Yzk0NzI4MzczZDU1ZiJ9.HcU5milX-vz2lMgP-JQXoiMUvjrTA6kiA5BMEXO3vZY";

	public Image imgPoop;
	public Image imgHam;
	public Image imgThoughtBub;

	public bool needsCleaning = false;
	public bool needsFeeding = false;

	// Use this for initialization
	void Start ()
	{

		//TokenCtrl tokenCtrl = new TokenCtrl ();
		//token = tokenCtrl.token;
		Debug.Log ("TOKEN: " + token);

		imgPoop.enabled = false;
		imgHam.enabled = false;
		imgThoughtBub.enabled = false;

		GetBlob ();
	}


	
	// Update is called once per frame
	void Update ()
	{

		
	}

	public void GetBlob() {
		WWW www = new WWW (url + blobId0.ToString ());
		StartCoroutine (GetBlobInfo (www));
	}
		

	IEnumerator GetBlobInfo (WWW www)
	{
		yield return www;

		// check for errors
		if (www.error == null) {
			N = JSON.Parse (www.text);
			ParseJson ();
			CompareTimes ();
			Debug.Log ("GetBlobInfo OK: " + www.text);
		} else {
			Debug.Log ("WWW Error: " + www.error);
		}    
	}

	public void ParseJson ()
	{
		nextCleanTime = N ["next_cleanup_time"].Value;
		nextFeedTime = N ["next_feed_time"].Value;
		cleanlinessLevel = N ["cleanliness_level"].Value;
		healthLevel = N ["health_level"].Value;
		blobName0 = N ["name"].Value;


		Debug.Log ("nextCleanTime: " + nextCleanTime);
		Debug.Log ("nextFeedTime: " + nextFeedTime);
		Debug.Log ("cleanliness level: " + cleanlinessLevel);
		Debug.Log ("health level: " + healthLevel);
			}


	public void CompareTimes ()
	{
		
		DateTime dateTime = DateTime.Now.ToUniversalTime ();
		Debug.Log ("Current time in UTC: " + dateTime.ToString ());

		DateTime cleanTime = Convert.ToDateTime (nextCleanTime);

		DateTime feedTime = Convert.ToDateTime (nextFeedTime);

		int cleanComp = DateTime.Compare (dateTime, cleanTime);
		int feedComp = DateTime.Compare (dateTime, feedTime);

		if (cleanComp < 0) {
			// dateTime is earlier than cleanTime

			imgPoop.enabled = false;
		} else if (cleanComp == 0 || cleanComp > 0) {
			// == 0 : dateTime same as CleanTime
			// > 0 : dateTime is later than cleanTime

			// print poop img

			Debug.Log ("PRINTING POOP...");

			needsCleaning = true;
			imgPoop.enabled = true;
		}

		if (feedComp < 0) {
			// dateTime is earlier than cleanTime
			imgThoughtBub.enabled = false;
			imgHam.enabled = false;
		} else if (feedComp == 0 || cleanComp > 0) {
			// == 0 : dateTime same as CleanTime
			// > 0 : dateTime is later than cleanTime
			Debug.Log ("PRINTING FOOD...");
			needsFeeding = true;
			imgThoughtBub.enabled = true;
			Invoke ("EnableHam", 1);
		}
	}



	public void FeedButtonClicked ()
	{
		// do something if blob needs feeding
		if (needsFeeding) {
			Debug.Log ("Feed button clicked.");
			StartCoroutine (UpdateBlob ("feed"));
	
		}
	}

	public void CleanButtonClicked ()
	{
		// do something if blob needs cleaning
		if (needsCleaning) {
			Debug.Log ("Clean button clicked.");
			StartCoroutine (UpdateBlob ("clean"));
		}
	}

	// PUT request
	IEnumerator UpdateBlob (string button)
	{
		string myData =" ";
		//byte[] myData = new byte[1];
		string finalUrl;

		if (button == "feed") {
			//myData = System.Text.Encoding.UTF8.GetBytes ("/token=" + token + "health_level=" + healthLevel);
			finalUrl = url + blobId0 + "?token=" + token + "&health_level=" + healthLevel;
			//myData = "?token=123" + "&health_level=" + healthLevel;
			//Debug.Log (myData);
		} else {
			//myData = System.Text.Encoding.UTF8.GetBytes ("/token=" + token + "cleanliness_level=" + cleanlinessLevel);
			finalUrl = url + blobId0 + "?token=" + token + "&cleanliness_level=" + cleanlinessLevel;
			//myData = "/token=" + token + "cleanliness_level=" + cleanlinessLevel;
		}

		using (UnityWebRequest www = UnityWebRequest.Put (finalUrl, myData)) {
		//using (UnityWebRequest www = UnityWebRequest.Put (url + blobId0.ToString (), myData)) {
			yield return www.Send ();

			if (www.isError) {
				Debug.Log ("PUT ERROR: " + www.error);
			} else {
				Debug.Log ("PUT REQUEST SUCCESSFUL.");
				Debug.Log (www.url.ToString ());

				// get next clean and/or feed times

				if (button == "feed") {
					needsFeeding = false;
					imgHam.enabled = false;
					Invoke ("DisableBubble", 1);


				} else if (button == "clean") {
					needsCleaning = false;
					imgPoop.enabled = false;
				}

				//GetBlob ();
			}
	
		}
	}

	// show ham img
	public void EnableHam ()
	{
		imgHam.enabled = true;
	}

	// hide thought bubble img
	public void DisableBubble ()
	{
		imgThoughtBub.enabled = false;
	}




}
