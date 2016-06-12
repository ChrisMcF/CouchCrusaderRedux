using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
This script allows you to have multiple pools of items that are preloaded at startup and then reused to avoid issues with garbage collection
Multiple pool components may be assigned to one or more game objects, however only one "Pool Manager" object is allowed. 
All pools must have a unique pool name.
The initalisationMethodName variable can be used to trigger a method on a script on the gameobject every time it is drawn from the pool.

The public methods in this script may be referenced through the pool manager from any script as follows:

		//Request a game object
		GameObject myGameObject = PoolManager.Instance.FindPoolByName ("WhateverYourPoolNameIs").RequestItemFromPoolAt (Vector3.zero, Quaternion.identity);
		
		//Put it back in the pool
		PoolManager.Instance.FindPoolByName ("WhateverYourPoolNameIs").PutItemBackInPool (myGameObject);
		
*/


public class Pool : MonoBehaviour 
{
	public string poolName; //A name for the pool to use in the Pool Manager FindPoolByName method.
	public GameObject pooledObject; // the object to pool
	public int preLoadedCopies; //The number of starting copies
	[HideInInspector]
	public List<GameObject> inPool = new List<GameObject>(); //Items waiting to be used
	[HideInInspector]
	public List<GameObject> inUse = new List<GameObject>(); //Items in use
	public Vector3 poolPosition = new Vector3 (-1000, -1000, -1000); // the position where the pooled items will wait to be used
	public string initializationMethodName;// if supplied thes method will be called every time an item is reused
	
	void Start()
	{
		if (string.IsNullOrEmpty (poolName)) 
			Debug.LogError ("Error: Pool name variable is empty");
		if(pooledObject == null)
			Debug.LogError ("Error: Pooled object variable is unassigned");
		PreLoadPool ();

	}

	
	private void PreLoadPool()
	{
		for(int i = 0; i < preLoadedCopies; i++)
		{
			InstantiateNewItemAndAddItToThePool();
		}
	}

	//Use this instead of instantiate
	public GameObject RequestItemFromPoolAt(Vector3 position, Quaternion rotation)
	{
		if (inPool.Count < 1) 
		{
			InstantiateNewItemAndAddItToThePool();
		}
		GameObject chosenObject = inPool [0];
		inPool.Remove (chosenObject);
		inUse.Add (chosenObject);
		chosenObject.SetActive (true);
		chosenObject.transform.position = position;
		chosenObject.transform.rotation = rotation;

		// If a value has been supplied for the initialization method, 
		// It will be called on every script on the chosen object which inherits from monobehavior.
		// For more details see the GameObject.SendMessage Documentation
		if (!string.IsNullOrEmpty (initializationMethodName)) 
		{
			chosenObject.SendMessage (initializationMethodName);
		}
		return chosenObject;
	}

	//Use this instead of destroy
	public void PutItemBackInPool(GameObject myObject)
	{
		inUse.Remove (myObject);
		inPool.Add (myObject);
		myObject.transform.position = poolPosition;
		myObject.transform.rotation = Quaternion.identity;
		myObject.SetActive (false);
	}

	private void InstantiateNewItemAndAddItToThePool()
	{
		GameObject myInstance = Instantiate (pooledObject, poolPosition, Quaternion.identity) as GameObject;
		inPool.Add(myInstance);
		myInstance.SetActive (false);
	}
}
