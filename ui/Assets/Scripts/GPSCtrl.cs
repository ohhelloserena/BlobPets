using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPSCtrl : MonoBehaviour
{

	public Text distance_label;
	public GameObject distanceObject;

	void Start ()
	{
		distance_label = distanceObject.GetComponent<Text> ();
		distance_label.text = "0";

		StartCoroutine (StartLocationService ());
	}

	IEnumerator StartLocationService ()
	{
		// First, check if user has location service enabled
		if (!Input.location.isEnabledByUser)
			//yield break;
			distance_label.text = "Not initialized";

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
			distance_label.text = "Timed out";
			//yield break;
		}


		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed) {
			distance_label.text = "Unable to determine device location";
			//yield break;
		} else {
			// Access granted and location value could be retrieved
			distance_label.text = "Location: " + Input.location.lastData.latitude + " " +
			Input.location.lastData.longitude + " " +
			Input.location.lastData.altitude + " " +
			Input.location.lastData.horizontalAccuracy + " " +
			Input.location.lastData.timestamp;
		}

		// Stop service if there is no need to query location updates continuously
		Input.location.Stop ();
	}



	/*
	IEnumerator Start ()
	{
		distance_label = distanceObject.GetComponent<Text> ();
		distance_label.text = "0";

		// First, check if user has location service enabled
		if (!Input.location.isEnabledByUser)
			//yield break;
			distance_label.text = "Not initialized";

		// Start service before querying location
		Input.location.Start (1.0f,1.0f);


		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
			yield return new WaitForSeconds (1);
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1) {
			distance_label.text = "Timed out";
			//yield break;
		}


		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed) {
			distance_label.text = "Unable to determine device location";
			//yield break;
		} else {
			// Access granted and location value could be retrieved
			distance_label.text = "Location: " + Input.location.lastData.latitude + " " +
			Input.location.lastData.longitude + " " +
			Input.location.lastData.altitude + " " +
			Input.location.lastData.horizontalAccuracy + " " +
			Input.location.lastData.timestamp;
		}

		// Stop service if there is no need to query location updates continuously
		Input.location.Stop ();
	}

	// Use this for initialization

	// Update is called once per frame
	void Update ()
	{
		

	}
	*/
}
