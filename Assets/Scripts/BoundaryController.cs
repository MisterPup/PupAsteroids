using UnityEngine;
using System.Collections;

public class BoundaryController : MonoBehaviour
{
//    public GameObject[] destroyableObjects;

	float worldXSize;
	float worldZSize;

	private Boundary boundary;

	void Start() 
	{
		worldXSize = gameObject.transform.localScale.x;
		worldZSize = gameObject.transform.localScale.z;

		//setting boundaries. We take for granted that the world is centered in x,z = (0,0)
		float boundaryXMin = -worldXSize / 2;
		float boundaryXMax = worldXSize / 2;
		float boundaryZMin = -worldZSize / 2;
		float boundaryZMax = worldZSize / 2;

		boundary = new Boundary (boundaryXMin, boundaryXMax, boundaryZMin, boundaryZMax);
	}

    // Delete destroyable object when they leave the boundaries
    void OnTriggerExit(Collider other)
    {
		Debug.Log (other);

		Vector3 otherPosition = other.transform.position; 
		float otherXPosition = otherPosition.x;
		float otherYPosition = otherPosition.y; //we do not modify it
		float otherZPosition = otherPosition.z;

		if (otherXPosition >= boundary.xMax)
		{
			otherXPosition -= worldXSize;
		}                
		else if (otherXPosition <= boundary.xMin)
		{
			otherXPosition += worldXSize;
		}
		if (otherZPosition >= boundary.zMax)
		{
			otherZPosition -= worldZSize;
		}                
		else if (otherZPosition <= boundary.zMin)
		{
			otherZPosition += worldZSize;
		}                

		other.transform.position = new Vector3
			(
				otherXPosition,
				otherYPosition,
				otherZPosition
			);

//        foreach (GameObject destroyableObj in destroyableObjects)
//        {
//            if (other.gameObject.CompareTag(destroyableObj.tag))
//            {
//                Destroy(other.gameObject);
//                break;
//            }
//        }
    }
}


public class Boundary
{
	public float xMin, xMax, zMin, zMax;

	public Boundary(float xMin, float xMax, float zMin, float zMax) 
	{
		this.xMin = xMin;
		this.xMax = xMax;
		this.zMin = zMin;
		this.zMax = zMax;
	}
}