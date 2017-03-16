using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement; 
using System;
using System.Net;
using System.Text.RegularExpressions;

/* Update User Profile*/


public class EditCtrl : MonoBehaviour
{

	public int userId = -1;

	public string url;

	public string newName = "-1";
	public string newPW = "-1";
	public string confirmedPW = "-1";

	public bool nameChangeRequested = false;
	public bool pwChangeRequested = false;

	public Text matched;

	public InputFieldCtrl ifctrl;
	public ValidInputsCtrl validInputsCtrl;

	public void Start ()
	{

	}

	/* 
	If inputs are valid, calls updateUser() and then 
	go to User Profile scene.
	*/

	public void Button_Click ()
	{
		Debug.Log ("Save button clicked.");

		newName = ifctrl.username;
		newPW = ifctrl.password;

		Debug.Log ("New name: " + newName);
		Debug.Log ("new PW: " + newPW);

		CheckVariables ();

		if (nameChangeRequested || pwChangeRequested) {
			//StartCoroutine(UpdateUser());
			//LoadScene ("UserProfileUI");
		} else {
			// user didn't update username or password
			// do nothing
		}

	}

	/*
	Update user information by sending PUT request to API.
	 */

	 public void UpdateUser() {
		if (nameChangeRequested) {
		}

		if (pwChangeRequested) {
		}

		
	 }

	public void CheckVariables() {
		bool isNameEmpty = IsEmpty (newName);
		bool isPWEmpty = IsEmpty (newPW);
		bool isNameValid = validInputsCtrl.IsValidName (newName);
		bool isPWValid = validInputsCtrl.IsValidPassword (newPW);

		if (!isNameEmpty && isNameValid) {
			nameChangeRequested = true;
		}

		if (!isPWEmpty && isPWValid) {
			pwChangeRequested = true;
		}
	}


	

	public void LoadScene (string sceneName)
	{
		SceneManager.LoadScene (sceneName);
	}
		

	public bool IsEmpty(string str) {
	string noSpaces = Regex.Replace (str, @"\s+", ""); // remove whitespaces in string
		if (String.IsNullOrEmpty (noSpaces)) {
			return true;
		} else {
			return false;
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
			return true;
		} else {
			return false;
		}
	}




}