using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	public GameObject asteroidPrefab;
	private string asteroidTag; //search asteroids in the world

	public GameObject player;

	private GameObject world;
	private float worldXSize;
	private float worldZSize;
	private float minSize; //min between x and z size

	private int waveNumber = 0;

	void Start ()
	{
		asteroidTag = asteroidPrefab.tag;

		world = GameObject.Find ("World"); //not very maintainable!
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
			startWave ();
		}
	}

	private void startWave ()
	{
		Debug.Log ("Starting wave: " + waveNumber);
		for (int i = 0; i < waveNumber; i++) {
			Vector3 randomPosition = Random.insideUnitSphere * 5; //temporary, because it must be far from the player!
			randomPosition.y = player.transform.position.y; //must be on the same xz plane of the player

			Quaternion randomRotation = Random.rotation;

			GameObject newAsteroid = (GameObject)Instantiate (asteroidPrefab, randomPosition, randomRotation);
		}
	}
}
