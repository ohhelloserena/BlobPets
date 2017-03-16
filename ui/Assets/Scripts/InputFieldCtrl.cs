using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor;

public class InputFieldCtrl : MonoBehaviour {

	public string username = "";
	public string email = "";
	public string password = "";

	//public GameObject inputField;
	public InputField Name_Field;
	public InputField Password_Field;
	public InputField Email_Field;



	// Use this for initialization
	void Start () {
		
	}
	


	public void getName (string name)
	{
		
		username = name;
		Debug.Log ("Name: " + username);
	}
		



	/*
	 * Get input email for new user.
	 * 
	 * Input(s):
	 * - pwField: new password inputted into password field.
	 */

	public void getEmail (string inputField)
	{

		email = inputField.ToLower ();
		Debug.Log ("Email: " + email);


	}

	/*
	 * Get input password for new user.
	 * 
	 * Input(s):
	 * - pwField: new password inputted into password field.
	 */

	public void getPassword (string inputField)
	{

		password = inputField;
		Debug.Log ("Password: " + password);
	}

}
