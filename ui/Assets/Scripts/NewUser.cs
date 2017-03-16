using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Text.RegularExpressions;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Create new user with given name, email, and password.
using System.Runtime.Remoting;
using UnityEditor;
using System.Runtime.InteropServices;
using System.ComponentModel;

public class NewUser : MonoBehaviour
{
	public InputFieldCtrl ifctrl;
	public ValidInputsCtrl validInputsCtrl;

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

		username = ifctrl.username;
		password = ifctrl.password;
		email = ifctrl.email;

		validName = validInputsCtrl.IsValidName (username);
		validPW = validInputsCtrl.IsValidPassword (password);
		validEmail = validInputsCtrl.IsValidEmailAddress (email);

		CreateUser ();
	}

	/*
	 * Sends post request to create new user if name, email, and 
	 * password are valid.
	 */

	public void CreateUser ()
	{
		if (validName && validEmail && validPW) {
			Debug.Log ("ENTERED VALID INPUTS.");
			CallAPI ();
		} else {
			Debug.Log ("ENTERED INVALID INPUTS.");

			Debug.Log ("validName: " + validName);
			Debug.Log ("validEmail: " + validEmail);
			Debug.Log ("validPW: " + validPW);

			if (validName == false) {
				ShowNamePanel ();
			}
			if (validEmail == false) {
				ShowEmailPanel ();
			}
			if (validPW == false) {
				ShowPasswordPanel ();
			}
		}
	}


	public void CallAPI() {
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

		// check for errors
		if (www.error == null) {
			
			userId = www.text;

			Debug.Log ("WWW Ok! User created.");

			// send to next screen
		} else {
			Debug.Log ("9. WWW Error: " + www.error);
		} 
	}

	public void ShowNamePanel() {
		//Debug.Log ("ShowNamePanel");
		namePanel.gameObject.SetActive (true);
	}

	public void ShowEmailPanel() {
		//Debug.Log ("ShowEmailPanel");
		emailPanel.gameObject.SetActive (true);
	}


	public void ShowPasswordPanel() {
		//Debug.Log ("ShowPasswordPanel");
		passwordPanel.gameObject.SetActive (true);
	}

	public void HideNamePanel() {
		//Debug.Log ("HideNamePanel");
		namePanel.gameObject.SetActive (false);
	}

	public void HideEmailPanel() {
		//Debug.Log ("HideEmailPanel");
		emailPanel.gameObject.SetActive (false);
	}

	public void HidePasswordPanel() {
		//Debug.Log ("HidePasswordPanel");
		passwordPanel.gameObject.SetActive (false);
	}

	
}
