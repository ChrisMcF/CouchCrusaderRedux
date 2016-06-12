using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AudioSource))]
public class AbraFirestorm : MonoBehaviour
{

	public GameObject fireballPrefab;
	public GameObject explosionEffect;
	public float fireballStartHeight;
	public float fireballEndHeight;
	public float lifetime;

	DamageInfo _damageInfo;
	public float damage;
	public float damageRadius;

	public Transform bounds_bottomLeft;
	public Transform bounds_topRight;
	public float castTime;
	public float fireballSpawnRate;
	public bool randomise = false;
	public Vector2 randomMinMax = new Vector2 (0.1f, 0.3f);

	//public float raiseStageTime;
	//public GameObject stage;

	//TEST VARS
	public GameObject[] _players;
	private GameObject cameraObject;
	private CameraShake cameraShake;


	// Use this for initialization
	void Start ()
	{
		_damageInfo = new DamageInfo (new Player (), -damage, 0f, false);

		cameraObject = GameObject.FindGameObjectWithTag ("MainCamera");
		cameraShake = cameraObject.GetComponent<CameraShake> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (randomise)
			fireballSpawnRate = Random.Range (randomMinMax.x, randomMinMax.y);
		/*
		if (Input.GetKeyDown("1") && !isCasting){
			//StartCoroutine(FireballStorm (bounds_bottomLeft, bounds_topRight, castTime, fireballSpawnRate));
			StartCoroutine(FireballStorm (castTime));

		}
		//*/
	}



	IEnumerator SpawnFireball (GameObject _fireballPrefab, GameObject _explosionEffect, Vector3 _startPos, Vector3 _endPos, float _lifetime, float _damage, float _damageRadius)
	{

		GameObject _fireball = (GameObject)Instantiate (_fireballPrefab, _startPos, _fireballPrefab.transform.rotation);

		//Create targeting circle
		TargetingClass _targetingClass = GameObject.Find ("Targeting System").GetComponent<TargetingClass> ();
		_targetingClass.CreateTargetCircle (_endPos + new Vector3 (0, 0.1f, 0), _damageRadius, Color.red, _lifetime);

		//Calculate the speed needed for the fireball to travel over its lifetime
		float i = 0.0f;
		float _speed = 1.0f / _lifetime;
		//Update the position of the fireball every frame
		while (i < 1.0f) {
			i += Time.deltaTime * _speed;
			_fireball.transform.position = Vector3.Lerp (_startPos, _endPos, i);

			yield return null;
		}

		//Create the explosion effect
		Instantiate (_explosionEffect, _endPos, _explosionEffect.transform.rotation);
		cameraShake.StartShake (0.1f);
		//Damage all players within the damage radius

		Collider[] _cols = Physics.OverlapSphere (_endPos, _damageRadius / 2);
		foreach (Collider _col in _cols) {
			if (_col.transform.tag == "Enemy") {
				_col.transform.SendMessage ("AdjustCurHealth", _damageInfo);
				//	Debug.Log (_hit.distance);
			}
			if (_col.transform.tag == "Player") {
				_col.transform.SendMessage ("AdjustCurHealth", -damage);
			}
		}

		//Fire trail of the fireball prefab
		Transform _fireEffect = _fireball.transform.GetChild (0);
		//Detach the fire trail's particle system from its parent and stop emmission
		//This lets the fire effect particles stay alive until they have animated for their full lifetime, rather than destroying them prematurely
		ParticleSystem _fireParticleSystem = _fireEffect.GetComponent<ParticleSystem> ();
		_fireParticleSystem.transform.parent = null;
		_fireParticleSystem.enableEmission = false;

		//Destroy the fireball and the detached fire effect particle system after its lifetime has ended
		Destroy (_fireParticleSystem.gameObject, _fireParticleSystem.startLifetime);
		Destroy (_fireball);
	}



//	public IEnumerator FireballStorm(Transform _bounds_bottomLeft, Transform _bounds_topRight, float _castTime, float _fireballSpawnRate){
	public IEnumerator FireballStorm (float _castTime)
	{
        

		//Spawn a fireball every '_fireballSpawnRate' seconds at a random position within the bounds
		for (int i = 0; i < _castTime/fireballSpawnRate; i++) {
			//Random start position of the fireball
			Vector3 _startPos = new Vector3 (Random.Range (bounds_bottomLeft.position.x, bounds_topRight.position.x), fireballStartHeight, Random.Range (bounds_bottomLeft.position.z, bounds_topRight.position.z));
			//End position of the fireball
			Vector3 _endPos = new Vector3 (_startPos.x, fireballEndHeight, _startPos.z);

			//Spawn a fireball
			StartCoroutine (SpawnFireball (fireballPrefab, explosionEffect, _startPos, _endPos, lifetime, damage, damageRadius));
			yield return new WaitForSeconds (fireballSpawnRate);
		}
	}
}
