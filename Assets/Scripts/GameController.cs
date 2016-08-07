using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	public GameObject asteroidPrefab;
	public GameObject world;
	public GameObject player;
	public float minPlayerNewAsteroidDistance;

	private string asteroidTag; //search asteroids in the world
	private float worldXSize;
	private float worldZSize;
	private float minSize; //min between x and z size

	private int waveNumber = 0;

	void Start ()
	{
		asteroidTag = asteroidPrefab.tag;

		BoundaryController boundaryController = (BoundaryController)world.GetComponent (typeof(BoundaryController));
		worldXSize = boundaryController.WorldXSize;
		worldZSize = boundaryController.WorldZSize;

		minSize = worldXSize < worldZSize ? worldXSize : worldZSize;
	}

	void Update ()
	{
		GameObject[] asteroids = GameObject.FindGameObjectsWithTag (asteroidTag); //heavy on performance? We need a better way to manage asteroids!
		int numAsteroids = asteroids.Length;
		Debug.Log ("Number of asteroids: " + numAsteroids);
				
		if (numAsteroids == 0) { //All asteroids destroyed!
			waveNumber++;
			startNewWave ();
		}
	}

	private Vector3 getPositionForNewAsteroid() {
		while(true) { //mmm pitfall?
			Vector3 newAsteroidPosition = Random.insideUnitSphere * minSize / 2; //circle inscribed into world
			newAsteroidPosition.y = player.transform.position.y; //must be on the same xz plane of the player

			float distancePlayerNewAsteroid = Vector3.Distance(player.transform.position, newAsteroidPosition);

			if (distancePlayerNewAsteroid > minPlayerNewAsteroidDistance) {
				return newAsteroidPosition;
			}
		}
	}

	private Quaternion getRotationForNewAsteroid() {
		return Random.rotation;
	}

	private void startNewWave ()
	{
		Debug.Log ("Starting wave: " + waveNumber);
		for (int i = 0; i < waveNumber; i++) {
			Vector3 newAsteroidPosition = getPositionForNewAsteroid ();
			newAsteroidPosition.y = player.transform.position.y; //must be on the same xz plane of the player

			Quaternion newAsteroidRotation = getRotationForNewAsteroid();

			GameObject newAsteroid = (GameObject)Instantiate (asteroidPrefab, newAsteroidPosition, newAsteroidRotation);
		}
	}
}
