using UnityEngine;
using System.Collections;

public class CrumbleZombie : MonoBehaviour {
	private Animator anim;
	public GameObject[] disableThese;
	public Rigidbody[] physicsObjects;
	public bool useTimer;
	public float crumbleTime = 5f;
	public GameObject pelvis;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	void Update()
	{
		if (useTimer) 
		{
			if(crumbleTime > 0)
			{
				crumbleTime -= Time.deltaTime;
			}
			else
			{
				Crumble ();
				useTimer = false;
			}
		}
	}
		
	void Crumble()
	{
		Destroy (pelvis);
		Debug.Log ("Crumble");
		anim.enabled = false;
		//foreach (GameObject g in disableThese) {
		//	g.SetActive(false);
		//}
		foreach (Rigidbody rb in physicsObjects) {
			rb.isKinematic = false;
		}
	}
}
