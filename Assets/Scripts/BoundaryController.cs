using UnityEngine;
using System.Collections;

public class BoundaryController : MonoBehaviour
{
    public GameObject[] destroyableObjects;

    // Delete destroyable object when they leave the boundaries
    void OnTriggerExit(Collider other)
    {
        foreach (GameObject destroyableObj in destroyableObjects)
        {
            if (other.gameObject.CompareTag(destroyableObj.tag))
            {
                Destroy(other.gameObject);
                break;
            }
        }
    }
}
