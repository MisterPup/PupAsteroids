using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public float speed;
	public float tilt;
	public float rotationSpeed;
	public float speedDecrement;
	public bool isBounded;

	float currentRotation = 0.0f;
	float currentSpeed = 0.0f;

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	private float nextFire;

	//Rotate player if horizontal buttons are pressed
	private void rotate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		bool isHorizontalPressed = Input.GetButton ("Horizontal");

		if (isHorizontalPressed) {
			float rotationDirection = Mathf.Sign (moveHorizontal);
			currentRotation += rotationSpeed * rotationDirection;
			GetComponent<Rigidbody> ().rotation = Quaternion.Euler (0.0f, currentRotation, 0.0f);
		}
	}

	//Move player if vertical buttons are pressed.
	//if not, move by inertia and slowly decelerate
	private void move ()
	{
		bool isVerticalPressed = Input.GetButton ("Vertical");

		if (isVerticalPressed) {
			GetComponent<Rigidbody> ().velocity = transform.forward * speed;
			currentSpeed = speed; //save value for when there is no speed at all
		} else {
			float newSpeed = currentSpeed - speedDecrement;
			currentSpeed = newSpeed > 0 ? newSpeed : 0.0f;
			GetComponent<Rigidbody> ().velocity = transform.forward * currentSpeed;
			//Vector3 zeroVelocity = new Vector3(0.0f, 0.0f, 0.0f);
			//GetComponent<Rigidbody>().velocity = zeroVelocity;
		}
	}

	//Shoot if Fire button is pressed
	private void fire ()
	{
		if (Input.GetButton ("Fire1") && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
			//play fire sound
			AudioSource audioSource = GetComponent<AudioSource>();
			audioSource.Play ();
		}
	}

	void FixedUpdate ()
	{
		rotate ();
		move ();
	}

	void Update ()
	{
		fire ();
	}

}