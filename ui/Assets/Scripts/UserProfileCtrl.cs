using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using SimpleJSON;
//using UnityEditor.Sprites;
using System;

public class UserProfileCtrl : MonoBehaviour
{

	public PlayerPreferences playerPreferences;

	public int userId;
	public string username = "-1";
	public string numWins = "-1";
	public int numBlobs = -1;

	public string blobName0;
	public string blobName1;
	public string blobName2;
	public string blobName3;


	public string blobId0;
	public string blobId1;
	public string blobId2;
	public string blobId3;

	public JSONNode result;

	public Text nameLabel;
	public Text winsLabel;
	public Text blobCountLabel;

	public Text blob0_label;
	public Text blob1_label;
	public Text blob2_label;
	public Text blob3_label;

	public Button b0;
	public Button b1;
	public Button b2;
	public Button b3;

	public GameObject nameGameObject;
	public GameObject winsGameObject;
	public GameObject blobCountGameObject;

	public GameObject blob0_GameObject;
	public GameObject blob1_GameObject;
	public GameObject blob2_GameObject;
	public GameObject blob3_GameObject;

	public GameObject b0GO;
	public GameObject b1GO;
	public GameObject b2GO;
	public GameObject b3GO;


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

		b0 = b0GO.GetComponent<Button> ();
		b1 = b1GO.GetComponent<Button> ();
		b2 = b2GO.GetComponent<Button> ();
		b3 = b3GO.GetComponent<Button> ();

		// Reset RequestedBlobId
		playerPreferences.ResetRequestedBlob ();

		userId = playerPreferences.GetUser ();



		CallAPI ();
	}
		
	public void CallAPI ()
	{
		string url = "http://104.131.144.86/api/users/";
		string fullUrl = url + userId;
		Debug.Log ("FULL URL: " + fullUrl);
		WWW www = new WWW (fullUrl);
		StartCoroutine (GetUserInfo (www)); 
	}

	IEnumerator GetUserInfo (WWW www)
	{
		yield return www;

		if (www.error == null) {
			result = JSON.Parse (www.text);
			ParseJson (result);
			SetHeader ();
			SetBlobNames ();
			ManageBlobButtons ();
		} else {
			
		}
	}

	public void ParseJson (JSONNode userResult)
	{
		// set user info
		username = result ["name"].Value;
		numWins = result ["battles_won"].Value;
		numBlobs = result ["blobs"].Count;

		// save number of blobs owned by user in memory
		playerPreferences.SetNumBlobs (numBlobs);

		// set blob names
		blobName0 = result ["blobs"] [0] ["name"].Value;
		blobName1 = result ["blobs"] [1] ["name"].Value;
		blobName2 = result ["blobs"] [2] ["name"].Value;
		blobName3 = result ["blobs"] [3] ["name"].Value;

		// set blob IDs
		blobId0 = result ["blobs"] [0] ["id"].Value;
		blobId1 = result ["blobs"] [1] ["id"].Value;
		blobId2 = result ["blobs"] [2] ["id"].Value;
		blobId3 = result ["blobs"] [3] ["id"].Value;
	}


	/// <summary>
	/// Prints user info on the scene.
	/// </summary>
	public void SetHeader ()
	{
		nameLabel.text = username;
		winsLabel.text = numWins;
		blobCountLabel.text = numBlobs.ToString ();
	}


	/// <summary>
	/// Prints blob names on the scene.
	/// </summary>
	public void SetBlobNames ()
	{
		if (String.IsNullOrEmpty (blobName0)) {
			blob0_label.text = "Locked!";
		} else {
			blob0_label.text = blobName0;
		}

		if (String.IsNullOrEmpty (blobName1)) {
			blob1_label.text = "Locked!";
		} else {
			blob1_label.text = blobName1;
		}

		if (String.IsNullOrEmpty (blobName2)) {
			blob2_label.text = "Locked!";
		} else {
			blob2_label.text = blobName2;
		}

		if (String.IsNullOrEmpty (blobName3)) {
			blob3_label.text = "Locked!";
		} else {
			blob3_label.text = blobName3;
		}
	}
		
	/// <summary>
	/// This is for he blob icons under "Your Blobs."
	/// It stores the blob ID of the blob selected in PlayerPrefs.
	/// </summary>
	/// <param name="blobNum">Number of blobs owned by user. </param>
	public void BlobButtonClick (string blobNum)
	{
		if (blobNum == "0" && !String.IsNullOrEmpty (blobName0)) {
			playerPreferences.SetRequestedBlob (blobId0);
		} else if (blobNum == "1" && !String.IsNullOrEmpty (blobName1)) {
			playerPreferences.SetRequestedBlob (blobId1);
		} else if (blobNum == "2" && !String.IsNullOrEmpty (blobName2)) {
			playerPreferences.SetRequestedBlob (blobId2);
		} else if (blobNum == "3" && !String.IsNullOrEmpty (blobName3)) {
			playerPreferences.SetRequestedBlob (blobId3);
		}
	}

	/// <summary>
	/// Enable or disable blob icons based on the number of blobs owned.
	/// If numBlobs == 1, then enable b0 ion. If numBlobs == 2, then enable 
	/// b0 and b1 icons...and so on.
	/// </summary>
	public void ManageBlobButtons()
	{
		Debug.Log ("Exercise... " + playerPreferences.GetExercise ());

		if (numBlobs == 1) {
			b0.enabled = true;
			b1.enabled = false;
			b2.enabled = false;
			b3.enabled = false;
		} else if (numBlobs == 2) {
			b0.enabled = true;
			b1.enabled = true;
			b2.enabled = false;
			b3.enabled = false;
		} else if (numBlobs == 3) {
			b0.enabled = true;
			b1.enabled = true;
			b2.enabled = true;
			b3.enabled = false;
		} else if (numBlobs == 4) {
			b0.enabled = true;
			b1.enabled = true;
			b2.enabled = true;
			b3.enabled = true;
		} else {
			b0.enabled = false;
			b1.enabled = false;
			b2.enabled = false;
			b3.enabled = false;
		}
	}
		
	/// <summary>
	/// Erase values saved in PlayerPrefs when user clicks Log Out button.
	/// </summary>
	public void LogOutButtonClick()
	{
		playerPreferences.ResetNumBlobs ();
		playerPreferences.ResetRequestedBlob ();
		playerPreferences.ResetUser ();

		//Debug.Log ("numBlobs after reset: " + playerPreferences.GetNumBlobs ());
		//Debug.Log ("requested blob ID after reset: " + playerPreferences.GetRequestedBlob ());
		//Debug.Log ("userID after reset: " + playerPreferences.GetUser ());
	}


		
}