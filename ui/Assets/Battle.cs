using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.SceneManagement;

public class Battle : MonoBehaviour
{
	private string blobID1;
	private string blobID2;

	public string token;
	public string email;
	public string password;

	string url = "http://104.131.144.86/api/battles";

	public PlayerPreferences pp;

	// Use this for initialization
	void Start ()
	{
		email = pp.GetEmail ();
		password = pp.GetPassword ();

		blobID1 = PlayerPrefs.GetString ("RequestedBlobId");
		blobID2 = PlayerPrefs.GetString ("opponentBlobId");

		SendTokenRequest (email, password);
	}

	// playerprefs string "RequestedBlobId" --> current user's blob that will battle

	// Update is called once per frame
	void Update ()
	{
		
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
			Debug.Log ("User exists.");

			ParseTokenJson (N);
			CallBattleAPI ();
		} else {
			Debug.Log ("Token WWW Error: " + www.error);
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

	public void CallBattleAPI ()
	{
		// serverAddress.com/api/battles?blob1=<>&blob2=<> (POST TO THIS)
		// add to url ---> ?blob1=<>&blob2=<>
		WWWForm form = new WWWForm ();
		form.AddField ("blob1", blobID1);
		form.AddField ("blob2", blobID2);
		WWW www = new WWW (url, form);
		
		StartCoroutine (WaitForBattleRequest (www));
	}

	IEnumerator WaitForBattleRequest (WWW www)
	{
		yield return www;

		// check for errors
		if (www.error == null) {
			Debug.Log ("!!! USER EXISTS.");
//			SceneManager.LoadScene ("BattleResult");
			Debug.Log ("battleresults: " + www.text);
			SceneManager.LoadScene ("BattleMain"); // TEMP SCENE
		} else {
			Debug.Log ("!!! USER DOESN'T EXIST.");
			Debug.Log ("***WWW Error: " + www.error);
		}    
	}
}
