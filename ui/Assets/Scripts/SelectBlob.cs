using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectBlob : MonoBehaviour {
	public Texture selectTexture;
	public Button button;
	private float xCoord;
	private float yCoord;
	private float width;
	private float height;
	private bool guiEnable = false;

	// Use this for initialization
	void Start () {
//		button = GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setTrue () {
		guiEnable = true;
		Debug.Log ("CLICKED");
	}

	void OnGUI() {
		if (!selectTexture) {
			Debug.LogError("Assign a Texture in the inspector.");
			return;
		}
		//		xCoord = button.GetComponent<RectTransform>().position.x;
		if (GameObject.Find("BlueBlobSelectButton") && guiEnable == true) {
			xCoord = 30;
			yCoord = 110;
			GUI.DrawTexture (new Rect (xCoord, yCoord, 113, 79), selectTexture, ScaleMode.StretchToFill, true, 10.0F);
		}
	}

//	public void selectBlob() {
//		if (!selectTexture) {
//			Debug.LogError("Assign a Texture in the inspector.");
//			return;
//		}
////		xCoord = button.GetComponent<RectTransform>().position.x;
//		xCoord = -56.5f;
//		yCoord = 90;
//		GUI.DrawTexture(new Rect(xCoord, yCoord, 113, 79), selectTexture, ScaleMode.ScaleToFit, true, 10.0F);
//	}
}
