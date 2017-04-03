using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using SimpleJSON;
using System;


public class CreateButtonScript : MonoBehaviour
{
	private string scene;

	public InputFieldToText nameInput;
	private GameObject nameInputObject;
	public JSONNode result;

	private SelectBlob selectBlueScript;
	private SelectBlob selectOrangeScript;
	private SelectBlob selectPinkScript;
	private SelectBlob selectGreenScript;

	private GameObject selectedBlue;
	private GameObject selectedOrange;
	private GameObject selectedPink;
	private GameObject selectedGreen;

	public string url = "http://104.131.144.86/api/users/";
	public string token;
	public string blobName = "";
	public string blobColor = "";
	public string blobType = "type A";

	// PlayerPrefs keys
	public string emailPPKey = "Email";
	public string passwordPPKey = "Password";
	public string userIdPPKey = "UserId";
	public string blobPPKey = "RequestedBlobId";

	public int userId;
	public string email;
	public string password;
	public string newBlobId;

	// Use this for initialization
	void Start ()
	{
		selectedBlue = GameObject.Find ("BlueBlobSelectionScriptObject");
		selectBlueScript = (SelectBlob)selectedBlue.GetComponent (typeof(SelectBlob));

		selectedOrange = GameObject.Find ("OrangeBlobSelectionScriptObject");
		selectOrangeScript = (SelectBlob)selectedOrange.GetComponent (typeof(SelectBlob));

		selectedPink = GameObject.Find ("PinkBlobSelectionScriptObject");
		selectPinkScript = (SelectBlob)selectedPink.GetComponent (typeof(SelectBlob));

		selectedGreen = GameObject.Find ("GreenBlobSelectionScriptObject");
		selectGreenScript = (SelectBlob)selectedGreen.GetComponent (typeof(SelectBlob));
	}

	// Update is called once per frame
	void Update ()
	{

	}

	public void LoadScene ()
	{
		nameInputObject = GameObject.Find ("InputFieldObject");
		nameInput = (InputFieldToText)nameInputObject.GetComponent (typeof(InputFieldToText));
		blobName = nameInput.TextBox.text;
		Debug.Log (blobName);

		if (nameInput != null) {
			if (selectBlueScript.blueGuiEnable == true) {
				scene = "BlueMain";
				blobColor = "blue";
				//			SendTokenRequest (email, password);
				Debug.Log (selectBlueScript.blueGuiEnable);
				Debug.Log ("Blue Scene load");
			}

			if (selectOrangeScript.orangeGuiEnable == true) {
				scene = "OrangeMain";
				blobColor = "orange";
				Debug.Log (selectOrangeScript.orangeGuiEnable);
				Debug.Log ("Orange Scene load");
			}

			if (selectPinkScript.pinkGuiEnable == true) {
				scene = "PinkMain";
				blobColor = "pink";
				Debug.Log (selectPinkScript.pinkGuiEnable);
				Debug.Log ("Pink Scene load");
			}

			if (selectGreenScript.greenGuiEnable == true) {
				scene = "GreenMain";
				blobColor = "green";
				Debug.Log (selectGreenScript.greenGuiEnable);
				Debug.Log ("Green Scene load");
			}
		} else {
			Debug.Log ("Error: no name");
		}

		GetPlayerPrefs ();
		CreateBlob ();
		SceneManager.LoadScene (scene);
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

	// POST request
	public void CreateBlob ()
	{
		Debug.Log ("blobname: " + blobName);
		if (blobName != null) {
			CallAPI ();
		} else {
			Debug.Log ("No name entered");
		}
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
			//			ParseJson (result);
			//			SetBlobName ();
			Debug.Log(result);
		} else {
			Debug.Log (www.error);
			// do nothing?
		}
	}

	//	private void CallAPI ()
	//	{
	//		Debug.Log ("SENDING API CALL...");
	//		Debug.Log (blobName + " " + blobColor + " " + blobType);
	//
	//		WWWForm form = new WWWForm ();
	//		form.AddField ("name", blobName);
	//		form.AddField ("color", blobColor);
	//		form.AddField ("type", blobType);
	//		WWW www = new WWW (url, form);
	//		StartCoroutine (GetBlobInfo (www));
	//	}
	//
	//	IEnumerator GetBlobInfo (WWW www)
	//	{
	//		Debug.Log("Entering GetBlobInfo()");
	//
	//		yield return www;
	//
	//		// check for errors
	//		if (www.error == null) {
	//			Debug.Log ("WWW Ok! Blob created.");
	//
	//			blobId = Convert.ToInt32 (www.text);
	//			Debug.Log ("ID: " + blobId);
	//
	//			//Debug.Log ("userId after parse: " + userId);
	//
	//			SaveBlobInfo (blobName, blobColor, blobType, blobId);
	//			SendTokenRequest (blobId, blobName);
	//		} else {
	//			Debug.Log ("WWW Error: " + www.error);
	//			result = JSON.Parse (www.text);
	//			errorMessage = ParseJson ("error");
	//			Debug.Log (errorMessage);
	//		} 
	//	}
	//
	//	private void SaveBlobInfo(string blobName, string blobColor, string blobType, int blobId) 
	//	{
	//		/*
	//		playerPreferences.SetEmail (emailAddr);
	//		playerPreferences.SetPassword (passwrd);
	//		playerPreferences.SetUser (userid);
	//		*/
	//
	//		PlayerPrefs.SetString ("BlobName", blobName);
	//		PlayerPrefs.SetString ("BlobColor", blobColor);
	//		PlayerPrefs.SetString ("BlobType", blobType);
	//		PlayerPrefs.SetInt ("BlobId", blobId);
	//		PlayerPrefs.Save ();
	//	}
	//
	//	/// <summary>
	//	/// Sends the token request via POST request to API.
	//	/// </summary>
	//	/// <param name="email">Email.</param>
	//	/// <param name="password">Password.</param>
	//	public void SendTokenRequest (int blobId, string blobName)
	//	{
	//		string tokenUrl = "http://104.131.144.86/api/users/authenticate";
	//		WWWForm form = new WWWForm ();
	//		form.AddField ("name", blobName);
	//		form.AddField ("id", blobId);
	//		WWW www = new WWW (tokenUrl, form);
	//		StartCoroutine (WaitForTokenRequest (www));
	//	}
	//
	//	IEnumerator WaitForTokenRequest (WWW www)
	//	{
	//		yield return www;
	//
	//		// check for errors
	//		if (www.error == null) {
	//			JSONNode N = JSON.Parse (www.text);
	//
	//			Debug.Log("Blob exists");
	//
	//			token = ParseJson ("token", N);
	//		} else {
	//			Debug.Log ("***WWW Error: " + www.error);
	//
	//			Debug.Log("Blob doesn't exist");
	//			if (www.error == "400 Bad Request") {
	//				// alert for duplicate email address
	//			}
	//		}    
	//	}
	//
	//	private string ParseJson (string name)
	//	{
	//		return result [name].Value;
	//	}
	//
	//	/// <summary>
	//	/// Parses the string value for the given JSONNode and ID.
	//	/// </summary>
	//	/// <returns>String value</returns>
	//	/// <param name="id">Identifier.</param>
	//	/// <param name="data">Data.</param>
	//	public string ParseJson(string id, JSONNode data)
	//	{
	//		return data [id].Value;
	//	}
}