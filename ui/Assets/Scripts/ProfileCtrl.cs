using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ProfileCtrl : MonoBehaviour {

	public string url = "http://104.131.144.86/api/users/";
	public string username = "-1";
	public string numWins = "-1";
	public int numBlobs = -1;

	public string blobName0 = "Locked!";
	public string blobName2 = "Locked!";
	public string blobName3 = "Locked!";


	public int blobId0 = -1;
	public int blobId1 = -1;
	public int blobId2 = -1;
	public int blobId3 = -1;

	public int sceneNumber = 1;




	// Use this for initialization
	void Start () {

		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}