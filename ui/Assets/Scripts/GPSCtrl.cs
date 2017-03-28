using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;




public class GPSCtrl : MonoBehaviour
{

	public Text distance_label;
	public GameObject distanceObject;

	// *** DELETE WHEN DONE - text labels for testing
	public Text testP1;
	public Text testP2;
	public GameObject p1Object;
	public GameObject p2Object;

	public double long1;
	public double lat1;

	public double long2;
	public double lat2;

	public double currLong;
	public double currLat;

	public double distanceWalked;

	void Start ()
	{
		distance_label = distanceObject.GetComponent<Text> ();

		// *** DELETE WHEN DONE - text labels for testing
		testP1 = p1Object.GetComponent<Text> ();
		testP2 = p2Object.GetComponent<Text> ();

		distance_label.text = "You and your blobs have walked " + distanceWalked + " km.";

	}

	void Update()
	{
		

		long2 = Input.location.lastData.longitude;
		lat2 = Input.location.lastData.latitude;

		if ((long1 != long2) || (lat1 != lat2)) {
			distanceWalked += CalculateDistance (long1, lat1, long2, lat2);
			distance_label.text = "You and your blobs have walked " + distanceWalked + " km.";
			long1 = long2;
			lat1 = lat2;
		}

		testP1.text = "<Point 1 Coordinates> Long: " + long1 + ", Lat: " + lat1;
		testP2.text = "<Point 2 Coordinates> Long: " + long2 + ", Lat: " + lat2;
	}

	/// <summary>
	/// Makes a call to start GPS service when Start button is clicked.
	/// </summary>
	public void StartButtonClicked() {
		Debug.Log ("Start button clicked...");
		StartCoroutine (StartLocationService ());
	}

	/// <summary>
	/// Stops GPS service when Stop button is clicked.
	/// </summary>
	public void StopButtonClicked()
	{
		Debug.Log ("Stop button clicked...");
		Input.location.Stop ();
	}

	/// <summary>
	/// Calculate distance traveled given two points.
	/// </summary>
	/// <param name="longitude1"> Longitude coordinate of point 1</param>
	/// <param name="latitude1"> Latitude coordinate of point 1</param>
	/// <param name="longitude2"> Longitude coordinate of point 2</param>
	/// <param name="latitude2"> Latitude coordinate of point 2</param>
	public double CalculateDistance(double longitude1, double latitude1, double longitude2, double latitude2)
	{
		double radiansOverDegrees = (Math.PI / 180.0);
		double EARTH_MEAN_RADIUS = 6371;	// in km

		double radiansLong1 = longitude1 * radiansOverDegrees;
		double radiansLat1 = latitude1 * radiansOverDegrees;

		double radiansLong2 = longitude2 * radiansOverDegrees;
		double radiansLat2 = latitude2 * radiansOverDegrees;

		double dLong = radiansLong2 - radiansLong1;
		double dLat = radiansLat2 - radiansLat1;

		double dist1 = Math.Pow (Math.Sin (dLat / 2.0), 2.0) +
		                 Math.Cos (radiansLat1) * Math.Cos (radiansLat2) *
		                 Math.Pow (Math.Sin (dLong / 2.0), 2.0);

		double dist2 = EARTH_MEAN_RADIUS * 2.0 * Math.Atan2 (Math.Sqrt (dist1), Math.Sqrt (1.0 - dist1));

		return dist2;
	}

	/// <summary>
	/// Starts the location service.
	/// </summary>
	IEnumerator StartLocationService ()
	{
		// First, check if user has location service enabled
		if (!Input.location.isEnabledByUser)
			//yield break;
			distance_label.text = "Not initialized";

		// Start service before querying location
		Input.location.Start (0.5f);	// accuracy = 0.5m


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
			long1 = Input.location.lastData.longitude;
			lat1 = Input.location.lastData.latitude;

			// Access granted and location value could be retrieved
			// distance_label.text = "Location: " + Input.location.lastData.latitude + " " +
			// Input.location.lastData.longitude + " " +
			// Input.location.lastData.altitude;

		}

		// Stop service if there is no need to query location updates continuously
		//Input.location.Stop ();
	}







}
