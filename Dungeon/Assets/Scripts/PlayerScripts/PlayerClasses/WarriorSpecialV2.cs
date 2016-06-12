using UnityEngine;
using System.Collections;

public class WarriorSpecialV2 : MonoBehaviour {
	
	public float raycastDistance = 10f;
	public float raycastThickness = 0.2f;
	public float startDelay;
	//public float damage;

	BasePlayerClass _warrior;

    Player player = new Player();

	// Use this for initialization
	void Start () {
		_warrior = GameObject.Find ("Warrior").GetComponent<BasePlayerClass> ();
        player.playerClass = _warrior;
        player.playerObject = _warrior.gameObject;

        StartCoroutine("DoDamageAll");
	}

	IEnumerator DoDamageAll(){
		yield return new WaitForSeconds(startDelay);

		DamageAll ();
	}

	public void DamageAll(){
		//RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, raycastDistance);
		RaycastHit[] hits = Physics.SphereCastAll(transform.position, raycastThickness/2, transform.forward, raycastDistance);

        DamageInfo damageInfo = new DamageInfo(player, -_warrior.actions.special.damage, -_warrior.actions.special.damage * _warrior.threatMultiplier, false);
        foreach (RaycastHit hit in hits){
			if (hit.transform.tag == "Boss" || hit.transform.tag == "Enemy")
            {
				hit.transform.root.SendMessage("AdjustCurHealth", damageInfo);
			}
		}
	}
}
