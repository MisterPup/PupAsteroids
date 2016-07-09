using UnityEngine;
using System.Collections;

//every asteroid instance has this script attached
public class AsteroidController : MonoBehaviour
{
    public float tumble;
    public int numberOfFragments;
    public GameObject[] destroyableObjects;

	public int maxDivision;
	private int numDivision = 0;
	    
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

	private void setNumDivision(int numDivision) {
		this.numDivision = numDivision;
	}

    private void destroyObjects(Collider other)
    {
        Destroy(other.gameObject);
        Destroy(gameObject);
    }

	private int getRandomValueForComponent() 
	{
		int[] possibileValues = new int[2] {-1 , 1};

		int randomNumber = Random.Range (0, 2); //gives in outputs 0 or 1
		return possibileValues[randomNumber];
	}

    private Vector3 getRandomVelocity() {
		return new Vector3(getRandomValueForComponent(), 0, getRandomValueForComponent());
    }

    private void createMiniAsteroid()
    {
		if (numDivision == maxDivision) //we can divide an asteroids a limited number of times
		{
			return;
		}

        GameObject newAsteroid = (GameObject)Instantiate(gameObject, transform.position, transform.rotation);

		//setting scale and velocity
        Rigidbody newAsteroidRb = newAsteroid.GetComponent<Rigidbody>();
        Vector3 newAsteroidsScale = newAsteroidRb.transform.localScale;
        newAsteroidRb.transform.localScale = newAsteroidsScale * 0.5f; //halve the size
        newAsteroidRb.velocity = getRandomVelocity();

		//changing numDivision property of created asteroid to limit generation of new ones
		AsteroidController newAsteroidControllerScript = (AsteroidController)newAsteroid.GetComponent(typeof(AsteroidController));
		newAsteroidControllerScript.setNumDivision(numDivision + 1);
    }

    //When the asteroid is destroyed, create "numberOfFragments" mini asteroids, each of them half the size of its parent
    private void createMiniAsteroids()
    {
        for (int i = 0; i < numberOfFragments; i++)
        {
            createMiniAsteroid();
        }
    }
}
