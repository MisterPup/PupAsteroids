using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	public GameObject asteroidPrefab;
	public GameObject player; //temporary
	private string asteroidGameObjectName;

	private GameObject world;
	private float worldXSize;
	private float worldZSize;

	private int waveNumber = 0;
	private bool test = true;

	void Start ()
	{
		Debug.Log ("Starting");
		asteroidGameObjectName = asteroidPrefab.name;

		world = GameObject.Find ("World"); //not very maintainable!
		BoundaryController boundaryController = (BoundaryController)world.GetComponent (typeof(BoundaryController));
		worldXSize = boundaryController.WorldXSize;
		worldZSize = boundaryController.WorldZSize;
	}

	void Update ()
	{
		Debug.Log ("Asteroid found: " + GameObject.Find (asteroidGameObjectName));
		if (GameObject.Find (asteroidGameObjectName) == null && test == true) { //All asteroids destroyed!
			waveNumber++;
			Debug.Log ("Starting wave: " + waveNumber);
			startWave ();
			test = false;
		}
	}

	private void startWave ()
	{
		for (int i = 0; i < waveNumber; i++) {
			GameObject newAsteroid = (GameObject)Instantiate (gameObject, new Vector3 (7, player.transform.position.y, 7), player.transform.rotation); //temporary
		}
	}
}
