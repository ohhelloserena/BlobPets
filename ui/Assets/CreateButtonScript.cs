using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 


public class CreateButtonScript : MonoBehaviour
{
	private string scene;

	private SelectBlob selectBlueScript;
	private SelectBlob selectOrangeScript;

	private GameObject selectedBlue;
	private GameObject selectedOrange;
	private GameObject selectedPink;
	private GameObject selectedGreen;

	// Use this for initialization
	void Start () {
		selectedBlue = GameObject.Find("BlueBlobSelectionScriptObject");
		selectBlueScript = (SelectBlob) selectedBlue.GetComponent(typeof(SelectBlob));

		selectedOrange = GameObject.Find ("OrangeBlobSelectionScriptObject");
		selectOrangeScript = (SelectBlob)selectedOrange.GetComponent (typeof(SelectBlob));

	}

	// Update is called once per frame
	void Update () {

	}

//	public void chooseScene () {
//		//if blue, if orange, if pink, if green
//		if (selectScript.blueGuiEnable == true) {
//			scene = "BlueMain";
//			LoadScene (scene);
//			Debug.Log ("Blue Scene load");
//		} else if (selectScript.orangeGuiEnable == true) {
//			scene = "OrangeMain";
//			LoadScene (scene);
//			Debug.Log ("Orange Scene load");
//		}
//	}

	public void LoadScene () {
		if (selectBlueScript.blueGuiEnable == true) {
			scene = "BlueMain";
			Debug.Log (selectBlueScript.blueGuiEnable);
			Debug.Log ("Blue Scene load");
		}

		if (selectOrangeScript.orangeGuiEnable == true) {
			scene = "OrangeMain";
			Debug.Log (selectOrangeScript.orangeGuiEnable);
			Debug.Log ("Orange Scene load");
		}
//
//		if (selectScript.pinkGuiEnable == true) {
//			scene = "BlueMain";
//			Debug.Log (selectScript.pinkGuiEnable);
//			Debug.Log ("Pink Scene load");
//		}
//
//		if (selectScript.greenGuiEnable == true) {
//			scene = "BlueMain";
//			Debug.Log (selectScript.greenGuiEnable);
//			Debug.Log ("Green Scene load");
//		}

		SceneManager.LoadScene (scene);
	}


}
