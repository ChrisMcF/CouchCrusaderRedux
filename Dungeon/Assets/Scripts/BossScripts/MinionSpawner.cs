using UnityEngine;
using System.Collections;

public class MinionSpawner : MonoBehaviour {


	public Transform[] spawnPoints;
	public float spawnTimer;
	public GameObject[] minions;
	public float waitUntilStart = 4f; 

	// Use this for initialization
	void Start ()
	{
		InvokeRepeating ("SpawnMinion", waitUntilStart, spawnTimer);
	}

	void SpawnMinion()
	{
		// to help spawn random minions that have been selected in the inspected at the different spawnpoints that have been selected in the inspector as well.
		int randomSpawn = Random.Range (0, spawnPoints.Length);
		int randomMinion = Random.Range (0, minions.Length);
		Instantiate (minions [randomMinion], spawnPoints [randomSpawn].position, Quaternion.identity);
	}
}
