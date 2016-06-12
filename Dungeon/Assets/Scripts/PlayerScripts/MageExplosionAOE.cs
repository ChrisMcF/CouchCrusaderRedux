using UnityEngine;
using System.Collections;

public class MageExplosionAOE : MonoBehaviour
{
	//private SphereCollider myCollider;
	//public LayerMask collideWith;

//	public float damage = 50f;
	Collider[] hits;
	public float radius = 2.6f;
	public float waitTime = 0.6f; 
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public IEnumerator ApplyAOEDamage (DamageInfo damageInfo)
	{
		yield return new WaitForSeconds (waitTime);

		//Ensure that damage value is negative
		if (damageInfo.damage > 0)
			damageInfo.damage = -damageInfo.damage;

		Debug.Log (transform.position);
		hits = Physics.OverlapSphere (transform.position, radius);

		foreach (Collider col in hits) {
			Debug.Log ("Mage Explosion Hit");
			//Collider col = hit.collider;
		
			if (col.gameObject.tag == "Boss" && col.gameObject.layer == 16) 
				col.transform.root.SendMessage ("AdjustCurHealth", damageInfo, SendMessageOptions.RequireReceiver);
			if (col.gameObject.tag == "Enemy" && col.gameObject.layer == 16) 
				col.transform.root.SendMessage ("AdjustCurHealth", damageInfo, SendMessageOptions.RequireReceiver);
	
		}
	}
}
