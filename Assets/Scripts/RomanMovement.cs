using UnityEngine;
using System.Collections;

public class RomanMovement : MonoBehaviour {
	Vector3 target_ = new Vector3(0,0,0);

	public float Speed = 1.0f;

	Pickup pickup_;

	void Start () {	
		this.pickup_ = this.GetComponent<Pickup>();
	}

	void Update () {
		if( pickup_.CanMove ) {
			var p = this.transform.position;
			var walk_dir = (target_ - p).normalized;
			this.transform.position += walk_dir * Time.deltaTime * Speed;
		}
	}
}
