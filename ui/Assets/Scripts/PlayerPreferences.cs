﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script sets/gets/resets values saved in PlayerPrefs.
 */

public class PlayerPreferences : MonoBehaviour {
	
	public string requestedBlobKey = "RequestedBlobId";	// stores the blob ID of the selected blob
	public string userKey = "UserId";	// stores the user ID for the logged in user
	public string numKey = "numBlobs";	// stores the number of blobs owned by logged in user
	public string alertKey0 = "AlertForBlob0";
	public string alertKey1 = "AlertForBlob1";
	public string alertKey2 = "AlertForBlob2";
	public string alertKey3 = "AlertForBlob3";


	/*
	 * Returns true if PlayerPrefs contains the given key.
	 */

	public bool HasKey(string keyName)
	{
		if (PlayerPrefs.HasKey (keyName)) {
			return true;
		} else {
			return false;
		}
	}

	/*
	 * Sets string value for key = "UserId".
	 */

	public void SetUser(int id)
	{
			PlayerPrefs.SetInt (userKey, id);
			PlayerPrefs.Save ();
	}

	/*
	 * Returns the string user for key = "UserId".
	 */

	public int GetUser()
	{
		if (HasKey (userKey)) {
			return PlayerPrefs.GetInt (userKey);
		} else {
			return -1;
		}
	}

	/*
	 * Resets string value for key = "UserId".
	 */ 

	public void ResetUser()
	{
		PlayerPrefs.SetInt (userKey, -1);
		PlayerPrefs.Save ();
	}
		

	/*
	 * Sets int value for key = "numBlobs".
	 */

	public void SetNumBlobs(int num)
	{
			PlayerPrefs.SetInt (numKey, num);
			PlayerPrefs.Save ();
	}

	/* Returns the int blob number for key = "numBlobs".
	 */

	public int GetNumBlobs()
	{
		if (HasKey (numKey)) {
			return PlayerPrefs.GetInt (numKey);
		} else {
			return -1;
		}
	}

	/*
	 * Resets int value for key = "numBlobs".
	 */

	public void ResetNumBlobs()
	{
		PlayerPrefs.SetInt (numKey, 0);
		PlayerPrefs.Save ();
	}

	/*
	 * Sets string value for key = "RequestedBlobId".
	 */

	public void SetRequestedBlob(string id)
	{

			PlayerPrefs.SetString (requestedBlobKey, id);
			PlayerPrefs.Save ();

	}

	/*
	 * Returns string value for key = "RequestedBlobId".
	 */

	public string GetRequestedBlob()
	{
		if (HasKey (requestedBlobKey)) {
			return PlayerPrefs.GetString (requestedBlobKey);
		} else {
			return "invalid";
		}
	}
		

	/*
	 * Resets string value for key = "RequestedBlobId".
	 */

	public void ResetRequestedBlob()
	{
		if (HasKey (requestedBlobKey)) {
			PlayerPrefs.SetString (requestedBlobKey, "-1");
			PlayerPrefs.Save ();
		}
	}

	public void SetBlobAlert(int alert, int blobNumber)
	{		
		if (blobNumber == 0) {
			PlayerPrefs.SetInt (alertKey0, alert);
		} else if (blobNumber == 1) {
			PlayerPrefs.SetInt (alertKey1, alert);
		} else if (blobNumber == 2) {
			PlayerPrefs.SetInt (alertKey2, alert);
		} else if (blobNumber == 3) {
			PlayerPrefs.SetInt (alertKey3, alert);
		}

		PlayerPrefs.Save ();
	}



}