using UnityEngine;
using System.Collections;

public class PaladinSpecial : MonoBehaviour {


    private float _range;
    private float _damage;
    private float _destroyTime;
    Player p = new Player();
    DamageInfo dInfo;

    public LayerMask enemyLayer;


    void Initialise(float [] info)
    {
        _range = info[0];
        _damage = info[1];
        _destroyTime = info[3];
        p.playerObject = gameObject.transform.root.gameObject;
        p.playerClass = p.playerObject.GetComponent<BasePlayerClass>();
        dInfo = new DamageInfo(p, _damage, _damage * p.playerClass.threatMultiplier, true);
        Destroy(gameObject, _destroyTime);
    }
	
	// Update is called once per frame
	void Update ()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, _range, enemyLayer);
        foreach (Collider target in targets)
        {
            if (target.tag == "Boss" || target.tag == "Enemy")
            {
                    dInfo = new DamageInfo(p, _damage *Time.deltaTime, _damage * p.playerClass.threatMultiplier, true);

                    target.transform.root.SendMessage("AdjustCurHealth", dInfo, SendMessageOptions.RequireReceiver);
            }
        }
    }
}
