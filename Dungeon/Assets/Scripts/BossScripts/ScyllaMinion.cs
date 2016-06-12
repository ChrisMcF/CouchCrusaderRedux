using UnityEngine;
using System.Collections;

public class ScyllaMinion : EnemyClass
{

	//public Transform[] _playerLoc;
	//public GameObject[] _playerObj;
	public float moveSpeed = 400f;
	public float rotSpeed = 50f;
	public float distanceThreshold;
	private float _distance;
	private float _damageTime;
	public float attackRate;
	public float damage;
	public float attackRadius;

	private Vector3 _temp;

	private GameObject playerTarget;

	// Use this for initialization
	void Start ()
	{	
	}
	
	// Update is called once per frame
	void Update ()
	{	
		playerTarget = FindClosestPlayer ();

		var rotationAngle = Quaternion.LookRotation (playerTarget.transform.position - transform.position); // we get the angle has to be rotated
		rotationAngle.x = 0;
		rotationAngle.z = 0;
		transform.rotation = Quaternion.Slerp (transform.rotation, rotationAngle, Time.deltaTime * rotSpeed); // we rotate the rotationAngle 

		_distance = Vector3.Distance (transform.position, playerTarget.transform.position);
		if (_distance >= distanceThreshold) {
			_temp = Vector3.MoveTowards (transform.position, playerTarget.transform.position, Time.deltaTime * moveSpeed);
			transform.position = new Vector3 (_temp.x, transform.position.y, _temp.z);
		}
	}
	

	GameObject FindClosestPlayer ()
	{
		GameObject[] _players;
		_players = GameObject.FindGameObjectsWithTag ("Player");
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject go in _players) {
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closest = go;
				distance = curDistance;
			}
		}
		return closest;
	}

	void OnTriggerStay (Collider col)
	{
		if (_damageTime < attackRate) {
			_damageTime += Time.deltaTime;
		} else {
			_damageTime = 0;
			Collider[] colliders = Physics.OverlapSphere (transform.position, attackRadius);
			foreach (Collider item in colliders) {
				if (item.gameObject.CompareTag ("Player")) {
					//TODO: MAKE PLAYERS TAKE DAMAGE
				}
			}
		}
	}
}
