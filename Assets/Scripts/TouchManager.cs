using UnityEngine;
using System.Collections.Generic;

public class TouchManager : MonoBehaviour {

	public const int NO_FINGER_ID = -1;

	public Vector3 GetUpdatedPosition(ref int finger_id, ref bool keep, out Vector3 dir) {
		foreach(var t in Touches) {
			if( t.fingerId != finger_id ) continue;

			if( t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary ) {
				dir = new Vector3(0,0,0);
				Debug.Log ("Moving player");
				return Camera.main.ScreenToWorldPoint(t.position);
			}
			else if( t.phase == TouchPhase.Canceled || t.phase == TouchPhase.Ended ) {
				keep = false;
				var d = t.deltaPosition;
				dir = new Vector3(d.x, 0, d.y);
				Debug.Log("Dropping player");
				return Camera.main.ScreenToWorldPoint(t.position);
			}
		}

		// BUG - touch not found
		dir = new Vector3(0,0,0);
		return new Vector3();
	}

	public bool IsTouched(GameObject position, out int finger_id) {
		foreach(var t in Touches) {
			if( t.phase == TouchPhase.Began ) {
				Debug.Log ("Touching somewhere");
				var ray = Camera.main.ScreenPointToRay(t.position);
				RaycastHit hit;
				GameObject o = null;
				if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
					if( hit.collider ) {
						o = hit.collider.gameObject;
						Debug.Log(string.Format("Found col {0}", o.name));
					}
					else {
						Debug.Log("no col");
					}

					if( hit.rigidbody != null ) {
						o = hit.rigidbody.gameObject;
						Debug.Log(string.Format("Found rb {0}", o.name));
					}
					else {
						Debug.Log("No rb");
					}
				}
				if( o == position ) {
					Debug.Log("Picked up");
					finger_id = t.fingerId;
					return true;
				}
			}
		}

		finger_id = NO_FINGER_ID;
		return false;
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
