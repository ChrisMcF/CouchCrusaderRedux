using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {

    public GameObject text;
    public int speed = 1;

	
	// Update is called once per frame
	void Update ()
    {
        text.transform.Translate(Vector3.up * Time.deltaTime * speed);
	}

}
