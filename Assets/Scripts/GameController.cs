using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
	public GameObject asteroid;
	public GameObject world;
	public GameObject player;
	public GameObject playerScore;
	public float minPlayerNewAsteroidDistance;

	public int waitTimeBeforeNewWave;

	private string asteroidTag; //search asteroids in the world
	private float worldXSize;
	private float worldZSize;
	private float minSize; //min between x and z size

	int waveNumber = 0;
	int numAsteroids = 0;

	void Start ()
	{
		asteroidTag = asteroid.tag;

		BoundaryController boundaryController = (BoundaryController)world.GetComponent (typeof(BoundaryController));
		worldXSize = boundaryController.WorldXSize;
		worldZSize = boundaryController.WorldZSize;

		minSize = worldXSize < worldZSize ? worldXSize : worldZSize;

		StartCoroutine (waveManager ());

		Instantiate (playerScore, playerScore.transform.position, playerScore.transform.rotation);
//		Instantiate (playerScore, new Vector3(0.7f, 0.7f, 0.0f), new Quaternion());
	}

	void Update ()
	{
		
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

	IEnumerator waveManager ()
	{				
		while (true) {
			if (isWaveDefeated()) {
				startNewWave ();
			}
			yield return new WaitForSeconds (waitTimeBeforeNewWave); //wait "waitTimeBeforeNewWave" before checking if new wave is needed
		}
	}

	void startNewWave() {
		waveNumber++;
		Debug.Log ("Starting wave: " + waveNumber);
		for (int i = 0; i < waveNumber; i++) {
			numAsteroids++;
			Vector3 newAsteroidPosition = getPositionForNewAsteroid ();
			newAsteroidPosition.y = player.transform.position.y; //must be on the same xz plane of the player
			Quaternion newAsteroidRotation = getRotationForNewAsteroid ();
			Instantiate (asteroid, newAsteroidPosition, newAsteroidRotation);
		}
	}

	bool isWaveDefeated() {
		GameObject[] asteroids = GameObject.FindGameObjectsWithTag (asteroidTag); //heavy on performance? We need a better way to manage asteroids!
		numAsteroids = asteroids.Length;
		Debug.Log ("numAsteroids: " + numAsteroids);
		return numAsteroids == 0;
	}
}
