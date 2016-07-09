using UnityEngine;
using System.Collections;

public class BoltController : MonoBehaviour
{
    public float speed;
	public float lifeTime; //the bolt will disappear after lifeTime seconds
	private float endTime; //exact point in time when the bolt will disappear

    void Start()
    {
		endTime = Time.time + lifeTime;
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

	void Update() {
		if (Time.time >= endTime) {
			Destroy(gameObject);
		}
	}
}
