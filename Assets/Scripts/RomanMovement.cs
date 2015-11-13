using UnityEngine;
using System.Collections;

public class RomanMovement : MonoBehaviour {
	Vector3 target_ = new Vector3(0,0,0);

	public float Speed = 1.0f;

	TouchManager touch_manager_;
	bool is_picked_up_ = false;
	int finger_id_ = TouchManager.NO_FINGER_ID;

	void Start () {	
		this.touch_manager_ = GameObject.Find("TouchManager").GetComponent<TouchManager>();
	}

	void Update () {
		if( this.is_picked_up_ == false ) {
			var p = this.transform.position;
			var walk_dir = (target_ - p).normalized;
			this.transform.position += walk_dir * Time.deltaTime * Speed;
		}

		if( this.is_picked_up_ ) {
			Vector3 throw_dir;
			this.transform.position = this.touch_manager_.GetUpdatedPosition(ref this.finger_id_, ref this.is_picked_up_, out throw_dir);
		}
		else {
			this.is_picked_up_ = this.touch_manager_.IsTouched(this.gameObject, out this.finger_id_);
		}
	}
}
