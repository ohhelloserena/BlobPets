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
	public InputFieldCtrl ifCtrl;
	public string username;
	public string email;
	public string password;
	public string userId = "";
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
		username = ifCtrl.username;
		email = ifCtrl.email;
		password = ifCtrl.password;

		Debug.Log ("username: " + username);
		Debug.Log ("email: " + email);
		Debug.Log ("pw: " + password);



		CreateUser ();
	}

	/*
	 * Sends post request to create new user if name, email, and 
	 * password are valid.
	 */

	public void CreateUser ()
	{

		validName = IsValidName (username);
		validEmail = IsValidEmailAddress (email);
		validPW = IsValidPassword (password);

		if (validName && validEmail && validPW) {
			Debug.Log ("Valid inputs");
			CallAPI ();
		} else {
			Debug.Log ("Invalid inputs");


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
		Debug.Log ("7. CALLAPI");
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

			Debug.Log ("8. WWW Ok! User created.");

			// send to next screen
		} else {
			Debug.Log ("9. WWW Error: " + www.error);
		} 
	}


	/*
	 * Returns true if inputted name is a valid name, else false.
	 * A valid name is at least 1 character (that isn't a white space) long
	 * and no longer than 25 characters long.
	 * 
	 * Input(s):
	 * - n: new name inputted into name text field.
	 */

	public bool IsValidName(string n) 
	{
		int len = n.Length;

		string nNoSpaces = Regex.Replace (n, @"\s+", "");
		int lenNoSpaces = nNoSpaces.Length;

		if ((lenNoSpaces >= 1) && len <= 25) {
			HideNamePanel ();
			return true;
		} else {
			return false;
		}
	}

	/*
	 * Returns true if inputted email address is a valid email 
	 * address format, else false.
	 * 
	 * Input(s):
	 * - emailAddress: new email address inputted into email address text field.
	 */

	public bool IsValidEmailAddress (string emailAddress)
	{
			Regex rx = new Regex (
				           @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$"
			           );
			bool res= rx.IsMatch (emailAddress);
			if (res == true) {
				HideEmailPanel();
				return true;
			} else {
				return false;
			}
	
	}

	/*
	 * Returns true if inputted password is valid, else false.
	 * A valid password is at least 6 characters long and doesn't contain spaces.
	 * 
	 * Input(s):
	 * - pw: new password inputted into password text field.
	 */

	public bool IsValidPassword (string pw)
	{
		bool containsWhiteSpace = pw.Contains (" ");
		int len = pw.Length;

		if ((len >= 6) && !containsWhiteSpace) {
			HideEmailPanel ();
			return true;
		} else {
			return false;
		}
	}

	/*
	 * Checks to see if someone is already registered in the system under the
	 * inputted email and password combo by sending a token request with the listed info.
	 * Returns true if inputted info do not belong to an existing user, else false.
	 * 
	 * Input(s):
	 * - n: new name inputted into name text field.
	 * - emailAddress: new email address inputted into email address text field.
	 * - pw: new password inputted into password text field.
	 */

	/*
	public bool IsValidNewUserCombo (string emailAddress, string pw)
	{
		bool userExists = tokenCtrl.userExists;

		if (userExists) {
			return true;
		} else {
			return false;
		}
	}
	*/
		
	/*
	 * Get input name for new user. 
	 * 
	 * Input(s):
	 * - nameField: new name inputted into name text field.
	*/
 
	

	public void ShowNamePanel() {
		Debug.Log ("ShowNamePanel");
		namePanel.gameObject.SetActive (true);
	}

	public void ShowEmailPanel() {
		Debug.Log ("ShowEmailPanel");
		emailPanel.gameObject.SetActive (true);
	}


	public void ShowPasswordPanel() {
		Debug.Log ("ShowPasswordPanel");
		passwordPanel.gameObject.SetActive (true);
	}

	public void HideNamePanel() {
		Debug.Log ("HideNamePanel");
		namePanel.gameObject.SetActive (false);
	}

	public void HideEmailPanel() {
		Debug.Log ("HideEmailPanel");
		emailPanel.gameObject.SetActive (false);
	}

	public void HidePasswordPanel() {
		Debug.Log ("HidePasswordPanel");
		passwordPanel.gameObject.SetActive (false);
	}

	
}
