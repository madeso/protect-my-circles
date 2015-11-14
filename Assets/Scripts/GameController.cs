using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public float RomanTimer = 3.0f;
	float roman_timer_ = 0;

	void Start() {
		this.roman_timer_ = this.RomanTimer;
	}
	
	// Update is called once per frame
	void Update () {
		var players = GameObject.FindGameObjectsWithTag("Player").Length;
		if( players <= 0 ) {
			// Debug.Log("No circles left");
			Application.LoadLevel("Death");
		}

		this.roman_timer_ -= Time.deltaTime;
		if( this.roman_timer_ <= 0 ) {
			this.roman_timer_ += this.RomanTimer;
			var romans = GameObject.FindGameObjectsWithTag("Enemy");
			var r = romans[Random.Range(0, romans.Length)];
			r.GetComponent<RomanMovement>().RecieveSpeedBonus();
		}
	}
}
