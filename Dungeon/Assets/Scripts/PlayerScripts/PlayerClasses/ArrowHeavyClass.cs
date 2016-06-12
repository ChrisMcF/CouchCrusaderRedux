using UnityEngine;
using System.Collections;

public class ArrowHeavyClass : MonoBehaviour 
{

	private float timer;
	private GameObject archer;

    void OnTriggerEnter(Collider col)
    {
		if (col.tag == "Boss") 
		{
			col.SendMessage("AdjustCurHealth", -archer.GetComponent<BasePlayerClass>().actions.heavy.damage);
			archer.GetComponent<BasePlayerClass>().stats.specialAmount += archer.GetComponent<BasePlayerClass>().actions.heavy.damage * archer.GetComponent<BasePlayerClass>().stats.specialIncrement;
			//archer.GetComponent<BasePlayerClass>().CallAttack(2);

			Debug.Log ("hit");
			Destroy (gameObject);
		}
    }

	void Start ()
	{
		archer = GameObject.Find ("Archer");
	}

	void Update ()
	{
		transform.Translate (Vector3.forward * 100 * Time.deltaTime);
		
		timer += Time.deltaTime;
		
		if (timer >= 5f)
			Destroy (gameObject);
	}
}
