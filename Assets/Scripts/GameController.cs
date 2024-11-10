using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public GameObject asteroid;
	public GameObject world;
	public GameObject player;

	public float minPlayerNewAsteroidDistance;

	public int waitTimeBeforeNewWave;

	private string asteroidTag; //search asteroids in the world
	private float worldXSize;
	private float worldZSize;
	private float minSize; //min between x and z size

	int waveNumber = 0;
	int numAsteroids = 0;

	public Text playerScoreTextPrefab;
	public Text gameOverTextPrefab;
	public Text restartTextPrefab;

	private Text playerScoreText;
	private Text gameOverText;
	private Text restartText;

	private int playerScore;
	private bool isGameOver;

	void Start ()
	{
		asteroidTag = asteroid.tag;

		BoundaryController boundaryController = (BoundaryController)world.GetComponent (typeof(BoundaryController));
		worldXSize = boundaryController.WorldXSize;
		worldZSize = boundaryController.WorldZSize;

		minSize = worldXSize < worldZSize ? worldXSize : worldZSize;

		StartCoroutine (waveManager ());

		playerScoreText = (Text)Instantiate (playerScoreTextPrefab, playerScoreTextPrefab.transform.position, playerScoreTextPrefab.transform.rotation);
		gameOverText = (Text)Instantiate (gameOverTextPrefab, gameOverTextPrefab.transform.position, gameOverTextPrefab.transform.rotation);
		restartText = (Text)Instantiate (restartTextPrefab, restartTextPrefab.transform.position, restartTextPrefab.transform.rotation);

		playerScoreText.text = "Player score: 0";
		gameOverText.text = "";
		restartText.text = "";

		playerScore = 0;
		isGameOver = false;
	}

	public void updatePlayersScore(int playerScore)
	{
		this.playerScore += playerScore;
		playerScoreText.text = "Player score: " + this.playerScore;
	}

	public void gameOver() 
	{
		isGameOver = true;
		gameOverText.text = "Game Over";
	}

	void Update ()
	{
		if (isGameOver) {
			if (Input.GetKey (KeyCode.R)) {
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//				Application.LoadLevel (Application.loadedLevel);
			}
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

	IEnumerator waveManager ()
	{				
		while (true) {
			if (isWaveDefeated()) {
				startNewWave ();
			}
			yield return new WaitForSeconds (waitTimeBeforeNewWave); //wait "waitTimeBeforeNewWave" before checking if new wave is needed

			if (isGameOver) {
				restartText.text = "Press 'R' to restart";
				break;
			}
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

		for (int i = 0; i < asteroids.Length; i++) {
//			Debug.Log (asteroids[i].transform.position);
			Debug.Log ("Name: " + asteroids[i].name);
		}

		numAsteroids = asteroids.Length;
		Debug.Log ("numAsteroids: " + numAsteroids);
		return numAsteroids == 0;
	}
}
