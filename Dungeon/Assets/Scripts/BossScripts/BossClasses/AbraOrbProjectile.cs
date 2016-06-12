using UnityEngine;
using System.Collections;

public class AbraOrbProjectile : MonoBehaviour
{
	
	Transform _playerTarget;

	float targetRadius;
	float orbSpeed;
	float orbLifetime;
	float orbDamage;
	Vector3 _targetPosition;
	bool _hitPlayer;
	GameObject orbDeathEffectPrefab;

	void OnCollisionEnter(Collision coll)
	{
		if (coll.gameObject.tag != "Player")
			Destroy (gameObject);
	}

	public IEnumerator MoveOrb (object[] _parms)
	{

		targetRadius = (float)_parms [0];
		orbSpeed = (float)_parms [1];
		orbLifetime = (float)_parms [2];
		orbDamage = (float)_parms [3];
		_targetPosition = (Vector3)_parms [4];
		_playerTarget = (Transform)_parms [5];
		orbDeathEffectPrefab = (GameObject)_parms [6];

		float _orbLifetime_timeLeft = orbLifetime;

		while (Vector3.Distance (transform.position, _targetPosition) > targetRadius && _orbLifetime_timeLeft > 0) {

			//The position of the middle of the player (since the player's transform is at the bottom of their feet)
			_targetPosition = _playerTarget.position + Vector3.up;
			
			//Look at the target
			transform.LookAt (_targetPosition);
			//Move towards the target
			transform.Translate (Vector3.forward * orbSpeed * Time.deltaTime);
			
			//Update the time left until the orb is automatically destroyed
			_orbLifetime_timeLeft -= Time.deltaTime;
			
			if (Vector3.Distance (transform.position, _targetPosition) <= targetRadius) {
				
				_hitPlayer = true;
			}
			
			yield return null;
		}
		
		if (_hitPlayer) {
			
			_playerTarget.SendMessage ("AdjustCurHealth", -orbDamage);
		}

		GameObject _orbDeathEffect = (GameObject)Instantiate (orbDeathEffectPrefab, transform.position, orbDeathEffectPrefab.transform.rotation);
		//Destroy the orb death effect after its lifetime has ended
		Destroy (_orbDeathEffect, _orbDeathEffect.GetComponent<ParticleSystem> ().startLifetime);
		Destroy (gameObject);
	}
}
