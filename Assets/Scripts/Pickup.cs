using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {
	private bool can_move_ = false;

	public float ThrowPower = 1;
	
	TouchManager touch_manager_;
	bool is_picked_up_ = false;
	int finger_id_ = TouchManager.NO_FINGER_ID;
	
	Vector3 throwing_ = new Vector3(0,0,0);
	float throwing_power_ = -1.0f;

	public bool CanMove {
		get {
			return this.can_move_;
		}
	}
	
	void Start () {	
		this.touch_manager_ = GameObject.Find("TouchManager").GetComponent<TouchManager>();
	}
	
	void Update () {
		this.can_move_ = false;
		if( this.throwing_power_ <= 0 ) {
			if( this.is_picked_up_ == false ) {
				this.can_move_ = true;
			}
		}
		else {
			this.transform.position += throwing_ * Time.deltaTime * this.throwing_power_;
			this.throwing_power_ -= Time.deltaTime;
			if( this.throwing_power_ <= 0.0f ) {
				this.throwing_power_ = - 10;
			}
		}
		
		if( this.is_picked_up_ ) {
			Vector3 throw_dir;
			this.transform.position = this.touch_manager_.GetUpdatedPosition(ref this.finger_id_, ref this.is_picked_up_, out throw_dir);
			
			if( this.is_picked_up_ == false ) {
				this.throwing_ = throw_dir;
				this.throwing_power_ = this.ThrowPower;
			}
		}
		else {
			this.is_picked_up_ = this.touch_manager_.IsTouched(this.gameObject, out this.finger_id_);
		}
	}
}
