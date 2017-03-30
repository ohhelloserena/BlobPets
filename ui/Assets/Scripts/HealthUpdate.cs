using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System;

public class HealthUpdate : MonoBehaviour {
	public PlayerPreferences playerPreferences;

	//private string nameKey = "Name";
	private string emailKey = "Email";
	private string passwordKey = "Password";
	private string idKey = "UserId";

	public bool userExists;
	public string userId;
	public string email;
	public string password;

	public string url = "http://104.131.144.86/api/blobs/";
	public string token;

	public string blobId;

	public JSONNode N;
	public string healthLevel;
	public float healthFloatValue;

	public Image healthBar;

	public float health = 100.0f;
	private const float coef = 0.2f;

	// Use this for initialization
	void Start () {
		healthBar = GetComponent<Image>();
		healthBar.fillAmount = 1;

		string blobIdKey = "RequestedBlobId";
		if (PlayerPrefs.HasKey (blobIdKey)) {
			blobId = PlayerPrefs.GetString (blobIdKey);
		}

		Debug.Log ("blobId: " + blobId);

		if (PlayerPrefs.HasKey (emailKey)) {
			email = PlayerPrefs.GetString (emailKey);
		}

		if (PlayerPrefs.HasKey (passwordKey)) {
			password = PlayerPrefs.GetString (passwordKey);
		}

		if (PlayerPrefs.HasKey (idKey)) {
			userId = PlayerPrefs.GetString (idKey);
		}
			
		SendTokenRequest (email, password);
		GetBlob ();
	}

	/// <summary>
	/// Sends the token request via POST request to API.
	/// </summary>
	/// <param name="email">Email address of logged in user. </param>
	/// <param name="password">Password of logged in user. </param>
	public void SendTokenRequest (string email, string password)
	{
		string tokenUrl = "http://104.131.144.86/api/users/authenticate";
		WWWForm form = new WWWForm ();
		form.AddField ("email", email);
		form.AddField ("password", password);
		WWW www = new WWW (tokenUrl, form);
		StartCoroutine (WaitForRequest (www));
	}

	/// <summary>
	/// Waits for request.
	/// </summary>
	/// <returns>The for request.</returns>
	/// <param name="www">Www.</param>
	IEnumerator WaitForRequest (WWW www)
	{
		yield return www;

		// check for errors
		if (www.error == null) {
			Debug.Log ("!!! USER EXISTS.");
			userExists = true;
			JSONNode N = JSON.Parse (www.text);
			ParseJson (N);

			PlayerPrefs.SetString (emailKey, email);
			PlayerPrefs.SetString (passwordKey, password);
			PlayerPrefs.SetInt (idKey, Int32.Parse (userId));
			PlayerPrefs.Save ();

		} else {
			Debug.Log ("!!! USER DOESN'T EXIST.");
			Debug.Log ("***WWW Error: " + www.error);
			userExists = false;

		}    
	}

	public void ParseJson (JSONNode data)
	{
		token = data ["token"].Value;
		userId = data ["id"].Value;
	}

	public void GetBlob ()
	{
		WWW www = new WWW (url + blobId);
		StartCoroutine (GetBlobInfo (www));
	}


	IEnumerator GetBlobInfo (WWW www)
	{
		yield return www;

		// check for errors
		if (www.error == null) {
			N = JSON.Parse (www.text);
			ParseJson ();
			Debug.Log ("GetBlobInfo OK: " + www.text);
		} else {
			Debug.Log ("WWW Error: " + www.error);
		}    
	}

	public void ParseJson ()
	{
		healthLevel = N ["health_level"].Value;

		healthFloatValue = float.Parse (healthLevel);
		healthFloatValue = healthFloatValue / 100;

		Debug.Log ("health level: " + healthLevel);
	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}

	void Update () {
//		health -= coef * Time.deltaTime;
		if (healthBar.fillAmount == 0.20f) {
			
		}
		healthBar.fillAmount -= 0.0005f;
//		healthBar.fillAmount -= healthFloatValue;
		//Debug.Log("Health : " + health);
		//Debug.Log("fill: " + healthBar.fillAmount);
	}        
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//
//public class HealthUpdate : MonoBehaviour {
//
//	private Image _energyBar;
//
//	void Start()
//	{
//		_energyBar = GetComponent<Image>();
//		_energyBar.fillAmount = 0;
//	}
//
//	// Update is called once per frame
//	void Update()
//	{
//		_energyBar.fillAmount += 0.0010f;
//	}
//
//	public int getMode()     {
//		int mode = 0;
//		if (_energyBar.fillAmount > 0.999f)
//		{
//			mode = 3;
//		}
//		else if (_energyBar.fillAmount > 0.666f)
//		{
//			mode = 2;
//		}
//		else if (_energyBar.fillAmount > 0.333f)
//		{
//			mode = 1;
//		}
//		return mode;
//
//	}
//
//
//	public void shrink()
//	{
//		if (_energyBar.fillAmount > 0.999f) {
//			_energyBar.fillAmount = 0;
//		} else if (_energyBar.fillAmount > 0.666f) {
//			_energyBar.fillAmount -= 0.666f;
//		} else if (_energyBar.fillAmount > 0.333f) {
//			_energyBar.fillAmount -= 0.333f;
//		}
//	}
//}