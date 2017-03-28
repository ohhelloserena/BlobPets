using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Threading;
using UnityEngine.Networking;

public class ExerciseCtrl : MonoBehaviour
{

	public Text steps_label;
	public GameObject stepsGO;


	/*
	void Start ()
	{

		steps_label = stepsGO.GetComponent<Text> ();
		steps_label.text = "0";


	}
	*/






	IEnumerator Start ()
	{
		steps_label = stepsGO.GetComponent<Text> ();
		steps_label.text = "0";


		// First, check if user has location service enabled
		if (!Input.location.isEnabledByUser)
			yield break;

		// Start service before querying location
		Input.location.Start ();

		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
			yield return new WaitForSeconds (1);
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1) {
			steps_label.text = "Timed out";
			yield break;
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed) {
			steps_label.text = "Unable to determine device location";
			yield break;
		} else {
			// Access granted and location value could be retrieved
			steps_label.text = "Location: " + Input.location.lastData.latitude + " " + 
				Input.location.lastData.longitude + " " + 
				Input.location.lastData.altitude + " " + 
				Input.location.lastData.horizontalAccuracy + " " + 
				Input.location.lastData.timestamp;
		}

		// Stop service if there is no need to query location updates continuously
		Input.location.Stop ();
	}

	void Update ()
	{

	}
}


	
