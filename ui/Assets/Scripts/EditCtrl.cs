using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement; 
using System;
using System.Net;

/* Update User Profile*/


public class EditCtrl : MonoBehaviour
{

	public int userId = -1;

	public string url;

	public string newName = "-1";
	public string newPW = "-1";
	public string confirmedPW = "-1";

	public Text matched;

	public void Start ()
	{
		printPasswordMsg();
	}

	/* 
	If inputs are valid, calls updateUser() and then 
	go to User Profile scene.
	*/

	public void Button_Click ()
	{
		Debug.Log ("Button clicked.");
		//StartCoroutine(UpdateUser());
		LoadScene ("UserProfileUI");

	}

	/*
	Update user information by sending PUT request to API.
	 */

	 public void UpdateUser() {
		
	 }


	

	public void LoadScene (string sceneName)
	{
		SceneManager.LoadScene (sceneName);
	}


	public void printPasswordMsg ()
	{
		if (checkPW ()) {
			matched.text = "Passwords match.";
		} else {
			matched.text = "Passwords do not match.";
		}
	}


	/*
	Get new name for the user. 
	
	Input: 
	nameField: new name inputted into name text field.
	*/
 
	public void getNewName (string nameField)
	{
		Debug.Log ("New name: " + nameField);
		newName = nameField;
	}

	/*
	Get new email for the user.

	Input: 
	- pwField: new password inputted into password field.
	 */

	public void getPW (string pwField)
	{
		Debug.Log ("PW: " + pwField);
		newPW = pwField;
	}


	/*
	Get confirmed email for the user.

	Input: 
	- pwField: confirmed email inputted into confirmed password field.
	 */

	public void getConfirmedPW (string pwField)
	{
		Debug.Log ("Confirmed PW: " + pwField);
		confirmedPW = pwField;
	}


	/*
	Return false if no new name entered or if new name only includes white spaces.
	Else return true.
	 */

	public bool checkName ()
	{
		return false;
		/*
		string nameNoSpaces = newName.Replace (" ", string.empty);
		Debug.Log ("nameNoSpaces: " + nameNoSpaces);
		if (newName == "-1" || nameNoSpaces.Length < 1) {
			return false;
		} else {
			return true;
		}
		*/
	}

	/*
	Return false if no new password entered, if user doesn't fill out both password fields,
	or if input for both fields don't match.
	Else return true.
	 */

	public bool checkPW ()
	{
		if (newPW == "-1" || confirmedPW == "-1" || newPW != confirmedPW) {
			return false;
		} else {
			return true;
		}
	}



}