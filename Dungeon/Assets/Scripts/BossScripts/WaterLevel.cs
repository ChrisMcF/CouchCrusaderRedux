using UnityEngine;
using System.Collections;

public class WaterLevel : MonoBehaviour {

	public GameObject water;
	private Vector3 waterlevel;

	void Start (){
        waterlevel = new Vector3 (490, 5, -80);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey ("a")) {
			iTween.MoveTo (water, waterlevel, 10);

		}
		if (Input.GetKey ("d")) {
			water.transform.position = new Vector3 (490,10, -80);
		}
	}
}
