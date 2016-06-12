using UnityEngine;
using System.Collections;

public class WarriorSpecialClass : MonoBehaviour {

	public BasePlayerClass warrior;

	// Use this for initialization
	void Start () {

		warrior = GameObject.Find ("Warrior").GetComponent<BasePlayerClass> ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.tag == "Boss" || col.tag == "Enemy")
		{
			col.gameObject.SendMessageUpwards ("AdjustCurHealth", -warrior.actions.special.damage, SendMessageOptions.DontRequireReceiver);
		}
	}
}
