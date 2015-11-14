using UnityEngine;
using System.Collections;

public class RomanSpawner : MonoBehaviour {
	public GameObject Roman;

	public float Distance = 20.0f;
	public float ExtraRomanDistance = 5.0f;
	public float SpawnInterval = 1.0f;

	public float TotalRomanDistance {
		get {
			return this.Distance + this.ExtraRomanDistance;
		}
	}

	float spawn_timer_ = 0.0f;

	public int MaxCount = 20;

	private void Spawn() {
		var roman = GameObject.Instantiate(Roman);
		var r = Random.insideUnitCircle.normalized;
		roman.transform.position = new Vector3(r.x, Pickup.YPOS, r.y) * this.Distance;
	}

	void Update () {
		this.spawn_timer_ -= Time.deltaTime;
		if( this.spawn_timer_ <= 0 ) {
			var count = GameObject.FindGameObjectsWithTag("Enemy").Length;
			if( count < this.MaxCount ) {
				this.Spawn();
				this.spawn_timer_ += this.SpawnInterval;
			}
		}

	}
}
