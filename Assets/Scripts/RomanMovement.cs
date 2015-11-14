using UnityEngine;
using System.Collections;

public class RomanMovement : MonoBehaviour {
	public float Speed = 1.0f;
	public float ExtraSpeed = 3.0f;

	Pickup pickup_;

	Renderer renderer_;

	bool has_become_visible_ = false;

	Rigidbody body_;

	RomanSpawner spawner_;

	AudioSource audio_;

	// stolen from the unity documentation... yes I am a lazy person
	GameObject FindClosest(string tag) {
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag(tag);
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject go in gos) {
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closest = go;
				distance = curDistance;
			}
		}
		return closest;
	}

	Vector3 TargetPosition {
		get {
			var player = FindClosest("Player");
			if( player == null ) return new Vector3(0,0,0);
			return player.transform.position;
		}
	}

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

	private float extra_speed_ = 0;
	public void RecieveSpeedBonus() {
		this.extra_speed_ = 1;
	}

	void Update () {
		if( pickup_.CanMove ) {
			var p = this.transform.position;
			var d = (this.TargetPosition - p);
			d.y = 0;
			var walk_dir = d.normalized;
			var s = this.Speed;
			if( this.extra_speed_ > 0 ) {
				s += this.extra_speed_ * this.ExtraSpeed;
				this.extra_speed_ -= Time.deltaTime;
			}
			this.body_.velocity = walk_dir * s;
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
		var g = c.gameObject;
		if( g == null) return;

		if( g.CompareTag("Enemy") ) {
			var roman = g.GetComponent<RomanMovement>();
			var pickup = g.GetComponent<Pickup>();

			if( roman != null ) {
				var collision = RomanCollision(this, this.pickup_, roman, pickup) ||
				RomanCollision(roman, pickup, this, this.pickup_);
				if( collision ) {
					this.audio_.PlayOneShot(this.SndCollision);
				}
			}
		}
		else if( g.CompareTag("Player") ) {
			if( this.pickup_.IsBeingThrown ) return;
			if( this.pickup_.IsPickedUp ) return;
			Debug.Log ("Player died");
			this.audio_.PlayOneShot(this.SndPlayerDeath);
			Destroy(g);
		}
	}

	private Vector3 pos {
		get {
			return this.transform.position;
		}
	}

	public AudioClip SndDie;
	public AudioClip SndCollision;
	public AudioClip SndPlayerDeath;

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
