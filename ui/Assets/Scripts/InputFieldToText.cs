using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class InputFieldToText : MonoBehaviour {

	//	// Use this for initialization
	//	void Start () {
	//		
	//	}
	//	
	//	// Update is called once per frame
	//	void Update () {
	//		
	//	}
	public string blobName = "";

	public InputField Field;
	public Text TextBox;

	public string url = "http://104.131.144.86/api/users/";

	//	public void setName() {
	//		blobName = TextBox.text;
	////		Debug.Log ("blobname: " + blobName); WORKS
	//	}

	// POST request
	public void CreateBlob () {
		blobName = TextBox.text;
		Debug.Log ("blobname: " + blobName);
		StartCoroutine (BlobPost ());
	}

	IEnumerator BlobPost () {
		WWWForm form = new WWWForm();
		form.AddField("name", blobName);
		UnityWebRequest www = UnityWebRequest.Post(url, form);
		yield return www.Send();

		if(www.isError) {
			Debug.Log(www.error);
		}
		else {
			Debug.Log("Form upload complete!");
		}
	}
}
