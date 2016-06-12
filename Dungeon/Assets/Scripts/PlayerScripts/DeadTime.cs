using UnityEngine;
using System.Collections;

public class DeadTime : MonoBehaviour {
	public float deadTime = 1f;

	// Use this for initialization
	void Awake()
	{
		Destroy (gameObject, deadTime);
	}

}
