using UnityEngine;
using System.Collections;

public class DisableSlime : MonoBehaviour {

	public GameObject slimeFloor;

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Salts") 
		{
			Destroy(col.gameObject);
			Destroy (slimeFloor);
		}
	}
}
