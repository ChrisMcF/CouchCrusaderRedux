using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class VeleBoss : MonoBehaviour {
	public float talkPercentage = 10f;
	public float moveSpeed, rotSpeed, curHealth, maxHealth, gravity;
	public float processTick, detectArc, attackCool, attackTime;
	protected int AttFlag;
	protected CharacterController _char;
	protected Animator _anim;
	protected AnimatorStateInfo _info;
	protected float _y, _x, _r;
	public GameObject deathFX, deathPoint;
	public bool removeOnDeath;

	public int form;
	public float chargeTime, meleeRange, spinRange;
	public GameObject castNode, stabNode, headNode1,headNode2;
	public GameObject headEffect1,headEffect2, LineEffect, SpinEffect, StabEffect, DrillEffect, hitEffect, castEffect, missilePrefab, missileHit, missileSpawn;
	protected GameObject target;

	public float targetRadius;
	public float orbSpeed;
	public float orbLifetime;
	public float orbDamage;
	public float orbRot;

	Vector3 _targetPosition;
	private Vector3 _tPos;
	public WeaponTrail TrailMount;
	private List<WeaponTrail> trails = new List<WeaponTrail>();
	public List<WeaponTrail> WeaponTrails {
		get { return trails; }
		set { trails = value; }
	}
	protected float t = 0.033f;
	private float tempT = 0;
    private Slider _healthBar;
    private FleshPulse fleshPulse;
	private bool encounterStarted = false;
    private bool dead = false;
	private VeleAudio veleAudio;
	// Use this for initialization
	void Start () {
		SizeUp ();
		veleAudio = GetComponent<VeleAudio> ();
        maxHealth *= GameController.gameController.players.Count;
        _healthBar = GameObject.Find("Boss_HealthBar").GetComponent<Slider>();
        _healthBar.maxValue = maxHealth;
        _healthBar.value = maxHealth;
		curHealth = maxHealth;
		_healthBar.transform.parent.gameObject.SetActive (false);
        fleshPulse = GameObject.Find("InnerGround").GetComponent<FleshPulse>();

        _char = gameObject.GetComponent<CharacterController>();
		_anim = gameObject.GetComponent<Animator>();

		_anim.SetInteger ("Phase", form);

		WeaponTrails.Add (TrailMount);
		SetTrailOff ();
		ScanForEnemy ();
	}
	
	// Update is called once per frame
	void Update () {

		if (encounterStarted) {
			if (curHealth <= 0)
				return;


			if (form == 1) {
				if (curHealth < maxHealth / 2) {
					form = 2;
					_anim.SetInteger ("Phase", form);
				}
			}


			if (_anim == null) {
				Debug.Log ("ANIMATOR!!");
			}
			AnimatorStateInfo _info = _anim.GetCurrentAnimatorStateInfo (0);



			if (!_info.IsTag ("InAttack")) {
				if (_anim.GetInteger ("AttIndex") != 0) {
					_anim.SetInteger ("AttIndex", 0);
				}
			} else {
				if (form == 1) {
					if (Vector3.Distance (transform.position, _tPos) < meleeRange) {
						_anim.SetInteger ("AttIndex", 0);
						_tPos = Vector3.zero;
						chargeTime = 0;
					}
				}
			}

			if (attackTime > 0)
				attackTime -= Time.deltaTime;
			if (_tPos != Vector3.zero) {
				if (chargeTime > 0)
					chargeTime -= Time.deltaTime;
				else {
					chargeTime = 0;
					_anim.SetInteger ("AttIndex", 0);
					_tPos = Vector3.zero;
					attackTime = attackCool * 2.5f;
				}
			}

		
			if (target != null) {
				Vector3 rPos = target.transform.position;
				if (_tPos != Vector3.zero)
					rPos = _tPos;
				float rotation = _anim.GetFloat ("RSpeed");
				Quaternion wantedRot = Quaternion.LookRotation ((rPos - transform.position).normalized);
				Quaternion currentRot = Quaternion.Euler (0, Mathf.LerpAngle (transform.rotation.eulerAngles.y, wantedRot.eulerAngles.y, (rotSpeed * Time.deltaTime) * rotation), 0);
				transform.rotation = currentRot;

				Transform t = CheckForTargetClosest ();
				if (Vector3.Distance (transform.position, t.position) < spinRange) {
					if (attackTime <= 0) {
						int r = Random.Range (0, form);
						if (Vector3.Distance (transform.position, t.position) < spinRange / 2) {
							_anim.SetInteger ("AttIndex", 1 + r);
						} else {
							_anim.SetInteger ("AttIndex", 3 + r);
						}
						attackTime = attackCool;
					}
				} else {
					Vector3 dir = (target.transform.position - transform.position).normalized;
					if (Vector3.Dot (dir, transform.forward) > detectArc) {
						if (attackTime <= 0) {
							int r = Random.Range (0, form);
							_anim.SetInteger ("AttIndex", 5 + r);
							_tPos = target.transform.position;
							chargeTime = 4f;
							attackTime = attackCool;
						}
					}
				}
		
			
			} else {
				target = CheckForTargetClosest ().gameObject;
			}
			float projection = _anim.GetFloat ("ZSpeed");
			Vector3 _dir = new Vector3 (0, -10, (moveSpeed * Time.deltaTime) * projection);
			_char.Move (transform.TransformDirection (_dir));


		}
	}
	

	void SizeUp()
	{
		iTween.ScaleFrom (gameObject, iTween.Hash ("scale", Vector3.zero, "time", 0.4f, "easetype", iTween.EaseType.easeOutExpo));
	}
	void SizeDown()
	{
		iTween.ScaleTo(gameObject, iTween.Hash ("scale", Vector3.zero, "time", 0.4f, "easetype", iTween.EaseType.easeInExpo));
	}
	Transform CheckForTargetInfront()
	{
		float detectRange = 100;
		Transform t = null;
		foreach (Player targ in GameController.gameController.allPlayers) 
		{
			if(Vector3.Distance(transform.position, targ.playerObject.transform.position) < detectRange)
			{
				Vector3 dir = (targ.playerObject.transform.position - transform.position).normalized;
				if(Vector3.Dot(dir, transform.forward) > detectArc)
				{
					t = targ.playerObject.transform;
				}
			}
		}
		return t;
	}
	Transform CheckForTargetClosest()
	{
		float detectRange = 100;
		Transform t = null;
		foreach (Player targ in GameController.gameController.allPlayers) 
		{
			if(Vector3.Distance(transform.position, targ.playerObject.transform.position) < detectRange)
			{
				detectRange = Vector3.Distance(transform.position, targ.playerObject.transform.position);
				t = targ.playerObject.transform;
			}
		}
		return t;
	}
	
	
	public void CallAttack(int index)
	{
		if (encounterStarted) {
			veleAudio.PercentageChanceToPlayRandomVoiceClip (veleAudio.inFightSounds, talkPercentage);
			float range = 0, damage = 0, arc = 0;
			switch (index) {
			case 1:
				arc = 0.5f;
				range = 4f;
				damage = 15f;
				break;
			case 2:
			//Orbs
				veleAudio.PlayRandomSoundEffectClip (veleAudio.rangedAttackSounds);
				StartCoroutine ("ShootOrb", target.transform);
				arc = 0f;
				range = 0f;
				damage = 0f;
				break;
			case 3: // Spin Melee Attack - Fires 3 times
				arc = -1f;
				range = 3.5f;
				damage = 5f;
				GameObject.Instantiate (SpinEffect, transform.position, transform.rotation);
				break;
			case 4: // Drill Attack - Fires 3 Times
				arc = 0.7f;
				range = 6f;
				damage = 7f;
				break;
			case 5: // Sword attack - Fires 1 Time
				arc = 0.8f;
				range = 15f;
				damage = 12f;
				GameObject.Instantiate (LineEffect, transform.position, transform.rotation);
				break;
			case 6: // Ground stab attack
				arc = -1f;
				range = 4f;
				damage = 17f;
				GameObject.Instantiate (StabEffect, stabNode.transform.position, stabNode.transform.rotation);
				break;
			case 7:
				arc = 0;
				range = 0;
				damage = 0;
				GameObject.Instantiate (DrillEffect, stabNode.transform.position, stabNode.transform.rotation);
				break;
			}
			DamageScan (range, damage, arc, tag); 
			if (chargeTime <= 0)
				_anim.SetInteger ("AttIndex", 0);
		}
	}
	
	public IEnumerator ShootOrb (Transform _target) {
		Vector3 _targetPosition;


		bool _hitPlayer=false;
		Vector3 _orbSpawnPos = castNode.transform.position;
		if(form == 2)
			_orbSpawnPos = missileSpawn.transform.position;
		GameObject _orb = (GameObject)Instantiate (missilePrefab, _orbSpawnPos, transform.rotation);
		Instantiate (castEffect, _orbSpawnPos, transform.rotation);

		float _orbLifetime_timeLeft = orbLifetime;
		_targetPosition = _target.position + Vector3.up;// Set here for first iteration
		_orb.transform.LookAt (_targetPosition + new Vector3(Random.Range(-10, 10),Random.Range(-10, 10),Random.Range(-10, 10)));
		while (Vector3.Distance (_orb.transform.position, _targetPosition) > targetRadius && _orbLifetime_timeLeft > 0) {
			
			//The position of the middle of the player (since the player's transform is at the bottom of their feet)
			_targetPosition = _target.position + Vector3.up;

			Vector3 currentRot, wantedRot;
			
			wantedRot = Quaternion.LookRotation ((target.transform.position - 
			                                      _orb.transform.position).
			                                     normalized).eulerAngles;
			
			currentRot = Vector3.Lerp (_orb.transform.rotation.eulerAngles,
			                           wantedRot,
			                           orbRot * Time.deltaTime);
			
			_orb.transform.rotation = Quaternion.Euler (currentRot);

			//Look at the target
			//_orb.transform.LookAt (_targetPosition);
			//Move towards the target
			_orb.transform.Translate (Vector3.forward * orbSpeed * Time.deltaTime);
			
			//Update the time left until the orb is automatically destroyed
			_orbLifetime_timeLeft -= Time.deltaTime;
			
			if (Vector3.Distance (_orb.transform.position, target.transform.position) <= targetRadius) {
				
				_hitPlayer = true;
			}
			
			yield return null;
		}
		
		if (_hitPlayer){
			
			_target.SendMessage ("AdjustCurHealth", -orbDamage);
		}
		
		GameObject _orbDeathEffect = (GameObject)Instantiate (missileHit, _orb.transform.position, missileHit.transform.rotation);
		//Destroy the orb death effect after its lifetime has ended
		Destroy (_orbDeathEffect, _orbDeathEffect.GetComponent<ParticleSystem> ().startLifetime);
		Destroy (_orb);
		//StopCoroutine ("ShootOrb");
	}

	
	void MeleeAttack(float maxDamage, float meleeRange, float meleeArc)
	{
		//veleAudio.PercentageChanceToPlayRandomVoiceClip (veleAudio.inFightSounds, talkPercentage);
		veleAudio.PlayRandomSoundEffectClip (veleAudio.meleeAttackSounds);
		foreach (Player targ in GameController.gameController.allPlayers ) 
		{
			if(Vector3.Distance(transform.position, targ.playerObject.transform.position) < meleeRange)
			{
				Vector3 dir = (targ.playerObject.transform.position - transform.position).normalized;
				if(Vector3.Dot(dir, transform.forward) > meleeArc)
				{
					Quaternion rot = Quaternion.LookRotation((targ.playerObject.transform.position - transform.position));
					
					GameObject.Instantiate(hitEffect,
					                       targ.playerObject.transform.position,
					                       rot);
				}
			}
		}
	}
	

	void SplitHead(int v)
	{
		if(v == 1)
		GameObject.Instantiate(headEffect1,
		                       headNode1.transform.position,
		                       headNode1.transform.rotation);
		else if(v == 2)
		GameObject.Instantiate(headEffect2,
		                       headNode2.transform.position,
		                       headNode2.transform.rotation);
	}

	
	void MakeEffect(GameObject obj)
	{
		GameObject fx = GameObject.Instantiate (obj, transform.position, transform.rotation) as GameObject;
		if (obj.name == "ButterflyDrill") {

			fx.name = "Drill";
		}
		fx.transform.parent = transform;

	}
	void RemoveEffect(string obj)
	{
		if (transform.FindChild (obj) != null)
			Destroy (transform.FindChild (obj).gameObject);
	}

	void LateUpdate()
	{
		t = Mathf.Clamp (Time.deltaTime, 0, 0.066f);
		
		if (t > 0) 
		{
			while (tempT < t) 
			{
				tempT += 0.003f;
				
				for (int i = 0; i < WeaponTrails.Count; i++) 
				{
					if (WeaponTrails[i].time > 0) 
					{
						WeaponTrails[i].Itterate (Time.time - t + tempT);
					} 
					else 
					{
						WeaponTrails[i].ClearTrail ();
					}
				}
			}
			tempT -= t;
			for (int i = 0; i < WeaponTrails.Count; i++) {
				
				if (WeaponTrails[i].time > 0) {
					//Debug.Log(WeaponTrails[i].time);
					WeaponTrails[i].UpdateTrail (Time.time, t);
				}
			}
		}
	}
	
	
	
	void SetTrailOn()
	{
		TrailMount.StartTrail(0f, 0.2f);
		TrailMount.SetTime (0.4f, 0.4f, 0.2f);
	}
	
	void SetTrailOff()
	{
		TrailMount.FadeOut (0.4f);
	}

	public void AdjustCurHealth(DamageInfo damageInfo)
	{
		if (encounterStarted) 
		{
			_anim.SetTrigger ("TakeDamage");

			curHealth += damageInfo.damage; //POSITIVE means heal, NEGATIVE means damage
		
			if (maxHealth <= 0) //did i forget to set max health?
				maxHealth = 10;
		
			if (curHealth > maxHealth) //do i have more health than i should?
				curHealth = maxHealth;

			_healthBar.value = curHealth;

			fleshPulse.SetNerves (1 - (curHealth / maxHealth));

			if (curHealth > 0) {
				veleAudio.PlayRandomVoiceClip (veleAudio.hurtSounds);
			}

			if (curHealth <= 0 && !dead) { //I died
				dead = true;
				curHealth = 0;
				_anim.SetBool ("isDead", true);
				GameController.gameController.StartCoroutine ("Victory");
				_healthBar.transform.GetChild (1).GetChild (0).gameObject.SetActive (false);
				veleAudio.PlayRandomVoiceClip (veleAudio.deathSounds);
				//DIE
			}
		}
	}
	
	public void CallDeath()
	{
		Vector3 pos = transform.position;
		if (deathPoint != null) {
			pos = deathPoint.transform.position;
		}
		GameObject.Instantiate (deathFX, pos, transform.rotation);

		if(removeOnDeath)
			Destroy(gameObject);
	}
	
	
	
	public void DamageScan(float range, float damage, float arc, string tag)
	{
		//check all loaded objects of tag
		foreach (Player targ in GameController.gameController.allPlayers ) 
		{
			//are they close enough to me?
			if(Vector3.Distance(targ.playerObject.transform.position, transform.position) < range)
			{
				//are they within the scan FOV?
				Vector3 _dir = (targ.playerObject.transform.position - transform.position).normalized;
				if(Vector3.Dot(transform.forward, _dir) > arc)
				{
					targ.playerClass.AdjustCurHealth(-damage); //deal the damage
				}
			}
		}
	}

	void ScanForEnemy()
	{	
		if (curHealth <= 0)
			return;
		
		float tRange = 1000;
		GameObject tTarg = null;
		
		//check all loaded objects of tag
		foreach (Player targ in GameController.gameController.allPlayers ) 
		{
			//are they close enough to me?
			if(Vector3.Distance(targ.playerObject.transform.position, transform.position) < tRange)
			{
				//are they within the scan FOV?
				Vector3 _dir = (targ.playerObject.transform.position - transform.position).normalized;
				if(Vector3.Dot(transform.forward, _dir) > detectArc)
				{
					tRange = Vector3.Distance(targ.playerObject.transform.position, transform.position);
					tTarg = targ.playerObject;
				}
			}
		}
		if (tTarg != null)
			target = tTarg;
		
		Invoke ("ScanForEnemy", processTick);
	}

	public void StartEncounter()
	{
		_healthBar.transform.parent.gameObject.SetActive (true);
		encounterStarted = true;
		veleAudio.PlayRandomVoiceClip (veleAudio.encounterStart);
	}
	
}

