using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System.Linq;

public class BattleUserList : MonoBehaviour
{
	public FeedPoopCtrl fpctrl;
	private GameObject fpObject;
	public JSONNode battleJsonData;

	// User info
	private string userName;
	public ArrayList userList;

	// Blob info

	// Use this for initialization
	void Start ()
	{
		fpObject = GameObject.Find ("FEED_POOP_CTRL");
		fpctrl = (FeedPoopCtrl)fpObject.GetComponent (typeof(FeedPoopCtrl));
		battleJsonData = fpctrl.battleListResult;
		Debug.Log ("Battlejson: " + battleJsonData);
//		BuildList ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void BuildList ()
	{
//		Button[] buttons = this.GetComponentsInChildren<Button> ();

//		for (int i = 0; i < buttons.Length; i++) {
//			// for # of users returned, 
//			// add info to button i
//			ProcessJson(battleJsonData);
//		}
	}

	private void ProcessJson(JSONNode battleData)
	{
//		int count = battleData.Count;
		Debug.Log ("battledata: " + battleData);
//		JSONClass j = (JSONClass) battleData.AsObject ["user"];
//		foreach (string k in j.keys){
//			Debug.Log (k);
//			Debug.Log (j[k]);
//		}
//		JsonData jsonvale = JsonMapper.ToObject(jsonString);
//		parseJSON parsejson;
//		parsejson = new parseJSON();
//		parsejson.title = jsonvale["title"].ToString();
//		parsejson.id = jsonvale["ID"].ToString();
//
//		parsejson.but_title = new ArrayList ();
//		parsejson.but_image = new ArrayList ();
// 
//		for(int i = 0; i<jsonvale["buttons"].Count; i++)
//		{
//			parsejson.but_title.Add(jsonvale["buttons"][i]["title"].ToString());
//			parsejson.but_image.Add(jsonvale["buttons"][i]["image"].ToString());
//		}
	}
}
