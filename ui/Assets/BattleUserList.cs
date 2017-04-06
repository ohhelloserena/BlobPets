using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class BattleUserList : MonoBehaviour
{
	public string battleData;
	private JSONNode battleListData;

	// User info
	private string userName;
	private List<string> userList;

	//	public ArrayList userList;

	// Blob info

	// Use this for initialization
	void Start ()
	{
		battleData = PlayerPrefs.GetString ("battleUserList");
		battleListData = JSON.Parse (battleData);

		if (battleListData != null) {
			Debug.Log ("DATA IS NOT NULL");
		} else {
			Debug.Log ("DATA IS NULL");
		}
		userList = new List<string> ();
		BuildList ();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	public void BuildList ()
	{
		Button[] buttons = this.GetComponentsInChildren<Button> ();
		Debug.Log ("NUMBER OF BUTTONS: " + buttons.Length);

		ProcessJson(battleListData);

		for (int i = 0; i < buttons.Length; i++) {
			string buttonName = buttons [i].name;
			// for # of users returned, 
			// add info to button i
			GameObject.Find(buttonName).GetComponentInChildren<Text>().text = userList[i];
		}
		ProcessJson(battleListData);
	}

	private void ProcessJson (JSONNode battleData)
	{
		int count = battleData.Count;
		Debug.Log ("count: " + count);

		for (int i = 0; i < count; i++) {
			string nameValue = battleData [i] ["name"].Value;
			userList.Add (nameValue);
			Debug.Log (nameValue);
		}
	}
}
