using UnityEngine;
using System.Collections;

public class RestartLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		foreach(var t in TouchManager.Touches) {
			if( t.phase == TouchPhase.Began ) {
				Application.LoadLevel("MainScene");
			}
		}
	}
}
