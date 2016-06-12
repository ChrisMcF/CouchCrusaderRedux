using UnityEngine;
using System.Collections;

public class ItsRainingMan : MonoBehaviour {

    public int maxNumberOfCrystals;
    private int spawnedCrystalCount = 0;

	public GameObject rain;

	float timeLeft = 3.0f;
	public float spawnRadius;
	// Use this for initialization
	void OnDrawGizmos () 
	{Gizmos.color = Color.red;
		Gizmos.DrawWireCube (transform.position,  new Vector3(spawnRadius*2,1,spawnRadius*2));
	}
	
	// Update is called once per frame
	void Update () 
	{
		//spawnIndex = spawnPoint[0];
		timeLeft -= Time.deltaTime;
		if (timeLeft < 0 && spawnedCrystalCount < maxNumberOfCrystals) 
		{
            spawnedCrystalCount++;
          
			Instantiate(rain, new Vector3(transform.position.x+Random.Range(spawnRadius,-spawnRadius), transform.position.y, transform.position.z+Random.Range(spawnRadius,-spawnRadius)), Random.rotation);

		}
	}
}



