using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 


public class CreateButtonScript : MonoBehaviour
{
	private string scene;

	private SelectBlob selectBlueScript;
	private SelectBlob selectOrangeScript;
	private SelectBlob selectPinkScript;
	private SelectBlob selectGreenScript;

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

		selectedPink = GameObject.Find ("PinkBlobSelectionScriptObject");
		selectPinkScript = (SelectBlob)selectedPink.GetComponent (typeof(SelectBlob));

		selectedGreen = GameObject.Find ("GreenBlobSelectionScriptObject");
		selectGreenScript = (SelectBlob)selectedGreen.GetComponent (typeof(SelectBlob));
	}

	// Update is called once per frame
	void Update () {

	}

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

		if (selectPinkScript.pinkGuiEnable == true) {
			scene = "PinkMain";
			Debug.Log (selectPinkScript.pinkGuiEnable);
			Debug.Log ("Pink Scene load");
		}

		if (selectGreenScript.greenGuiEnable == true) {
			scene = "GreenMain";
			Debug.Log (selectGreenScript.greenGuiEnable);
			Debug.Log ("Green Scene load");
		}

		SceneManager.LoadScene (scene);
	}


}
