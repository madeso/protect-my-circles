using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {
	private bool can_move_ = false;

	public float MaxThrowTimer = 1;
	
	TouchManager touch_manager_;
	bool is_picked_up_ = false;
	int finger_id_ = TouchManager.NO_FINGER_ID;

	public bool CanBeThrown = true;
	
	Vector3 throwing_direction_ = new Vector3(0,0,0);
	float throwing_timer_ = -1.0f;

	public bool CanMove {
		get {
			return this.can_move_;
		}
	}

	public bool IsBeingThrown {
		get {
			return this.throwing_timer_ > 0;
		}
	}
	
	void Start () {	
		this.touch_manager_ = GameObject.Find("TouchManager").GetComponent<TouchManager>();
	}
	
	void Update () {
		this.can_move_ = false;
		if( this.throwing_timer_ <= 0 ) {
			if( this.is_picked_up_ == false ) {
				this.can_move_ = true;
			}
		}
		else {
			this.transform.position += throwing_direction_ * Time.deltaTime * this.throwing_timer_;
			this.throwing_timer_ -= Time.deltaTime;
			if( this.throwing_timer_ <= 0.0f ) {
				this.throwing_timer_ = - 10;
			}
		}
		
		if( this.is_picked_up_ ) {
			Vector3 throw_dir;
			this.transform.position = this.touch_manager_.GetUpdatedPosition(ref this.finger_id_, ref this.is_picked_up_, out throw_dir);
			
			if( this.is_picked_up_ == false ) {
				if( this.CanBeThrown ) {
					this.throwing_direction_ = throw_dir;
					this.throwing_timer_ = this.MaxThrowTimer;
				}
			}
		}
		else {
			this.is_picked_up_ = this.touch_manager_.IsTouched(this.gameObject, out this.finger_id_);
		}
	}
}
