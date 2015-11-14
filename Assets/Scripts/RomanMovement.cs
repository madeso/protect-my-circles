using UnityEngine;
using System.Collections;

public class RomanMovement : MonoBehaviour {
	Vector3 target_ = new Vector3(0,0,0);

	public float Speed = 1.0f;

	Pickup pickup_;

	Renderer renderer_;

	bool has_become_visible_ = false;

	void Start () {	
		this.pickup_ = this.GetComponent<Pickup>();
		this.renderer_ = this.GetComponent<Renderer>();
	}

	void Update () {
		if( pickup_.CanMove ) {
			var p = this.transform.position;
			var walk_dir = (target_ - p).normalized;
			this.transform.position += walk_dir * Time.deltaTime * Speed;
		}

		bool vis = this.renderer_.isVisible;
		if( vis ) has_become_visible_ = true;
		if( has_become_visible_ && vis == false ) {
			Destroy(this.gameObject);
			Debug.Log("Killed roman");
		}
	}
}
