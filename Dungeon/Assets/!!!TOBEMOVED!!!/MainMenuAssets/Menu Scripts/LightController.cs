using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour 
{
	float timeLeft = 3.0f;
	void Start()
	{

	}

	// Update is called once per frame
	void Update () 
	{

		timeLeft -= Time.deltaTime;
		if (timeLeft < 0) 
		{

		}
	}
}
