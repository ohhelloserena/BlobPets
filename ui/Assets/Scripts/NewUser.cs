using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Text.RegularExpressions;
using System;
using UnityEngine.UI;

// Create new user with given name, email, and password.
using System.Runtime.Remoting;
using UnityEditor;

public class NewUser : MonoBehaviour
{
	public TokenCtrl tokenCtrl;
	public string username = "";
	public string email = "";
	public string password = "";
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
	public void Start ()
	{
		HideNamePanel ();
		HideEmailPanel ();
		HidePasswordPanel ();


		
	}

	public void ButtonClick ()
	{
		Debug.Log ("1. Button clicked.");

		CreateUser ();
	}

	/*
	 * Sends post request to create new user if name, email, and 
	 * password are valid.
	 */

	public void CreateUser ()
	{
		Debug.Log ("CreateUser() entered");
		Debug.Log ("*NAME* " + username);
		Debug.Log ("*PASSWORD* " + password);
		Debug.Log ("*EMAIL* " + email);

		validName = IsValidName (username);
		validEmail = IsValidEmailAddress (email);
		validPW = IsValidPassword (password);
		//validNewUserCombo = IsValidNewUserCombo (email, password);

		Debug.Log ("2. validName: " + validName);
		Debug.Log ("3. validEmail: " + validEmail);
		Debug.Log ("4. validPW: " + validPW);
		//Debug.Log ("5. validNewUserCombo: " + validNewUserCombo);

		if (validName && validEmail && validPW) {
			Debug.Log ("6. VALID INPUTS");
			CallAPI ();
		} else {
			Debug.Log ("8. INVALID INPUTS");
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

	public void ShowNamePanel() {
		Debug.Log ("shownamepanel");
		//namePanel.SetActive (true);
	}

	public void HideNamePanel() {
		namePanel.SetActive (false);
	}

	public void ShowEmailPanel() {
		Debug.Log ("showemailpanel");
		//emailPanel.SetActive (true);
	}

	public void HideEmailPanel() {
		emailPanel.SetActive (false);
	}

	public void ShowPasswordPanel() {
		Debug.Log ("showpasswordpanel");
		//passwordPanel.SetActive (true);
	}

	public void HidePasswordPanel() {
		passwordPanel.SetActive (false);
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
		try {
			Regex rx = new Regex (
				           @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$"
			           );
			return rx.IsMatch (emailAddress);
		} catch (FormatException) {
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
 
	public void getName (string inputField)
	{
		Debug.Log ("Name: " + inputField);
		username = inputField;
	}

	/*
	 * Get input email for new user.
	 * 
	 * Input(s):
	 * - pwField: new password inputted into password field.
	 */

	public void getEmail (string inputField)
	{
		Debug.Log ("Email: " + inputField);
		email = inputField.ToLower ();
	}

	/*
	 * Get input password for new user.
	 * 
	 * Input(s):
	 * - pwField: new password inputted into password field.
	 */

	public void getPassword (string inputField)
	{
		Debug.Log ("Password: " + inputField);
		password = inputField;
	}

	
}
