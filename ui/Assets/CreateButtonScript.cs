using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 


public class CreateButtonScript : MonoBehaviour
{
	private string scene;
	private SelectBlob selectScript;
	private GameObject selectedBlue;
	private GameObject selectedOrange;
	private GameObject selectedPink;
	private GameObject selectedGreen;

	// Use this for initialization
	void Start () {
		selectedBlue = GameObject.Find("BlueBlobSelectionScriptObject");
		selectScript = (SelectBlob) selectedBlue.GetComponent(typeof(SelectBlob));
	}

	// Update is called once per frame
	void Update () {

	}

	public void chooseScene () {
		//if blue, if orange, if pink, if green
		if (selectScript.blueGuiEnable == true) {
			scene = "BlueMain";
			LoadScene (scene);
			Debug.Log ("Blue Scene load");
		} else if (selectScript.orangeGuiEnable == true) {
			scene = "OrangeMain";
			LoadScene (scene);
			Debug.Log ("Orange Scene load");
		}
	}

	public void LoadScene (string sceneName) {
		
		SceneManager.LoadScene (sceneName);
	}


}
