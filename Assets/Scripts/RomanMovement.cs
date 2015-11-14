using UnityEngine;
using System.Collections;

public class RomanMovement : MonoBehaviour {
	Vector3 target_ = new Vector3(0,0,0);

	public float Speed = 1.0f;

	public float THROW_POWER = 1;

	TouchManager touch_manager_;
	bool is_picked_up_ = false;
	int finger_id_ = TouchManager.NO_FINGER_ID;
	
	Vector3 throwing_ = new Vector3(0,0,0);
	float throwing_power_ = -1.0f;

	void Start () {	
		this.touch_manager_ = GameObject.Find("TouchManager").GetComponent<TouchManager>();
	}

	void Update () {
		if( this.throwing_power_ <= 0 ) {
			if( this.is_picked_up_ == false ) {
				var p = this.transform.position;
				var walk_dir = (target_ - p).normalized;
				this.transform.position += walk_dir * Time.deltaTime * Speed;
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
				this.throwing_power_ = this.THROW_POWER;
			}
		}
		else {
			this.is_picked_up_ = this.touch_manager_.IsTouched(this.gameObject, out this.finger_id_);
		}
	}
}
