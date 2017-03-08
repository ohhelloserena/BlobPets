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

public class FeedPoopCtrl : MonoBehaviour
{
	// JSON
	public JSONNode N;
	public string nextCleanTime;
	public string nextFeedTime;

	// API URL
	public string url = "http://104.131.144.86/api/blobs/";
	public int blobId0 = 1;
	//public int blobId1 = 5;
	public string blobName0 = "";
	public string blobName1 = "";

	public Image imgPoop;
	public Image imgHam;
	public Image imgThoughtBub;

	public bool needsCleaning = false;
	public bool needsFeeding = false;

	// Use this for initialization
	void Start ()
	{
		imgPoop.enabled = false;
		imgHam.enabled = false;
		imgPoop.enabled = false;

		WWW www = new WWW (url + blobId0.ToString ());
		StartCoroutine (GetBlobInfo (www));

	}


	
	// Update is called once per frame
	void Update ()
	{

		
	}

	IEnumerator GetBlobInfo (WWW www)
	{
		yield return www;

		// check for errors
		if (www.error == null) {
			N = JSON.Parse (www.text);
			ParseJson ();
			CompareTimes ();
			Debug.Log ("WWW Ok!: " + www.text);
		} else {
			Debug.Log ("WWW Error: " + www.error);
		}    
	}

	public void ParseJson ()
	{
		nextCleanTime = N ["next_cleanup_time"].Value;
		nextFeedTime = N ["next_feed_time"].Value;

		blobName0 = N ["name"].Value;


		Debug.Log ("nextCleanTime: " + nextCleanTime);
		Debug.Log ("nextFeedTime: " + nextFeedTime);
	}


	public void CompareTimes ()
	{
		
		DateTime dateTime = DateTime.Now;
		Debug.Log ("CURRENT!! " + dateTime.ToString ());

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

			// print thought bubble img, then print ham img one second later

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
		byte[] myData = System.Text.Encoding.UTF8.GetBytes (blobName0);
		using (UnityWebRequest www = UnityWebRequest.Put (url + blobId0.ToString (), myData)) {
			yield return www.Send ();

			if (www.isError) {
				Debug.Log ("PUT ERROR: " + www.error);
			} else {
				Debug.Log ("Uploaded!!");

				if (button == "feed") {
					needsFeeding = false;
					imgHam.enabled = false;
					Invoke ("DisableBubble", 1);


				} else if (button == "clean") {
					needsCleaning = false;
					imgPoop.enabled = false;
				}
			}
	
		}
	}

	// show ham img
	public void EnableHam ()
	{
		imgHam.enabled = true;
	}

	// hide ham img
	public void DisableBubble ()
	{
		imgThoughtBub.enabled = false;
	}




}
