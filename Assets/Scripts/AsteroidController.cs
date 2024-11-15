﻿using UnityEngine;
using System.Collections;

//every asteroid instance has this script attached
public class AsteroidController : MonoBehaviour
{
	public float tumble; //random initial rotation
	public float randomInitialVelocity; //random initial velocity

	public int numberOfFragments; //fragments generated after destruction
	public GameObject[] destroyableObjects; //list of object that can be destroyed by an asteroid
	public GameObject player;

	public int maxDivision; //how many times new asteroids will be generated starting from the first ancestor
	private int curDivision = 0; //how many times we have generated asteroids starting by the first ancestor

	public GameObject asteroidExplosion;
	public GameObject playerExplosion;

	public int explosionLifetime;
	public int playerScore;

//	public GameObject gamePrefab;
	private GameController gameController;

	void Start ()
	{
		GetComponent<Rigidbody> ().angularVelocity = Random.insideUnitSphere * tumble; //random 3d rotation

		Vector3 randomVelocity = Random.insideUnitSphere * randomInitialVelocity;
		randomVelocity.y = 0; //must be on the xz plane
		GetComponent<Rigidbody> ().velocity = randomVelocity;

		GameObject gamePrefab = GameObject.FindGameObjectWithTag("GameController"); //not maintainable :S
		gameController = (GameController)gamePrefab.GetComponent (typeof(GameController));
	}

	void OnTriggerEnter (Collider other)
	{
		foreach (GameObject destroyableObj in destroyableObjects) {
			if (other.gameObject.CompareTag (destroyableObj.tag)) {
				if (!other.gameObject.Equals (player)) {
					createMiniAsteroids ();
					destroyObjects (other, false);

					break;
				}
			} else if (other.gameObject.CompareTag (player.tag)) { //game over
				destroyObjects (other, true);
			}
		}
	}

	private void setNumDivision (int numDivision)
	{
		this.curDivision = numDivision;
	}

	private void destroyObjects (Collider other, bool isPlayer)
	{
		GameObject createdAsteroidExplosion = (GameObject)Instantiate(asteroidExplosion, transform.position, transform.rotation); //create an asteroidExplosion with position and rotation of asteroid

		Destroy (other.gameObject);
		Destroy (gameObject);

		Destroy (createdAsteroidExplosion, explosionLifetime); //after explosionLifetime seconds, this gameobject will be destroyed

		updatePlayerScore ();

		if (isPlayer) {
			GameObject createdPlayerExplosion = (GameObject)Instantiate (playerExplosion, other.transform.position, other.transform.rotation); //create a playerExplosion with position and rotation of player
			Destroy (createdPlayerExplosion, explosionLifetime);

			gameController.gameOver ();
		}
	}

	private void updatePlayerScore() 
	{
		gameController.updatePlayersScore (playerScore);
	}

	private int getRandomValueForComponent ()
	{
		int[] possibileValues = new int[2] { -1, 1 };

		int randomNumber = Random.Range (0, 2); //gives in outputs 0 or 1
		return possibileValues [randomNumber];
	}

	private Vector3 getRandomVelocity ()
	{
		return new Vector3 (getRandomValueForComponent (), 0, getRandomValueForComponent ());
	}

	private void createMiniAsteroid ()
	{
		if (curDivision == maxDivision) { //we can divide an asteroids a limited number of times
			return;
		}
		
		GameObject newAsteroid = (GameObject)Instantiate (gameObject, transform.position, transform.rotation);

		//setting scale and velocity
		Rigidbody newAsteroidRb = newAsteroid.GetComponent<Rigidbody> ();
		Vector3 newAsteroidsScale = newAsteroidRb.transform.localScale;
		newAsteroidRb.transform.localScale = newAsteroidsScale * 0.5f; //halve the size
		newAsteroidRb.velocity = getRandomVelocity ();

		//changing numDivision property of created asteroid to limit generation of new ones
		AsteroidController newAsteroidControllerScript = (AsteroidController)newAsteroid.GetComponent (typeof(AsteroidController));
		newAsteroidControllerScript.setNumDivision (curDivision + 1);
	}

	//When the asteroid is destroyed, create "numberOfFragments" mini asteroids, each of them half the size of its parent
	private void createMiniAsteroids ()
	{
		for (int i = 0; i < numberOfFragments; i++) {
			createMiniAsteroid ();
		}
	}
}
