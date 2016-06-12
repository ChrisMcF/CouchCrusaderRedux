using UnityEngine;
using System.Collections;

public class RotLoop : MonoBehaviour {

	public float time;
	public Vector3 rotTo;
	public iTween.EaseType ease;
	public iTween.LoopType loop;

	// Use this for initialization
	void Start () {
		iTween.RotateTo (gameObject, iTween.Hash("rotation", rotTo, "islocal", true, "time", time, "easetype", ease, "looptype", loop));
	}
	

}
