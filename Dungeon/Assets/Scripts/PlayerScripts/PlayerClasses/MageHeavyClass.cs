﻿using UnityEngine;
using System.Collections;

public class MageHeavyClass : MonoBehaviour 
{

	private float timer;
	private GameObject mage;

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Boss") 
		{
			col.SendMessage("AdjustCurHealth", -mage.GetComponent<BasePlayerClass>().actions.heavy.damage);
			mage.GetComponent<BasePlayerClass>().stats.specialAmount += mage.GetComponent<BasePlayerClass>().actions.heavy.damage * mage.GetComponent<BasePlayerClass>().stats.specialIncrement;
			//mage.GetComponent<BasePlayerClass>().CallAttack(2);
			Debug.Log ("hit!");
			Destroy (gameObject);
		}
	}

	void Start ()
	{
		mage = GameObject.Find ("Mage");
	}
	
	void Update ()
	{
		transform.Translate (Vector3.forward * 100 * Time.deltaTime);
		
		timer += Time.deltaTime;
		
		if (timer >= 5f)
			Destroy (gameObject);
	}
}
