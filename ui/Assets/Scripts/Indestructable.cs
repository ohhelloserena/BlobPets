using UnityEngine;
using System.Collections;

/*
 * This script tracks the previously loaded scene.
 * 
 * Source: http://stackoverflow.com/questions/33615804/check-the-previous-loaded-scene
 */

public class Indestructable : MonoBehaviour
{
	public static Indestructable instance = null;

	// For sake of example, assume -1 indicates first scene
	public int prevScene = -1;

	void Awake ()
	{
		// If we don't have an instance set - set it now
		if (!instance)
			instance = this;
		// Otherwise, its a double, we dont need it - destroy
		else {
			Destroy (this.gameObject);
			return;
		}

		DontDestroyOnLoad (this.gameObject);
	}
}