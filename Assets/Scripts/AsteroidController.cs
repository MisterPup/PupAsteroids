using UnityEngine;
using System.Collections;

public class AsteroidController : MonoBehaviour
{

    public float tumble;
    public GameObject[] destroyableObjects;
    public int numberOfFragments;

    void Start()
    {
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble; //random 3d rotation
    }

    void OnTriggerEnter(Collider other)
    {
        foreach (GameObject destroyableObj in destroyableObjects)
        {
            if (other.gameObject.CompareTag(destroyableObj.tag))
            {
                createMiniAsteroids();
                destroyObjects(other);


                break;
            }
        }
    }

    private void destroyObjects(Collider other)
    {
        Destroy(other.gameObject);
        Destroy(gameObject);
    }

    //When the asteroid in destroyed, create "numberOfFragments" mini asteroids, half the size of its parent
    private void createMiniAsteroids()
    {
        for (int i = 0; i < 4; i++)
        {

        }

        GameObject newAsteroid1 = cloneAsteroid();
        Rigidbody newAsteroid1Rb = newAsteroid1.GetComponent<Rigidbody>();
        newAsteroid1Rb.velocity = new Vector3(1.0f, 0.0f, 1.0f);

        GameObject newAsteroid2 = cloneAsteroid();
        Rigidbody newAsteroid2Rb = newAsteroid2.GetComponent<Rigidbody>();
        newAsteroid2Rb.velocity = new Vector3(-1.0f, 0.0f, 1.0f);

        GameObject newAsteroid3 = cloneAsteroid();
        Rigidbody newAsteroid3Rb = newAsteroid3.GetComponent<Rigidbody>();
        newAsteroid3Rb.velocity = new Vector3(-1.0f, 0.0f, -1.0f);

        GameObject newAsteroid4 = cloneAsteroid();
        Rigidbody newAsteroid4Rb = newAsteroid4.GetComponent<Rigidbody>();
        newAsteroid4Rb.velocity = new Vector3(1.0f, 0.0f, -1.0f);
    }

    private GameObject cloneAsteroid()
    {
        GameObject newAsteroid = (GameObject)Instantiate(gameObject, transform.position, transform.rotation);
        Rigidbody newAsteroidRb = newAsteroid.GetComponent<Rigidbody>();
        Vector3 newAsteroidsScale = newAsteroidRb.transform.localScale;
        newAsteroidRb.transform.localScale = newAsteroidsScale * 0.5f; //halve the size
        return newAsteroid;
    }
}
