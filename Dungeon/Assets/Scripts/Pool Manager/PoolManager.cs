using UnityEngine;
using System.Collections;

//This script finds all of the pool objects and provides a convinent method to reference a pool by it's poolName. 
//This script uses a singleton approach to ensure that there is only one static instance that it is easy to referance.

[DisallowMultipleComponent]
public class PoolManager : MonoBehaviour 
{
	public static PoolManager Instance;
	[HideInInspector]
	public Pool[] pools;

	void Awake()
	{
		//ensure there is only one instance of PoolManager
		if(Instance != null && Instance != this) 
			Destroy(gameObject);

		Instance = this;
		//Cache references to all pools
		pools = FindObjectsOfType(typeof(Pool)) as Pool[];
	}

	void Start () 
	{
		Debug.Log (pools.Length);
		//Check to ensure that pool names are unique
		if (pools.Length > 1) 
		{
			for (int i = 0; i < pools.Length; i++) 
			{
				for (int j= i+1; j < pools.Length; j++) 
				{
					if (pools [i].poolName == pools [j].poolName)
						Debug.LogError ("Error: Pool names are not unique");
				}
			}
		}
	}

	//This method can be used to find a specific pool using its pool name
	public Pool FindPoolByName(string poolName)
	{
		if (pools.Length == 0) 
		{
			Debug.LogError ("Error: No Pools Found");
		}
		else 
		{
			for (int i = 0; i < pools.Length; i++) 
			{
				if (pools [i].poolName == poolName) 
					return pools [i];
			}
		}
		// if nothing was returned then error
		Debug.LogError ("Requested Pool: " + poolName + " was not found!");
		return null;
	}
}
