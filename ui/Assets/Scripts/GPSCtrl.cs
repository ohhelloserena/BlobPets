using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GPSCtrl : MonoBehaviour
{

	public Text distance_label;
	public GameObject distanceObject;

	public Text end;
	public GameObject endObject;


	public double long1 = 0;
	public double lat1 = 0;

	public double long2 = 0;
	public double lat2 = 0;

	public double currLong;
	public double currLat;

	public double distanceWalked = 0;

	void Start ()
	{
		distance_label = distanceObject.GetComponent<Text> ();
		end = endObject.GetComponent<Text> ();

		distance_label.text = " ";
		end.text = " ";

	}




	/// <summary>
	/// Makes a call to start GPS service when Start button is clicked.
	/// </summary>
	public void StartButtonClicked ()
	{
		distance_label.text = "You and your blobs have walked " + Math.Round (distanceWalked, 2) + " km so far on this trip.";
		StartCoroutine (StartLocationService ());
	}

	/// <summary>
	/// Stops GPS service when Stop button is clicked.
	/// </summary>
	public void StopButtonClicked ()
	{
		Input.location.Stop ();
		end.text = "You've ended your trip. Good job! " + Math.Round (distanceWalked, 2) + " will be added to your exercise record.";
	}
		

	/// <summary>
	/// Starts the location service.
	/// </summary>
	IEnumerator StartLocationService ()
	{
		// First, check if user has location service enabled
		if (!Input.location.isEnabledByUser)
			//yield break;
			distance_label.text = "Your phone's location setting is turned off. Go to your phone's Settings -> Location to turn on this service.";
		// Start service before querying location
		Input.location.Start (5f, 5f);	// accuracy = 5m, update frequency = 5m


		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
			yield return new WaitForSeconds (1);
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1) {
			distance_label.text = "Timed out";
			yield break;
		}
			
		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed) {
			distance_label.text = "Unable to determine device location";
			yield break;
		} else {
			long1 = long2 = Input.location.lastData.longitude;
			lat1 = lat2 = Input.location.lastData.latitude;

			InvokeRepeating ("GetCurrentLocation", 10, 10);
			InvokeRepeating ("HasTraveled", 10, 10);
		}

		// Stop service if there is no need to query location updates continuously
		//Input.location.Stop ();
	}

	/// <summary>
	/// Gets the current location.
	/// </summary>
	public void GetCurrentLocation()
	{

		if (Input.location.status == LocationServiceStatus.Running) {
			lat2 = Input.location.lastData.latitude;
			long2 = Input.location.lastData.longitude;
		}
	}

	/// <summary>
	/// Calculates distance traveled by user. Sets current geo coordinates as the original geo coordinates.
	/// </summary>
	public void HasTraveled()
	{
			distanceWalked += CalculateDistance (lat1, long1, lat2, long2);
			distance_label.text = "You and your blobs have walked " + Math.Round (distanceWalked, 2) + " km so far on this trip.";
			long1 = long2;
			lat1 = lat2;
	}


	/// <summary>
	/// Calculate the distance between two geo coordinates.
	/// </summary>
	/// <returns>Distance traveled in km.</returns>
	/// <param name="latitude1">Latitude1.</param>
	/// <param name="longitude1">Longitude1.</param>
	/// <param name="latitude2">Latitude2.</param>
	/// <param name="longitude2">Longitude2.</param>
	public double CalculateDistance (double latitude1, double longitude1, double latitude2, double longitude2)
	{
		double radiansOverDegrees = (Math.PI / 180.0);
		double R = 6371;	// Earth radius in km

		double radiansLat1 = latitude1 * radiansOverDegrees;
		double radiansLong1 = longitude1 * radiansOverDegrees;

		double radiansLat2 = latitude2 * radiansOverDegrees;
		double radiansLong2 = longitude2 * radiansOverDegrees;

		double dLat = radiansLat2 - radiansLat1;
		double dLong = radiansLong2 - radiansLong1;

		double a = Math.Sin (dLat / 2) * Math.Sin (dLat / 2) +
		           Math.Sin (dLong / 2) * Math.Sin (dLong / 2) * Math.Cos (radiansLat1) * Math.Cos (radiansLat2);
		double c = 2 * Math.Atan2 (Math.Sqrt (a), Math.Sqrt (1 - a));
		double d = R * c;

		return d;
	}



}
