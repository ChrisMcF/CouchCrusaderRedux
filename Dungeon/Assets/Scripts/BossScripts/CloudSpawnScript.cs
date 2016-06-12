using UnityEngine;
using System.Collections;

public class CloudSpawnScript : MonoBehaviour {

	public GameObject movingCloud;

	// Use this for initialization
	void Start () {
		Instantiate (movingCloud, transform.position, transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
