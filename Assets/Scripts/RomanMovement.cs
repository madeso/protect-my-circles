using UnityEngine;
using System.Collections;

public class RomanMovement : MonoBehaviour {
	Vector3 target_ = new Vector3(0,Pickup.YPOS,0);

	public float Speed = 1.0f;

	Pickup pickup_;

	Renderer renderer_;

	bool has_become_visible_ = false;

	Rigidbody body_;

	RomanSpawner spawner_;

	AudioSource audio_;

	void Start () {
		this.spawner_ = GameObject.Find("RomanSpawner").GetComponent<RomanSpawner>();
		this.pickup_ = this.GetComponent<Pickup>();
		this.renderer_ = this.GetComponent<Renderer>();
		this.body_ = this.GetComponent<Rigidbody>();
		var p = this.transform.position;
		p.y = Pickup.YPOS;
		this.body_.MovePosition(p);
		this.audio_ = GameObject.Find ("Audio").GetComponent<AudioSource>();
	}

	bool Visible {
		get {
			return this.transform.position.sqrMagnitude < this.spawner_.TotalRomanDistance * this.spawner_.TotalRomanDistance;
			// return this.renderer_.isVisible;
		}
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

		if( this.pickup_.IsPickedUp == false ) {
			bool vis = this.Visible;
			if( vis ) has_become_visible_ = true;
			if( has_become_visible_ && vis == false ) {
				Destroy(this.gameObject);
				audio_.PlayOneShot(this.SndDie);
			}
		}
	}

	 public void OnTriggerEnter(Collider c) {
		if( c.gameObject != null) {
			var roman = c.gameObject.GetComponent<RomanMovement>();
			var pickup = c.gameObject.GetComponent<Pickup>();

			if( roman != null ) {
				var collision = RomanCollision(this, this.pickup_, roman, pickup) ||
				RomanCollision(roman, pickup, this, this.pickup_);
				if( collision ) {
					this.audio_.PlayOneShot(this.SndCollision);
				}
			}
		}

	}

	private Vector3 pos {
		get {
			return this.transform.position;
		}
	}

	public AudioClip SndDie;
	public AudioClip SndCollision;

	private static bool RomanCollision(RomanMovement m1, Pickup p1, RomanMovement m2, Pickup p2) {
		if( p1.IsBeingThrown == false && p2.IsBeingThrown == true ) {
			var dir = (p2.ThrowingDirection.normalized + (m1.pos - m2.pos)).normalized * p2.ThrowingDirection.magnitude;
			p1.Throw(p2.SuggestedThrowingTimer, dir);
			Debug.Log("Collision");
			return true;
		}

		return false;
	}
}
