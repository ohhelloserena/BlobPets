using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Text.RegularExpressions;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SimpleJSON;
using System.Runtime.Remoting;
using UnityEditor;
using System.Runtime.InteropServices;
using System.ComponentModel;


// Create new user with given name, email, and password.

public class NewUser : MonoBehaviour
{
	public InputFieldCtrl ifctrl;
	public ValidInputsCtrl validInputsCtrl;
	public ButtonCtrl buttonCtrl;

	public string username = "-1";
	public string email = "-1";
	public string password = "-1";
	public string userId = "-1";

	public string url = "http://104.131.144.86/api/users/";

	public bool validName = false;
	public bool validEmail = false;
	public bool validPW = false;
	public bool validNewUserCombo = false;

	public GameObject namePanel;
	public GameObject emailPanel;
	public GameObject passwordPanel;

	public JSONNode result;
	private string errorMsg;
	public string emailServerError = "Email address has already been taken";
	public string otherServerError = "Missing required input fields";

	// Use this for initialization
	void Start ()
	{

		HideNamePanel ();
		HideEmailPanel ();
		HidePasswordPanel ();
	}



	public void ButtonClick ()
	{
		Debug.Log ("BUTTON CLICKED");

		username = ifctrl.getName ();
		password = ifctrl.getPassword ();
		email = ifctrl.getEmail ();

		validName = validInputsCtrl.IsValidName (username);
		validPW = validInputsCtrl.IsValidPassword (password);
		validEmail = validInputsCtrl.IsValidEmailAddress (email);

		CreateUser ();
	}

	public string getErrorMessage ()
	{
		return errorMsg;
	}


	/*
	 * Sends post request to create new user if name, email, and 
	 * password are valid.
	 */

	private void CreateUser ()
	{
		if (validName && validEmail && validPW) {
			Debug.Log ("ENTERED VALID INPUTS.");
			CallAPI ();
		} else {
			Debug.Log ("ENTERED INVALID INPUTS.");

			if (validName == false) {
				ShowNamePanel ();
			} else {
				HideNamePanel ();
			}

			if (validEmail == false) {
				ShowEmailPanel ();
			} else {
				HideEmailPanel ();
			}

			if (validPW == false) {
				ShowPasswordPanel ();
			} else {
				HidePasswordPanel ();
			}
		}
	}


	private void CallAPI ()
	{
		Debug.Log ("SENDING API CALL...");
		WWWForm form = new WWWForm ();
		form.AddField ("name", username);
		form.AddField ("email", email);
		form.AddField ("password", password);
		WWW www = new WWW (url, form);
		StartCoroutine (GetUserInfo (www));
	}

	IEnumerator GetUserInfo (WWW www)
	{
		yield return www;

		result = JSON.Parse (www.text);

		// check for errors
		if (www.error == null) {
			Debug.Log ("WWW Ok! User created.");
			userId = www.text;
			Debug.Log ("userId: " + userId);
			SceneManager.LoadScene ("UserProfileUI");

		} else {
			Debug.Log ("WWW Error: " + www.error);
			errorMsg = ParseJson ("error");

			if (errorMsg == emailServerError) {
				ShowEmailPanel ();
			}

			if (errorMsg == otherServerError) {
				ButtonClick ();
			}
		} 
	}
		
	private string ParseJson (string name)
	{
		return result [name].Value;
	}

	public void ShowNamePanel ()
	{
		namePanel.gameObject.SetActive (true);
	}

	public void ShowEmailPanel ()
	{
		emailPanel.gameObject.SetActive (true);
	}

	public void ShowPasswordPanel ()
	{
		passwordPanel.gameObject.SetActive (true);
	}

	public void HideNamePanel ()
	{
		namePanel.gameObject.SetActive (false);
	}

	public void HideEmailPanel ()
	{
		emailPanel.gameObject.SetActive (false);
	}

	public void HidePasswordPanel ()
	{
		passwordPanel.gameObject.SetActive (false);
	}

	
}
