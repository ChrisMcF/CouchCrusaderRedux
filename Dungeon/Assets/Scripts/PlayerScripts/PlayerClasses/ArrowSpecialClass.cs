using UnityEngine;
using System.Collections;

public class ArrowSpecialClass : MonoBehaviour {

	
	private float timer;
	private GameObject archer;
    private Rigidbody rb;

    void OnCollisionEnter(Collision col)
	{
        if (col.gameObject.tag == "Player")
            return;

        if (col.gameObject.name == "Arrow")
        {
            Physics.IgnoreCollision(col.gameObject.GetComponent<BoxCollider>(), gameObject.GetComponent<BoxCollider>());
            return;
        }
            
        

        if (col.gameObject.tag == "Boss" || col.gameObject.tag == "Enemy") 
		{
	        col.gameObject.SendMessageUpwards("AdjustCurHealth", -(archer.GetComponent<BasePlayerClass>().actions.special.damage), SendMessageOptions.DontRequireReceiver);
		}
        else
        {
            Debug.Log(col.gameObject.name);
            foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
            {
                mr.enabled = false;
            }
            rb.isKinematic = true;
        }
	}
	
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.forward * 10, ForceMode.Impulse);
        archer = GameObject.Find ("Archer");
	}
	
	void Update ()
	{
		//transform.Translate (Vector3.forward * 100 * Time.deltaTime);
		
		timer += Time.deltaTime;
		
		if (timer >= 5f)
			Destroy (gameObject);
	}
}
