using UnityEngine;
using System.Collections;

public class GateForce2 : MonoBehaviour {

	public AbraCadaverBossClass abra;
	private Rigidbody rb;
	public float forceMultiplier = 50f;
	public Rigidbody otherDoor;
	//public GameObject Gate1, Gate2;

	void Start () {
		rb = GetComponent<Rigidbody> ();

	}	

	public void AdjustCurHealth(DamageInfo damageInfo)
	{
		float val = damageInfo.damage;

		rb.isKinematic = false;
		rb.AddForce(Vector3.forward * -val * forceMultiplier);
		otherDoor.AddForce (Vector3.forward * -val * 10f);
		
		abra.StartEncounter ();
		Destroy (gameObject,3);
		try
		{
			Destroy(otherDoor.gameObject);
		}
		catch
		{

		}
		//Destroy (Gate2,3);

	}
}