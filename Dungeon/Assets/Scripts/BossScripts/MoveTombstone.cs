using UnityEngine;
using System.Collections;

public class MoveTombstone : MonoBehaviour {

	public GameObject Tombstone;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter()
	{
		Tombstone.transform.localPosition=Vector3.MoveTowards (Tombstone.transform.localPosition, new Vector3 (0f,10f,0f), 10f*Time.deltaTime);
	}
}
