using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System;
//using UnityEngine.VR.WSA.WebCam;
using System.Runtime.Remoting;
using System.ComponentModel;
using UnityEngine.SceneManagement;

public class BreedCtrl : MonoBehaviour
{

	public TokenCtrl tokenCtrl;

	public string token = "";
	public bool userExists;

	public string url = "http://104.131.144.86/api/users/";
	 

	// PlayerPrefs keys
	public string emailPPKey = "Email";
	public string passwordPPKey = "Password";
	public string userIdPPKey = "UserId";
	public string blobPPKey = "RequestedBlobId";

	public int userId;
	public string email;
	public string password;
	public string newBlobId;

	public string blobName0;
	public string blobName1;
	public string blobName2;
	public string blobName3;

	public string blobId0;
	public string blobId1;
	public string blobId2;
	public string blobId3;

	public Text blobname0_label;
	public Text blobname1_label;
	public Text blobname2_label;
	public Text blobname3_label;

	public Button b0;
	public Button b1;
	public Button b2;
	public Button b3;

	public GameObject b0GO;
	public GameObject b1GO;
	public GameObject b2GO;
	public GameObject b3GO;

	public GameObject blobLabel0_GO;
	public GameObject blobLabel1_GO;
	public GameObject blobLabel2_GO;
	public GameObject blobLabel3_GO;

	public GameObject blobImg0_GO;
	public GameObject blobImg1_GO;
	public GameObject blobImg2_GO;
	public GameObject blobImg3_GO;

	public bool b0Selected = false;
	public bool b1Selected = false;
	public bool b2Selected = false;
	public bool b3Selected = false;

	public string id1;
	public string id2;


	public JSONNode result;



	// Use this for initialization
	void Start ()
	{
		
		// blob names
		blobname0_label = blobLabel0_GO.GetComponent<Text> ();
		blobname1_label = blobLabel1_GO.GetComponent<Text> ();
		blobname2_label = blobLabel2_GO.GetComponent<Text> ();
		blobname3_label = blobLabel3_GO.GetComponent<Text> ();

		// blob icon buttons
		b0 = b0GO.GetComponent<Button> ();
		b1 = b1GO.GetComponent<Button> ();
		b2 = b2GO.GetComponent<Button> ();
		b3 = b3GO.GetComponent<Button> ();

		// PlayerPrefs
		PlayerPrefs.SetInt (blobPPKey, -1);

		GetPlayerPrefs ();
		CallAPI ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	/*
	 * Retrieve the User ID, email, and password for logged in user.
	 */

	private void GetPlayerPrefs ()
	{
		if (PlayerPrefs.HasKey (userIdPPKey)) {
			userId = PlayerPrefs.GetInt (userIdPPKey);
		}

		if (PlayerPrefs.HasKey (emailPPKey)) {
			email = PlayerPrefs.GetString (emailPPKey);
		}

		if (PlayerPrefs.HasKey (passwordPPKey)) {
			password = PlayerPrefs.GetString (passwordPPKey);
		}

		Debug.Log (userId);
		Debug.Log (email);
		Debug.Log (password);
	
	}





	public void CallAPI ()
	{
		string fullUrl = url + userId;

		Debug.Log (fullUrl);

		WWW www = new WWW (fullUrl);
		StartCoroutine (GetUserInfo (www)); 
	}

	IEnumerator GetUserInfo (WWW www)
	{
		yield return www;

		if (www.error == null) {
			Debug.Log ("WWW ok");
			result = JSON.Parse (www.text);
			ParseJson (result);
			SetBlobNames ();
		} else {
			Debug.Log (www.error);
			// do nothing?
		}
	}

	public void ParseJson (JSONNode userResult)
	{


		// get blob names
		blobName0 = result ["blobs"] [0] ["name"].Value;
		blobName1 = result ["blobs"] [1] ["name"].Value;
		blobName2 = result ["blobs"] [2] ["name"].Value;
		blobName3 = result ["blobs"] [3] ["name"].Value;

		// get blob IDs
		blobId0 = result ["blobs"] [0] ["id"].Value;
		blobId1 = result ["blobs"] [1] ["id"].Value;
		blobId2 = result ["blobs"] [2] ["id"].Value;
		blobId3 = result ["blobs"] [3] ["id"].Value;
	}

	/*
	 * Prints the blob names on the scene.
	 */

	public void SetBlobNames ()
	{
		if (String.IsNullOrEmpty (blobName0)) {
			HideTextLabel (blobname0_label);
		} else {
			ShowTextLabel (blobname0_label);
			blobname0_label.text = blobName0;
		}

		if (String.IsNullOrEmpty (blobName1)) {
			HideTextLabel (blobname1_label);
		} else {
			ShowTextLabel (blobname1_label);
			blobname1_label.text = blobName1;
		}

		if (String.IsNullOrEmpty (blobName2)) {
			HideTextLabel (blobname2_label);
		} else {
			ShowTextLabel (blobname2_label);
			blobname2_label.text = blobName2;
		}

		if (String.IsNullOrEmpty (blobName3)) {
			HideTextLabel (blobname3_label);
		} else {
			ShowTextLabel (blobname3_label);
			blobname3_label.text = blobName3;
		}
	}

	public void HideTextLabel (Text label)
	{
		label.enabled = false;
	}

	public void ShowTextLabel (Text label)
	{
		label.enabled = true;
	}

	public void GetSelectedBlobs ()
	{

	}

	/*
	 * Tracks which blob buttons were selected.
	 */

	public void ButtonClicked (string selected)
	{
		if (selected == "0") {
			b0Selected = true;
		}

		if (selected == "1") {
			b1Selected = true;
		}

		if (selected == "2") {
			b2Selected = true;
		}

		if (selected == "3") {
			b3Selected = true;
		}
	}

	/*
	 * Returns true if 2 blobs were selected, else false.
	 */

	public bool SelectedTwoBlobs ()
	{
		int count = 0;

		if (b0Selected == true) {
			count++;
		}

		if (b1Selected == true) {
			count++;
		}

		if (b2Selected == true) {
			count++;
		}

		if (b3Selected == true) {
			count++;
		}

		if (count != 2) {
			return false;
		} else {
			return true;
		}
	}

	/*
	 * Store the blob IDs of the two selected blobs in id1 and id2.
	 */

	public void GetSelectedIds ()
	{
		if (b0Selected == true) {
			id1 = blobId0;
			if (b1Selected == true) {
				id2 = blobId1;
			} else if (b2Selected == true) {
				id2 = blobId2;
			} else if (b3Selected == true) {
				id2 = blobId3;
			}
		} else if (b1Selected == true) {
			id1 = blobId1;
			if (b2Selected == true) {
				id2 = blobId2;
			} else if (b3Selected == true) {
				id2 = blobId3;
			}
		} else if (b2Selected == true) {
			id1 = blobId2;
			if (b3Selected == true) {
				id2 = blobId3;
			}
		}
	}

	/// <summary>
	/// Sends the token request.
	/// </summary>
	/// <param name="email">Email.</param>
	/// <param name="password">Password.</param>
	public void SendTokenRequest (string email, string password)
	{
		Debug.Log ("In token request...");

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

			JSONNode N = JSON.Parse (www.text);

			userExists = true;
			Debug.Log ("User exists.");

			ParseTokenJson (N);
			CallBreedAPI ();

		} else {
			Debug.Log ("Token WWW Error: " + www.error);
			userExists = false;
			Debug.Log ("User doesn't exist.");
			if (www.error == "400 Bad Request") {
				// alert for duplicate email address
			}
		}    
	}

	public void ParseTokenJson (JSONNode data)
	{
		token = data ["token"].Value;
		//Debug.Log ("Parsed, token is: " + token);
	}

	public bool getUserExists ()
	{
		return userExists;
	}

	public void BreedButtonClick ()
	{
		if (SelectedTwoBlobs ()) {
			GetSelectedIds ();
			Debug.Log ("Two blobs selected.");
			SendTokenRequest (email, password);
		} else {
			
		}

	}
		
	/*
	 * Send POST request to API to breed blobs.
	 * 'BlobController@breedBlob'
	 */

	public void CallBreedAPI ()
	{
		string test_id1 = "1";
		string test_id2 = "5";
		string test_token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOjEsImlzcyI6Imh0dHA6XC9cLzEwNC4xMzEuMTQ0Ljg2XC9hcGlcL3VzZXJzXC9hdXRoZW50aWNhdGUiLCJpYXQiOjE0OTA4OTk0NDEsImV4cCI6MTQ5MDkwMzA0MSwibmJmIjoxNDkwODk5NDQxLCJqdGkiOiIzZDc3ZDk5ODk5MGE5YTM4MTRjZGE5NzgwZDM3ZDY4OSJ9.5ehhlR1stVZynGG470M9pyHdwlW1Ih8aUXwAMVPTKUg";

		Debug.Log ("Entering CallBreedAPI()...");
		Debug.Log ("test_id1: " + test_id1);
		Debug.Log ("test_id2: " + test_id2);
		Debug.Log ("test_token: " + test_token);

		string breedUrl = "http://104.131.144.86/api/blobs";

		WWWForm form = new WWWForm ();
		form.AddField ("id1", test_id1);
		form.AddField ("id2", test_id1);
		form.AddField ("token", test_token);
		WWW www = new WWW (breedUrl, form);

		StartCoroutine (WaitForRequest2 (www));
	}

	IEnumerator WaitForRequest2 (WWW www)
	{
		yield return www;

		JSONNode N = JSON.Parse (www.text);

		if (www.error == null) {
			ParseNewBlobJson (N);
			SceneManager.LoadScene ("BreedNameChange");
		} else {
			string err = N ["error"].Value;

			Debug.Log ("WWW breed error..." + err);
		}
	}

	/*
	 * Returns the blob ID of the newly breeded blob.
	 */

	public void ParseNewBlobJson (JSONNode N)
	{
		newBlobId = N ["blobID"].Value;

		PlayerPrefs.SetString ("NewBlobID", newBlobId);
		PlayerPrefs.Save ();
	}

}
