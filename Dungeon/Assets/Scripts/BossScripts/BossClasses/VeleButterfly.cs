using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class VeleButterfly : MonoBehaviour {
	
	public float moveSpeed, rotSpeed, curHealth, maxHealth, detectArc;
	public int form;
	public float processSpeed, attackTime, cooldownTimer, chargeTime, meleeRange, missileRange;

	public GameObject castNode,HitEffect, castEffect, missilePrefab, missileHit, missileSpawn;
	public Transform targetEnemy;

	public float targetRadius;
	public float orbSpeed;
	public float orbLifetime;
	public float orbDamage;
	Vector3 _targetPosition;

	private CharacterController _controller;
	private Animator _animator;
	private Vector3 _tPos;
	public List<GameObject> players = new List<GameObject>();
	
	// Use this for initialization
	void Start () {
		SizeUp ();
		foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player")) {
			players.Add(p);
		}
		_controller = gameObject.GetComponent<CharacterController>();
		_animator = gameObject.GetComponent<Animator>();

		_animator.SetInteger ("Phase", form);
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo _info = _animator.GetCurrentAnimatorStateInfo (0);



		if (!_info.IsTag ("InAttack")) {
			if (_animator.GetInteger ("AttIndex") != 0) {
				_animator.SetInteger ("AttIndex", 0);
			}
		} else {
			if (form == 1) {
				if (Vector3.Distance (transform.position, _tPos) < meleeRange) {
					_animator.SetInteger ("AttIndex", 0);
					_tPos = Vector3.zero;
					chargeTime = 0;
				}
			}
		}

		if (cooldownTimer > 0)
			cooldownTimer -= Time.deltaTime;
		if (_tPos != Vector3.zero) {
			if (chargeTime > 0)
				chargeTime -= Time.deltaTime;
			else
			{
				chargeTime = 0;
				_animator.SetInteger ("AttIndex", 0);
				_tPos = Vector3.zero;
			}
		}

		
		if (targetEnemy != null) {
			Vector3 rPos = targetEnemy.position;
			if(_tPos != Vector3.zero)
					rPos = _tPos;
			float rotation = _animator.GetFloat ("RSpeed");
			Quaternion wantedRot = Quaternion.LookRotation ((rPos - transform.position).normalized);
			Quaternion currentRot = Quaternion.Euler (0, Mathf.LerpAngle(transform.rotation.eulerAngles.y, wantedRot.eulerAngles.y,(rotSpeed * Time.deltaTime)*rotation),0);
			transform.rotation = currentRot;
			
			Vector3 dir = (targetEnemy.position - transform.position).normalized;
			if (Vector3.Dot (dir, transform.forward) > detectArc) {
				if (cooldownTimer <= 0) {
					_animator.SetInteger ("AttIndex", 1);
					_tPos = targetEnemy.position;
					chargeTime = 4f;
					cooldownTimer = attackTime;
				}
			}
			
		
			
		} else {
			targetEnemy = CheckForTargetClosest();
		}
		float projection = _animator.GetFloat ("ZSpeed");
		Vector3 _dir = new Vector3(0, -10, (moveSpeed*Time.deltaTime)*projection);
		_controller.Move(transform.TransformDirection(_dir));


		
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
		foreach (GameObject target in players.ToArray()) 
		{
			if(Vector3.Distance(transform.position, target.transform.position) < detectRange)
			{
				Vector3 dir = (target.transform.position - transform.position).normalized;
				if(Vector3.Dot(dir, transform.forward) > detectArc)
				{
					t = target.transform;
				}
			}
		}
		return t;
	}
	Transform CheckForTargetClosest()
	{
		float detectRange = 100;
		Transform t = null;
		foreach (GameObject target in players.ToArray()) 
		{
			if(Vector3.Distance(transform.position, target.transform.position) < detectRange)
			{
				detectRange = Vector3.Distance(transform.position, target.transform.position);
				t = target.transform;
			}
		}
		return t;
	}
	
	
	public void CallAttack(int index)
	{
		switch (index) 
		{
		case 2:
			StartCoroutine("ShootOrb", targetEnemy);
			break;
		case 1:
			MeleeAttack(0, 2, 0.4f);
			break;
		}
	}
	


	public IEnumerator ShootOrb (Transform _target) {
		Vector3 _targetPosition;
		
		


		bool _hitPlayer=false;
		Vector3 _orbSpawnPos = castNode.transform.position;
		GameObject _orb = (GameObject)Instantiate (missilePrefab, _orbSpawnPos, transform.rotation);
		
		float _orbLifetime_timeLeft = orbLifetime;
		_targetPosition = _target.position + Vector3.up;// Set here for first iteration
		
		while (Vector3.Distance (_orb.transform.position, _targetPosition) > targetRadius && _orbLifetime_timeLeft > 0) {
			
			//The position of the middle of the player (since the player's transform is at the bottom of their feet)
			_targetPosition = _target.position + Vector3.up;
			
			//Look at the target
			//_orb.transform.LookAt (_targetPosition);
			//Move towards the target
			_orb.transform.Translate (Vector3.forward * orbSpeed * Time.deltaTime);
			
			//Update the time left until the orb is automatically destroyed
			_orbLifetime_timeLeft -= Time.deltaTime;
			
			if (Vector3.Distance (_orb.transform.position, _targetPosition) <= targetRadius) {
				
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
		foreach (GameObject target in players.ToArray()) 
		{
			if(Vector3.Distance(transform.position, target.transform.position) < meleeRange)
			{
				Vector3 dir = (target.transform.position - transform.position).normalized;
				if(Vector3.Dot(dir, transform.forward) > meleeArc)
				{
					Quaternion rot = Quaternion.LookRotation((target.transform.position - transform.position));
					
					GameObject.Instantiate(HitEffect,
					                       target.transform.position,
					                       rot);
				}
			}
		}
	}
	

	
	public void AdjustCurHealth(float val)
	{
		
		curHealth += val; // positive values is HEAL, negative is DAMAGE
		
		if (curHealth < 0)
			curHealth = 0;
		
		if (curHealth > maxHealth)
			curHealth = maxHealth;
		
		if (curHealth == 0)
		{
			Destroy(gameObject);
		}
		
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
	
}

