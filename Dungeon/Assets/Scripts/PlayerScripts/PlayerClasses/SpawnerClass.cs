using UnityEngine;
using System.Collections;

public class SpawnerClass : MonoBehaviour
{
	public Transform[] spawnPoints = new Transform[0];

	public int numberOfEnemiesPerSpawn;
	public int timeBetweenSpawn;
	public GameObject enemyObject;
	private float _spawnTime;
	private bool _finishSpawn;
	private int _spawnNum;


	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (_spawnNum >= numberOfEnemiesPerSpawn) {
			_finishSpawn = true;
		}
		if (!_finishSpawn && _spawnTime < timeBetweenSpawn) {
			_spawnTime += Time.deltaTime;
		}
		if (!_finishSpawn && _spawnTime >= timeBetweenSpawn) {
			var _rand = Random.Range (0, spawnPoints.Length);
			Instantiate (enemyObject, spawnPoints [_rand].position, Quaternion.identity);
			_spawnNum += 1;
			_spawnTime = 0;
		}

	}


}
