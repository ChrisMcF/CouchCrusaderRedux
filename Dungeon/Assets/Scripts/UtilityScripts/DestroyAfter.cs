﻿using UnityEngine;
using System.Collections;

public class DestroyAfter : MonoBehaviour {

	public float destroyTime = 5f;
	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, destroyTime);
	}

}
