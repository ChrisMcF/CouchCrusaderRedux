using UnityEngine;
using System.Collections;

public class TestTimer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator Cooldown (float time)
	{
		float timer = time;
		while (timer > 0)
		{
			timer -= Time.deltaTime;
			yield return null;
		}
	}
}
