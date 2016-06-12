using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
	private BasePlayerClass _callingPlayer;
	private float _damage;
	private float _force;
	private Collider _player, _bath;
	private Rigidbody rb;
	private float destroyTimer = 3f;
	private DamageInfo _damageInfo;
    private bool _penetrate;
	//private DamageInfo _damageInfo;
	public GameObject onDestroyEffect;
	public bool isMageHeavyAttack = false;
	public bool exploded = false;
    private bool _isSpecial;

	void Start()
	{
		rb = GetComponent<Rigidbody>();

	}


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
            return;

        if (col.gameObject.layer == 16)
        {
			if(col.gameObject.tag == "Enemy")  
            	col.transform.root.SendMessage("AdjustCurHealth", _damageInfo, SendMessageOptions.RequireReceiver);
			else if(col.gameObject.tag == "Boss")
				col.transform.root.SendMessage("AdjustCurHealth", _damageInfo, SendMessageOptions.RequireReceiver);
            if (!_isSpecial)
            {
                _callingPlayer.stats.specialAmount += -_damage * _callingPlayer.stats.specialIncrement;
            }
        }
    }


    void OnCollisionEnter (Collision col)
	{
        if (col.gameObject.tag == "Player")
            return;

		if (col.gameObject.layer == 16)
        {
            Debug.Log("Projectile Hit: " + col.gameObject.name);
			if(col.gameObject.tag == "Enemy")
				col.transform.root.SendMessage ("AdjustCurHealth", _damageInfo, SendMessageOptions.RequireReceiver);
			else if(col.gameObject.tag == "Boss")
				col.transform.root.SendMessage ("AdjustCurHealth", _damageInfo, SendMessageOptions.RequireReceiver);
            if (!_isSpecial)
            {
                _callingPlayer.stats.specialAmount += -_damage * _callingPlayer.stats.specialIncrement;
            }
            //Destroy (gameObject);
            StartCoroutine("ExplodeProjectile", 0f);
		} else {

            if (onDestroyEffect != null)
            {
                StartCoroutine("ExplodeProjectile", 0f);
            }
            else
                Destroy(gameObject);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		destroyTimer -= Time.deltaTime;
		if (destroyTimer <= 0) {
			StartCoroutine("ExplodeProjectile", 0f);
		}

		//transform.Translate (Vector3.forward * _speed * Time.deltaTime);
	}

	IEnumerator PropelProjectile()
	{
		yield return new WaitForFixedUpdate();
		rb.AddForce (transform.forward * _force, ForceMode.Impulse);
	}

	void MageHeavyProjectile ()
	{
		float spellTime = 2.4f;
		float distance = 8;
		Vector3[] pathPoints = new Vector3[2];
		pathPoints [0] = transform.position;
		pathPoints [1] = transform.position + transform.forward * distance;
		iTween.MoveTo (gameObject, iTween.Hash ("path", pathPoints, "time", spellTime, "easetype", iTween.EaseType.easeOutExpo));
		ExplodeProjectile (spellTime);

	}

	IEnumerator ExplodeProjectile(float waitTime)
	{
		if (!exploded) 
		{
			exploded = true;
			yield return new WaitForSeconds (waitTime);
			Debug.Log("Wait time is over");
			if (onDestroyEffect != null) {
				Debug.Log ("Creating Explosion");
				GameObject instance = Instantiate (onDestroyEffect, transform.position, transform.rotation) as GameObject;
				MageExplosionAOE myExplosion = instance.GetComponent<MageExplosionAOE> ();

				myExplosion.StartCoroutine("ApplyAOEDamage", _damageInfo);
			}
            //Debug.Log ("Disabling Mesh renderer");

            if (!_penetrate)
            {
                foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
                {
                    mr.enabled = false;
                }
                rb.isKinematic = true;
                Destroy(gameObject);
            }
			    
		
		}
	}

	public void Setup (DamageInfo damageInfo, GameObject callingPlayer, float damage, float force, bool penetrate = false, bool isSpecial = false)
	{
        if(penetrate)
        {
            _penetrate = true;  
            GetComponent<BoxCollider>().isTrigger = true;
        }
		_damageInfo = damageInfo;
        _callingPlayer = callingPlayer.GetComponent<BasePlayerClass> ();
		_damage = damage;
		_force = force;
        _isSpecial = isSpecial;
        Debug.Log("isSpecial = " + isSpecial);
		if (!isMageHeavyAttack)
			StartCoroutine ("PropelProjectile");
		else
			MageHeavyProjectile ();
	}
}
