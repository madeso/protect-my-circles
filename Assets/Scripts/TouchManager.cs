using UnityEngine;
using System.Collections.Generic;

public class TouchManager : MonoBehaviour {

	public const int NO_FINGER_ID = -1;

	public float TOUCH_MAG = 1.0f;


	public Vector3 GetUpdatedPosition(ref int finger_id, ref bool keep, out Vector3 dir) {
		foreach(var t in Touches) {
			if( t.fingerId != finger_id ) continue;

			var suggested_pos = Camera.main.ScreenToWorldPoint(t.position);
			suggested_pos.y = 0.125f;

			if( t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary ) {
				dir = new Vector3(0,0,0);
				return SuggestedPosition(t);
			}
			else if( t.phase == TouchPhase.Canceled || t.phase == TouchPhase.Ended ) {
				keep = false;
				var d = t.deltaPosition;
				dir = new Vector3(d.x, 0, d.y);
				return SuggestedPosition(t);
			}
		}

		// BUG - touch not found
		dir = new Vector3(0,0,0);
		return new Vector3();
	}

	public bool IsTouched(GameObject position, out int finger_id) {
		foreach(var t in Touches) {
			if( t.phase == TouchPhase.Began ) {
				Vector3 suggested_pos = SuggestedPosition(t);
				if( (suggested_pos-position.transform.position).sqrMagnitude < TOUCH_MAG * TOUCH_MAG ) {
					finger_id = t.fingerId;
					return true;
				}
			}
		}

		finger_id = NO_FINGER_ID;
		return false;
	}

	Vector3 SuggestedPosition(Touch t) {
		var suggested_pos = Camera.main.ScreenToWorldPoint(t.position);
		suggested_pos.y = 0.125f;
		return suggested_pos;
	}

	private static IEnumerable<Touch> Touches {
		get {
			for(int touch_id=0; touch_id < Input.touchCount; ++touch_id) {
				var t = Input.GetTouch(touch_id);
				yield return t;
			}
		}
	}
}
