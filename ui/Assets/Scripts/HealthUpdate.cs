using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUpdate : MonoBehaviour {
	private Image healthBar;

	public float health = 100.0f;
	private const float coef = 0.2f;

	// Use this for initialization
	void Start () {
		healthBar = GetComponent<Image>();
		healthBar.fillAmount = 1;
	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}

	void Update () {
//		health -= coef * Time.deltaTime;
		healthBar.fillAmount -= 0.0010f;
		//Debug.Log("Health : " + health);
		//Debug.Log("fill: " + healthBar.fillAmount);
	}        
}
