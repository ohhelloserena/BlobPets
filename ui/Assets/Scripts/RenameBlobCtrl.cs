using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class RenameBlobCtrl : MonoBehaviour {

	public InputFieldCtrl ifCtrl;
	public string newName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/*
	 * Retrieve name entered in input field upon button click.
	 */

	public void SubmitButtonClick()
	{
		//Debug.Log ("Submit button clicked...");

		newName = ifCtrl.getName ();

		StartCoroutine (UpdateBlobName());
	}

	/*
	 * Send PUT request to API to update name for newly created blob.
	 */

	IEnumerator UpdateBlobName()
	{
		string url = "http://104.131.144.86/api/blobs/";
		string myData = "dummy";
		string newBlobId = GetNewBlobId();

		string finalUrl = url + newBlobId + "?name=" + newName;

		Debug.Log ("final URL: " + finalUrl);

		using (UnityWebRequest www = UnityWebRequest.Put (finalUrl, myData)) {
			yield return www.Send ();

			if (www.isError) {
				Debug.Log ("PUT error: " + www.error);
			} else {
				Debug.Log ("PUT request successful...");
				Debug.Log (www.url.ToString ());

				SceneManager.LoadScene ("UserProfileUI");

			}
		}
	
	}

	/*
	 * Retrieve the blob ID of the newly created blob from PlayerPrefs.
	 */

	public string GetNewBlobId() 
	{
		string key = "NewBlobID";
		if (PlayerPrefs.HasKey (key)) {
			return PlayerPrefs.GetString (key);
		} else {
			return "";
		}
	}




}
