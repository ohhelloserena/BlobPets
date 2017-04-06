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
using UnityEngine.SceneManagement;
using System.Security.Cryptography;

public class FeedPoopCtrl : MonoBehaviour
{
	public PlayerPreferences playerPreferences;
	// JSON
	public JSONNode N;
	public string nextCleanTime;
	public string nextFeedTime;
	public string level;
	public string cleanlinessLevel;
	public string healthLevel;
	public string exerciseLevel;
	public string blobLevel;

	//private string nameKey = "Name";
	private string emailKey = "Email";
	private string passwordKey = "Password";
	private string idKey = "UserId";

	public bool userExists;
	public string userId;
	public string email;
	public string password;

	public string url = "http://104.131.144.86/api/blobs/";
	public string blobId;
	public string blobName0 = "";
	public string blobName1 = "";

	public string token;

	public Image imgPoop;
	public Image imgHam;
	public Image imgThoughtBub;

	public GameObject poopGO;
	public GameObject hamGO;
	public GameObject thoughtGO;

	public bool needsCleaning = false;
	public bool needsFeeding = false;

	public DateTime dateTime;
	public DateTime cleanTime;
	public DateTime feedTime;
	public int cleanComp;
	public int feedComp;

	// level bars
	public Text h;	// health level
	public Text e;	// exercise level
	public Text c;	// cleanliness level
	public Text b;	// blob level

	public GameObject hGO;	// health level
	public GameObject eGO;	// exercise level	
	public GameObject cGO;	// cleanliness level
	public GameObject bGO;	// blob level

	public Button battleBtn;
	public GameObject blueBattleBtnGO;

	// battle notification warning
	public Image battleWarning;
	public GameObject battleGO;

	public Text warning_label;
	public GameObject WL_GO;

	public Text warning_body;
	public GameObject WB_GO;

	public Button yes_button;
	public GameObject yes_GO;

	public Button no_button;
	public GameObject no_GO;



	// battle 
	public GPSCtrl battleGPS;
	private GameObject gpsObject;
	public double currentLat;
	public double currentLong;

	// Use this for initialization
	void Start ()
	{
		battleBtn = blueBattleBtnGO.GetComponent<Button> ();

		battleWarning = battleGO.GetComponent<Image> ();
		warning_label = WL_GO.GetComponent<Text> ();
		warning_body = WB_GO.GetComponent<Text> ();
		yes_button = yes_GO.GetComponent<Button> ();
		no_button = no_GO.GetComponent<Button> ();

		h = hGO.GetComponent<Text> ();
		c = cGO.GetComponent<Text> ();
		e = eGO.GetComponent<Text> ();
		b = bGO.GetComponent<Text> ();


		DisableWarningWindow ();

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

		imgPoop = poopGO.GetComponent<Image> ();
		imgHam = hamGO.GetComponent<Image> ();
		imgThoughtBub = thoughtGO.GetComponent<Image> ();

		imgPoop.enabled = false;
		imgHam.enabled = false;
		imgThoughtBub.enabled = false;
		SendTokenRequest (email, password);
		GetBlob ();
	}



	// Update is called once per frame
	void Update ()
	{

		dateTime = DateTime.Now.ToUniversalTime ();
		//Debug.Log ("Current time in UTC: " + dateTime.ToString ());

		cleanTime = Convert.ToDateTime (nextCleanTime);

		feedTime = Convert.ToDateTime (nextFeedTime);

		cleanComp = DateTime.Compare (dateTime, cleanTime);
		feedComp = DateTime.Compare (dateTime, feedTime);


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
			PrintLevelBars ();
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
		exerciseLevel = N ["exercise_level"].Value;
		blobName0 = N ["name"].Value;
		blobLevel = N ["level"].Value;
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

			if (imgPoop.enabled != false) {
				imgPoop.enabled = false;
			}

		} else if (cleanComp == 0 || cleanComp > 0) {
			// == 0 : dateTime same as CleanTime
			// > 0 : dateTime is later than cleanTime

			// print poop img

			if (imgPoop.enabled != true) {
				Debug.Log ("PRINTING POOP...");

				needsCleaning = true;
				imgPoop.enabled = true;
			}
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
		string myData = "Hello";	// need to send a dummy string in UnityWebReqest.Put, otherwise it won't work
		string finalUrl;

		if (button == "feed") {
			finalUrl = url + blobId + "?token=" + token + "&health_level=" + healthLevel;
		} else {
			finalUrl = url + blobId + "?token=" + token + "&cleanliness_level=" + cleanlinessLevel;
		}

		Debug.Log ("final URL... " + finalUrl);

		using (UnityWebRequest www = UnityWebRequest.Put (finalUrl, myData)) {
			yield return www.Send ();

			if (www.isError) {
				Debug.Log ("PUT ERROR: " + www.error);
			} else {
				Debug.Log ("PUT REQUEST SUCCESSFUL.");
				Debug.Log (www.url.ToString ());

				GetBlob ();
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

	public void BattleButtonClicked ()
	{
		Debug.Log ("Battle Button clicked...");

		int healthLevelInt;
		int cleanlinessLevelInt;
		int exerciseLevelInt;

		decimal val = Decimal.Parse (exerciseLevel);
		exerciseLevelInt = Convert.ToInt32 (val);


		Int32.TryParse (healthLevel, out healthLevelInt);	// feed
		Int32.TryParse (cleanlinessLevel, out cleanlinessLevelInt);	// poop
		//Int32.TryParse (exerciseLevel, out exerciseLevelInt);	// exercise

		Debug.Log ("Lat: " + currentLat + " Long: " + currentLong);

		Debug.Log ("battle button clicked");
		Debug.Log ("string exercise: " + exerciseLevel);

		Debug.Log ("health: " + healthLevelInt);
		Debug.Log ("clean: " + cleanlinessLevelInt);
		Debug.Log ("exercise: " + exerciseLevelInt);


		if (healthLevelInt < 10 || cleanlinessLevelInt < 10 || exerciseLevelInt < 10) {
			EnableWarningWindow ();
		} else {
			gpsObject = GameObject.Find ("GPSCTRL");
			battleGPS = (GPSCtrl)gpsObject.GetComponent (typeof(GPSCtrl));
			battleGPS.StartLocationService ();
			battleGPS.GetCurrentLocation ();
			currentLat = battleGPS.lat2;
			currentLong = battleGPS.long2;
			StartCoroutine (BattlePutRequest ());
		}
	}

	// PUT request
	IEnumerator BattlePutRequest ()
	{
		string myData = "Hello";	// need to send a dummy string in UnityWebReqest.Put, otherwise it won't work
		string userURL = "http://104.131.144.86/api/users/";
		string finalUrl = userURL + userId + "?token=" + token + "&latitude=" + currentLat + "&longitude=" + currentLong;
		Debug.Log ("final URL... " + finalUrl);

		using (UnityWebRequest www = UnityWebRequest.Put (finalUrl, myData)) {
			yield return www.Send ();

			if (www.isError) {
				Debug.Log ("PUT ERROR: " + www.error);
			} else {
				Debug.Log ("PUT REQUEST SUCCESSFUL.");
				Debug.Log (www.url.ToString ());
				string battleGetURL = userURL + "?type=nearby&lat=" + currentLat + "&long=" + currentLong;
				GetBattleUsers (battleGetURL);
			}
		}
	}

	public void GetBattleUsers (string url)
	{
		WWW www = new WWW (url);
		StartCoroutine (BattleGetRequest (www));
	}

	IEnumerator BattleGetRequest (WWW www)
	{
		yield return www;

		// check for errors
		if (www.error == null) {
			PlayerPrefs.SetString("battleUserList", www.text);
			Debug.Log ("WWW Ok!: " + www.text);
			SceneManager.LoadScene ("BattleMain");
			Debug.Log("WWW Ok!: " + www.text);
		} else {
			Debug.Log ("WWW Error: " + www.error);
		}    
	}

	public void WarningButtonClicked(string cmd)
	{
		if (cmd == "yes") {
			gpsObject = GameObject.Find ("GPSCTRL");
			battleGPS = (GPSCtrl)gpsObject.GetComponent (typeof(GPSCtrl));
			battleGPS.StartLocationService ();
			battleGPS.GetCurrentLocation ();
			currentLat = battleGPS.lat2;
			currentLong = battleGPS.long2;
			StartCoroutine (BattlePutRequest ());
		} else if (cmd == "no") {
			DisableWarningWindow ();
		}
	}

	public void SetWarningWindowText(int msgCode)
	{
		if (msgCode == 0) {
			warning_body.text = "Your blob is too weak to battle. Would you like to do it anyway?";
		} else if (msgCode == 1) {
			warning_body.text = "Your blob is tired and needs to rest before it can battle again. Try again later.";
		}
	}

	public void DisableWarningWindow()
	{
		battleWarning.enabled = false;
		warning_label.enabled = false;
		warning_body.enabled = false;
		yes_GO.SetActive (false);
		no_GO.SetActive (false);
	}

	public void EnableWarningWindow()
	{
		battleWarning.enabled = true;
		warning_label.enabled = true;
		warning_body.enabled = true;
		yes_GO.SetActive (true);
		no_GO.SetActive (true);
	}

	public void PrintLevelBars()
	{
		decimal val = Decimal.Parse (exerciseLevel);
		//int level = Convert.ToInt32 (val);
		int levelRounded = (int)Math.Round (val, 0);

		h.text = healthLevel;
		c.text = cleanlinessLevel;
		e.text = levelRounded.ToString ();
		b.text = blobLevel;


	}
}