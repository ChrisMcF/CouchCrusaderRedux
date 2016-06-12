using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour
{
	public bool destroy;
	public bool hasDeathEffect;
	public GameObject deathEffect;

	public float maxHealth;
	[HideInInspector]
	public float
		currentHealth;

	HealthBarBehaviour _healthBarBehaviour;

	// Use this for initialization
	void Start ()
	{
		currentHealth = maxHealth;
		_healthBarBehaviour = transform.FindChild ("HealthBar").GetComponent<HealthBarBehaviour> ();
		_healthBarBehaviour.InitializeMaxAndCurrentHealth (maxHealth);
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
	
		if (currentHealth <= 0.0f) {

			if (hasDeathEffect) {

				Instantiate (deathEffect, transform.position, deathEffect.transform.rotation);
			}
			if (destroy)
				Destroy (gameObject);
		}
	}

//	void AdjustCurHealth(float value){
//
//		currentHealth += value;
//		if(_healthBarBehaviour != null)
//		_healthBarBehaviour.SetCurrentHealth(currentHealth);
//	}
}
