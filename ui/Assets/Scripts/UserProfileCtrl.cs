using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using SimpleJSON;
using UnityEditor.Sprites;
using System;

public class UserProfileCtrl : MonoBehaviour {

	public string url = "http://104.131.144.86/api/users/";
	public int userId = 4;
	public string username = "-1";
	public string numWins = "-1";
	public int numBlobs = -1;

	public string blobName0;
	public string blobName1;
	public string blobName2;
	public string blobName3;


	public int blobId0 = -1;
	public int blobId1 = -1;
	public int blobId2 = -1;
	public int blobId3 = -1;

	public JSONNode result;

	public Text nameLabel;
	public Text winsLabel;
	public Text blobCountLabel;
	public Text blob0_label;
	public Text blob1_label;
	public Text blob2_label;
	public Text blob3_label;


	public GameObject nameGameObject;
	public GameObject winsGameObject;
	public GameObject blobCountGameObject;
	public GameObject blob0_GameObject;
	public GameObject blob1_GameObject;
	public GameObject blob2_GameObject;
	public GameObject blob3_GameObject;


	// Use this for initialization
	void Start ()
	{
		nameLabel = nameGameObject.GetComponent<Text> ();
		winsLabel = winsGameObject.GetComponent<Text> ();
		blobCountLabel = blobCountGameObject.GetComponent<Text> ();
		blob0_label = blob0_GameObject.GetComponent<Text> ();
		blob1_label = blob1_GameObject.GetComponent<Text> ();
		blob2_label = blob2_GameObject.GetComponent<Text> ();
		blob3_label = blob3_GameObject.GetComponent<Text> ();

		string userKey = "UserId";
		if (PlayerPrefs.HasKey (userKey)) {
			userId = PlayerPrefs.GetInt (userKey);
			Debug.Log ("USERID: " + userId);
		}

		CallAPI ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CallAPI()
	{
		string fullUrl = url + userId;

		Debug.Log (fullUrl);

		WWW www = new WWW (fullUrl);
		StartCoroutine (GetUserInfo(www)); 
	}

	IEnumerator GetUserInfo(WWW www)
	{
		yield return www;

		if (www.error == null) {
			result = JSON.Parse (www.text);
			ParseJson (result);
			SetHeader ();
			SetBlobNames ();
		} else {
			
		}
	}

	public void ParseJson(JSONNode userResult)
	{
		// get user info
		username = result["name"].Value;
		numWins = result["battles_won"].Value;
		numBlobs = result["blobs"].Count;

		// get blob names
		blobName0 = result ["blobs"] [0] ["name"].Value;
		blobName1 = result ["blobs"] [1] ["name"].Value;
		blobName2 = result ["blobs"] [2] ["name"].Value;
		blobName3 = result ["blobs"] [3] ["name"].Value;

		// get blob IDs
		blobId0 = result ["blobs"] [0] ["id"].AsInt;
		blobId1 = result ["blobs"] [1] ["id"].AsInt;
		blobId2 = result ["blobs"] [2] ["id"].AsInt;
		blobId3 = result ["blobs"] [3] ["id"].AsInt;
	}

	public void SetHeader()
	{
		nameLabel.text = username;
		winsLabel.text = numWins;
		blobCountLabel.text = numBlobs.ToString();
	}

	public void SetBlobNames() 
	{
		if (String.IsNullOrEmpty (blobName0)) {
			blob0_label.text = "Locked!";
		} else {
			blob0_label.text = blobName0;
		}

		if (String.IsNullOrEmpty (blobName1)) {
			blob1_label.text = "Locked!";
		} else {
			blob1_label.text = blobName0;
		}

		if (String.IsNullOrEmpty (blobName2)) {
			blob2_label.text = "Locked!";
		} else {
			blob2_label.text = blobName0;
		}

		if (String.IsNullOrEmpty (blobName3)) {
			blob3_label.text = "Locked!";
		} else {
			blob3_label.text = blobName0;
		}
	}


		
}