using UnityEngine;
using System.Collections;

public class ScaleObj : MonoBehaviour {
	public float time, delay, scale;
	public iTween.EaseType curve;

	// Use this for initialization
	void Start () {
		iTween.ScaleFrom(gameObject, iTween.Hash ("time", time, "delay", delay, "scale", new Vector3(0, 0, 0), "easetype", curve));
	}
	

}
