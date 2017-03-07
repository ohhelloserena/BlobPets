using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

// Create new user with given name, email, and password.

public class NewUser : MonoBehaviour
{

	public string username = "";
	public string email = "";
	public string password = "";
	public string userId = "";

	public string url = "http://104.131.144.86/api/users/";

	public string userToken = "";

	// Use this for initialization
	public void Start ()
	{
		
	}

	public void ButtonClick() {
		CreateUser();
	}



/*
	public void GetToken ()
	{
		string tokenUrl = "http://104.131.144.86/api/users/authenticate";
		WWWForm form = new WWWForm ();
		form.AddField ("email", "ryanchenkie@gmail.com");
		form.AddField ("password", "secret");
		WWW www = new WWW (tokenUrl, form);
		StartCoroutine (WaitForRequest (www));
	}

	IEnumerator WaitForRequest (WWW www)
	{
		yield return www;

		// check for errors
		if (www.error == null) {
			Debug.Log ("Token: " + www.text);
			Debug.Log ("WWW Ok!");

			
		} else {
			Debug.Log ("WWW Error: " + www.error);
			if (www.error == "400 Bad Request") {
				// alert for duplicate email address
			}
		}    
	}
	*/
	

	// POST request
	public void CreateUser ()
	{
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

			Debug.Log ("*New userId: " + userId);
			Debug.Log ("*Name: " + username);
			Debug.Log ("*Email: " + email);
			Debug.Log ("*Password: " + password);
		} else {
			Debug.Log ("WWW Error: " + www.error);
			if (www.error == "400 Bad Request") {
				// alert for duplicate email address
			}
		}    
	}

		
	/*
	Get input name for new user. 
	
	Input: 
	nameField: new name inputted into name text field.
	*/
 
	public void getName (string inputField)
	{
		Debug.Log ("Name: " + inputField);
		username = inputField;
	}

	/*
	Get input email for new user.

	Input: 
	- pwField: new password inputted into password field.
	 */

	public void getEmail (string inputField)
	{
		Debug.Log ("Email: " + inputField);
		email = inputField;
	}

	/*
	Get input password for new user.

	Input: 
	- pwField: new password inputted into password field.
	 */

	public void getPassword (string inputField)
	{
		Debug.Log ("Password: " + inputField);
		password = inputField;
	}

	
}
