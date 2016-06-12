using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlimeZone : MonoBehaviour {
	public float range, damageScale =20f; 
	private float slimeZoneDamage;
	//public List <GameObject> playersDetected = new List<GameObject> ();


	// Use this for initialization
	void Start () 
	{
		
	}

	void OnEnabled()
	{
		slimeZoneDamage = 0f;
	}
	// Update is called once per frame
	void Update () 
	{






		//if(timer <= 0)
		//{
			slimeZoneDamage +=Time.deltaTime;
			//foreach(GameObject player in playersDetected.ToArray())
			foreach(Player player in GameController.gameController.players)
			{
				BasePlayerClass pc = player.playerClass;//player.GetComponent<BasePlayerClass>();
				if(pc._isDead )
				{
					continue;
				}

				if(Vector3.Distance(transform.position, player.playerObject.transform.position) <= range)
				{
					pc.AdjustCurHealth(-slimeZoneDamage * damageScale * Time.deltaTime);
				}
			}
			//timer = interval;
		//}
		//else
		//{
		//	timer-=Time.deltaTime;
		//}
	}



}
