using UnityEngine;
using System.Collections;

public class AbraMagicOrb : MonoBehaviour
{

	public GameObject orbPrefab;
	public GameObject orbDeathEffectPrefab;
	public float targetRadius;
	public float orbSpeed;
	public float orbLifetime;
	public float orbDamage;
	public Vector3 offset = new Vector3 (0, 4, 0);
	Vector3 _targetPosition;

	Transform[] _rangedPlayers;

	bool _isAlive;
	bool _hitPlayer;

	//TEST VARIABLES
	public Transform TEST_TARGET;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		///*
		/*
		if (Input.GetKeyDown ("2")) {
			foreach (GameObject _player in GameObject.FindGameObjectsWithTag("Player")) {
				if (!_player.GetComponent<BasePlayerClass> ()._isDead) {
					if (_player.name == "Archer" || _player.name == "Mage") {
						Debug.Log (_player.name);
						//StartCoroutine (ShootOrb (orbPrefab, orbDeathEffectPrefab, transform.position + Vector3.up * 4, _player.transform, targetRadius, orbSpeed, orbLifetime, orbDamage));
						StartCoroutine (ShootOrb (_player.transform));
					}
				}
			}
		}
		//*/
	}

	//public IEnumerator ShootOrb (GameObject _orbPrefab, GameObject _orbDeathEffectPrefab, Vector3 _orbSpawnPos, Transform _target, float _targetRadius, float _orbSpeed, float _orbLifetime, float _orbDamage) {
	public IEnumerator ShootOrb (Transform _target)
	{
		Vector3 _orbSpawnPos = transform.position + offset;
		GameObject _orb = (GameObject)Instantiate (orbPrefab, _orbSpawnPos, transform.rotation);
		AbraOrbProjectile _orbScript = _orb.GetComponent<AbraOrbProjectile> ();
		object[] parms = new object[7] {
			targetRadius,
			orbSpeed,
			orbLifetime,
			orbDamage,
			_targetPosition,
			_target,
			orbDeathEffectPrefab
		};

		_targetPosition = _target.position + Vector3.up;// Set here for first iteration

		_orbScript.StartCoroutine ("MoveOrb", parms);

		return null;

//		if (Vector3.Distance (_orb.transform.position, _targetPosition) <= _targetRadius) {
//
//			_target.SendMessage("AdjustCurHealth", -_orbDamage);
//		}
//
//		GameObject _orbDeathEffect = (GameObject)Instantiate (_orbDeathEffectPrefab, _orb.transform.position, _orbDeathEffectPrefab.transform.rotation);
//		//Destroy the orb death effect after its lifetime has ended
//		Destroy (_orbDeathEffect, _orbDeathEffect.GetComponent<ParticleSystem> ().startLifetime);
//
//		Destroy (_orb);
	}
}
