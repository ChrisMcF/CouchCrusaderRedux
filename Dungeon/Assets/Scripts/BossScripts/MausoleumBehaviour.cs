using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]

public class MausoleumBehaviour : MonoBehaviour {
	
	public float endHeight = 10f;
	
	//public GameObject healthBar;
	public float maxHealth;
	private float _currentHealth;
	public float raiseTime = 3f;
	public GameObject deathEffect;
	private AbraCadaverBossClass abra;
	HealthBarBehaviour _healthBarBehaviour;

	void Start()
	{
		//hb = GetComponent<HealthBar> ();
		abra = GameObject.Find ("AbraCadaver").GetComponent<AbraCadaverBossClass> ();
		StartCoroutine("MoveMausoleum", raiseTime);
		_healthBarBehaviour = transform.FindChild("HealthBar").GetComponent<HealthBarBehaviour>();
		_currentHealth = maxHealth;
		_healthBarBehaviour.InitializeMaxAndCurrentHealth (maxHealth);
		//abra.mausoleumList.Add (this);
	}
	
	void Update()
	{
		//_currentHealth = hb.currentHealth;
		if (_currentHealth <= 0.0f)
			OnDestroyMausoleum();
	}
	
	public IEnumerator MoveMausoleum(float travelTime)
	{	
		//Calculate the speed needed to reach the destination over the specified time
		//float i = 0.0f;
		//float _speed = 1.0f/travelTime;

		Vector3 _startPos = transform.position;
		Vector3 _endPos = new Vector3(transform.position.x, transform.position.y+endHeight, transform.position.z);
		float counter = 0;
		float lerpAmmount;
		//Update position every frame
		while (counter < travelTime)
		{
			lerpAmmount = counter/travelTime;
			transform.position = Vector3.Lerp (_startPos, _endPos, lerpAmmount);
			counter += Time.deltaTime;
			yield return null;
		}
	}

	public void AdjustCurHealth(DamageInfo damageInfo)
	{	
		float value = damageInfo.damage;
		_currentHealth += value;
		if(_healthBarBehaviour != null)
			_healthBarBehaviour.SetCurrentHealth(_currentHealth);
	}

	public void OnDestroyMausoleum()
	{
		abra.mausoleumList.Remove (this);
		if(deathEffect != null)
			Instantiate(deathEffect, transform.position, deathEffect.transform.rotation);
		Destroy(gameObject);
	}
}
