using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class LoginCtrl : MonoBehaviour {
	public InputFieldCtrl ifctrl;
	public TokenCtrl tokenCtrl;
	public ButtonCtrl buttonCtrl;

	public string email;
	public string password;
	public string token;
	public bool userExists;

	public JSONNode result;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ButtonClick()
	{
		Debug.Log ("BUTTON CLICKED");

		email = ifctrl.getEmail ();
		password = ifctrl.getPassword ();

		userExists = tokenCtrl.getUserExists ();

		LoginUser ();


	}

	public void LoginUser()
	{
		if (userExists) {
			buttonCtrl.LoadScene ("UserProfileUI");
		} else {
			
		}
	}

	private string ParseJson (string name)
	{
		return result [name].Value;
	}




}
