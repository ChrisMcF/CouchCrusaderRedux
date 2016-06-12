using UnityEngine;
using System.Collections;

public class CamShakeTest : MonoBehaviour {


    public float timer = 5f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = 5f;
            Camera.main.GetComponent<CameraShake>().StartShake(2f);
        }
	}
}
