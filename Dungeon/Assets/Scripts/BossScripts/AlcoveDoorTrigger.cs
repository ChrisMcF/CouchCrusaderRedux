using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlcoveDoorTrigger : MonoBehaviour {
	public Door alcoveDoor;
	private int numberOfPlayers;
	private int targetObjectsInCollider =0;//Starts at 1 to include the bathsalts 
	private List<BasePlayerClass> playersInTrigger = new List<BasePlayerClass>(); 

	// Use this for initialization
	void Start ()
    {

        //players = GameObject.FindGameObjectsWithTag ("Player");
        //CheckNumberOfPlayers ();
    }
	
	void OnTriggerEnter(Collider col)
	{
		//Debug.Log (col.transform.gameObject.tag);
		if (col.gameObject.tag == "Player") 
		{
			playersInTrigger.Add(col.gameObject.GetComponent<BasePlayerClass>());
			targetObjectsInCollider ++;
		}
		if (col.gameObject.tag == "Salts") 
		{
			targetObjectsInCollider+= 1;
		}
		
	}

	
	void OnTriggerStay(Collider col)
	{
		if (col.transform.gameObject.tag == "Player") 
		{
			// Remove dead players from the list
			foreach (BasePlayerClass b in playersInTrigger) {
				if (b._isDead)
				{
					targetObjectsInCollider-=1;
					playersInTrigger.Remove(b);
				}
			}
			// Open the door if it is closed and a living player is inside, then open the door.
			if(alcoveDoor.doorState == Door.doorStates.closed && !col.transform.gameObject.GetComponent<BasePlayerClass>()._isDead)
			{
				alcoveDoor.StartCoroutine("OpenDoor");
			}
		}
	}

	
	void OnTriggerExit(Collider col)
	{
		playersInTrigger.Remove(col.gameObject.GetComponent<BasePlayerClass>());
		Debug.Log (targetObjectsInCollider);
		if (col.gameObject.tag == "Player") 
		{
			targetObjectsInCollider -= 1;
		}
		if (col.gameObject.tag == "Salts") 
		{
			targetObjectsInCollider -= 2;
		}
		//If the player and the salts are out of the room, close the door.
		if (targetObjectsInCollider == 0) 
		{
			alcoveDoor.StartCoroutine ("CloseDoor");
		}
	}
}
