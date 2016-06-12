using UnityEngine;
using System.Collections;

public class Interact : MonoBehaviour {

	public bool singleUse, used, item;
	public int speakToIndex	= -1;
	

	void Start()
	{
		if(item)
		name = gameObject.GetInstanceID().ToString();
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{

			if(!singleUse || (singleUse && !used))
			{	
				other.BroadcastMessage("StartDialogue", speakToIndex);
				if(singleUse)
					used = true;
			}
		}
	}

	void Delete()
	{
		Destroy(gameObject);
	}
}
