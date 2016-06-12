using UnityEngine;
using System.Collections;

public class cloudScript : MonoBehaviour {



	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Translate (transform.forward * 10 * Time.deltaTime);

		if (gameObject.transform.position.z > 170) {
			gameObject.transform.Translate (transform.forward * -200);
		}

		if (gameObject.transform.position.z < 0) {
			gameObject.transform.Translate (transform.forward * 10 * Time.deltaTime);
		}
	}
}
