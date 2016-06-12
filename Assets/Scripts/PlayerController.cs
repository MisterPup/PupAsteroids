using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float tilt;
    public float rotationSpeed;
    public float speedDecrement;
    public bool isBounded;
    public Boundary boundary;

    float currentRotation = 0.0f;
    float currentSpeed = 0.0f;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    private float nextFire;

    //Rotate player if horizontal buttons are pressed
    private void rotate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        bool isHorizontalPressed = Input.GetButton("Horizontal");

        if (isHorizontalPressed)
        {
            float rotationDirection = Mathf.Sign(moveHorizontal);
            currentRotation += rotationSpeed * rotationDirection;
            GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, currentRotation, 0.0f);
        }
    }

    //Move player if vertical buttons are pressed.
    //if not, move by inertia and slowly decelerate
    private void move()
    {
        bool isVerticalPressed = Input.GetButton("Vertical");

        if (isVerticalPressed)
        {
            GetComponent<Rigidbody>().velocity = transform.forward * speed;
            currentSpeed = speed; //save value for when there is no speed at all
        }
        else
        {
            float newSpeed = currentSpeed - speedDecrement;
            currentSpeed = newSpeed > 0 ? newSpeed : 0.0f;
            GetComponent<Rigidbody>().velocity = transform.forward * currentSpeed;
            //Vector3 zeroVelocity = new Vector3(0.0f, 0.0f, 0.0f);
            //GetComponent<Rigidbody>().velocity = zeroVelocity;
        }
    }

    //Handle behavior when the player reaches world boundaries
    private void handleBoundaries()
    {
        if (isBounded) //are we bounded by the world limits?
        {
            GetComponent<Rigidbody>().position = new Vector3
            (
                Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
                0.0f,
                Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
            );
        }
        else //not bounded, teletransport player to the other side of the world if boundaries are overpassed
        {
            float xPosition = GetComponent<Rigidbody>().position.x;
            float zPosition = GetComponent<Rigidbody>().position.z;

            float worldXLength = Mathf.Abs(boundary.xMax) + Mathf.Abs(boundary.xMin);
            float worldZLength = Mathf.Abs(boundary.zMax) + Mathf.Abs(boundary.zMin);

            if (xPosition >= boundary.xMax)
                xPosition -= worldXLength;
            else if (xPosition <= boundary.xMin)
                xPosition += worldXLength;

            if (zPosition >= boundary.zMax)
                zPosition -= worldZLength;
            else if (zPosition <= boundary.zMin)
                zPosition += worldZLength;

            GetComponent<Rigidbody>().position = new Vector3
            (
                xPosition,
                0.0f,
                zPosition
            );
        }
    }

    //Shoot if Fire button is pressed
    private void fire()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        }
    }

    void FixedUpdate()
    {
        rotate();
        move();
        handleBoundaries();
    }

    void Update()
    {
        fire();
    }

}