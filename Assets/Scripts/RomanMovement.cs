using UnityEngine;
using System.Collections;

public class RomanMovement : MonoBehaviour {
	Vector3 target_ = new Vector3(0,Pickup.YPOS,0);

	public float Speed = 1.0f;

	Pickup pickup_;

	Renderer renderer_;

	bool has_become_visible_ = false;

	Rigidbody body_;

	void Start () {	
		this.pickup_ = this.GetComponent<Pickup>();
		this.renderer_ = this.GetComponent<Renderer>();
		this.body_ = this.GetComponent<Rigidbody>();
		var p = this.transform.position;
		p.y = Pickup.YPOS;
		this.body_.MovePosition(p);
	}

	void Update () {
		if( pickup_.CanMove ) {
			var p = this.transform.position;
			var d = (target_ - p);
			d.y = 0;
			var walk_dir = d.normalized;
			p += walk_dir * Time.deltaTime * Speed;
			p.y = Pickup.YPOS;
			this.body_.velocity = walk_dir * Speed;
			//this.transform.position = p;
			this.transform.rotation = Quaternion.identity;
		}

		bool vis = this.renderer_.isVisible;
		if( vis ) has_become_visible_ = true;
		if( has_become_visible_ && vis == false ) {
			Destroy(this.gameObject);
		}
	}

	 public void OnTriggerEnter(Collider c) {
	//public void OnCollisionEnter(Collision c) {
		if( c.gameObject != null) {
			var roman = c.gameObject.GetComponent<RomanMovement>();
			var pickup = c.gameObject.GetComponent<Pickup>();

			if( roman != null ) {
				RomanCollision(this, this.pickup_, roman, pickup);
				RomanCollision(roman, pickup, this, this.pickup_);
			}
		}

	}

	private Vector3 pos {
		get {
			return this.transform.position;
		}
	}

	private static void RomanCollision(RomanMovement m1, Pickup p1, RomanMovement m2, Pickup p2) {
		if( p1.IsBeingThrown == false && p2.IsBeingThrown == true ) {
			var dir = (p2.ThrowingDirection.normalized + (m1.pos - m2.pos)).normalized * p2.ThrowingDirection.magnitude;
			p1.Throw(p2.SuggestedThrowingTimer, dir);
			Debug.Log("Collision");
		}
	}
}
