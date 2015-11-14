using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var players = GameObject.FindGameObjectsWithTag("Player").Length;
		if( players <= 0 ) {
			Debug.Log("No circles left");
			Application.LoadLevel("Death");
		}
	}
}
