using UnityEngine;
using System.Collections;

public class SlimeProjectileClass : MonoBehaviour {

	private float timer;
	private ScyllaBossClass scylla;
	
	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") 
		{
			col.GetComponent<BasePlayerClass> ().AdjustCurHealth (-scylla._attack * 2f);
			//scylla.CallAttack(3);
			//Debug.Log ("hit");
			Destroy (gameObject);
		}
	}
	
	void Start ()
	{
		scylla = GameObject.Find ("Scylla").GetComponent<ScyllaBossClass> ();
	}
	
	void Update ()
	{
		transform.Translate (Vector3.forward * 40 * Time.deltaTime);
		
		timer += Time.deltaTime;
		
		if (timer >= 5f)
			Destroy (gameObject);
	}
}
