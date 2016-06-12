using UnityEngine;
using System.Collections;

public class Psyclops : BaseBossClass {

	private float agentSpeed;
	public float idleBelowSpeed = 0.1f;
	public float walkBelowSpeed = 10f;
	private bool ignoreThreat = false;
	public float chargeSpeed = 25f;
	public float chargeAcceleration = 25f;
	public float chargeOverShoot = 3f;
	public float rangeFromDestinationToEndCharge = 1f;
	public float meleeAttackRange = 2f;
	private float agentSpeedAtStart;
	private float agentAccelerationAtStart;
	public float timeBetweenMeleeAttacks = 1.5f;
	private 
	// Use this for initialization
	void Start () 
	{
		BossStart (); //calls the BossStart() method in the boss base class
		agentSpeedAtStart = _agent.speed;
		agentAccelerationAtStart = _agent.acceleration;
		InvokeRepeating ("DoMeleeAttackIfWithinRange", timeBetweenMeleeAttacks, timeBetweenMeleeAttacks);
	}
	
	// Update is called once per frame
	void Update () 
	{
		HandleIdleWalRunAnimations ();
		if (!ignoreThreat) {
			_targetPlayer = GetPlayerWithHigestThreat ().playerObject.transform;
			if(_animator.isActiveAndEnabled)
				_agent.SetDestination (_targetPlayer.position);
		}

	}

	void DoMeleeAttackIfWithinRange()
	{
		if(Vector3.Distance(_targetPlayer.position, transform.position) < meleeAttackRange)
			RandomMeleeAttack ();
	}

	void HandleIdleWalRunAnimations()
	{
		//Determines whether the character should be idle/walking/running based upon the NavMeshAgent.
		agentSpeed = GetMagnitudeOfNavMeshAgentVelocity ();
		//Debug.Log ("Agent Speed = " + agentSpeed);
		if (agentSpeed < idleBelowSpeed) 
		{
			_animator.SetBool ("Walking", false);
			_animator.SetBool ("Running", false);
		} 
		else if (agentSpeed < walkBelowSpeed) 
		{
			_animator.SetBool ("Walking", true);
			_animator.SetBool ("Running", false);
		} 
		else 
		{
			_animator.SetBool("Walking", false);
			_animator.SetBool("Running", true);
		}
	}

	void RandomMeleeAttack()
	{
		if (Vector3.Distance (transform.position, _targetPlayer.position) < meleeAttackRange) 
		{
			int randomInt = Random.Range(1,3) ;
			if(randomInt == 1)
			{
				_animator.SetInteger ("MeleeAttack", 1);
			}else
			{
				_animator.SetInteger ("MeleeAttack", 2);
			}
			_animator.SetTrigger("DoMeleeAttack");
		}
	}


	void DoMeleeAttack()
	{
		if (_animator.GetInteger ("MeleeAttack") == 1) 
		{
			//Do Damage
		}

		else if (_animator.GetInteger ("MeleeAttack") == 2) 
		{
			//Do Damage
		}
	}



	float GetMagnitudeOfNavMeshAgentVelocity()
	{
		return _agent.velocity.magnitude;
	}

	void ChargeAtTargetTfNoObsticles( Transform target)
	{
		if (!Physics.Linecast (transform.position, target.position))
			ChargeAt (target);
	}

	IEnumerator ChargeAt(Transform target)
	{
		ignoreThreat = true;
		_targetPlayer = target;
		_agent.acceleration = chargeAcceleration;
		_agent.speed = chargeSpeed;
		Vector3 chargeDestination = target.position += ((target.position - transform.position).normalized * chargeOverShoot);
		_agent.SetDestination (chargeDestination);
		while(Vector3.Distance(chargeDestination, transform.position) > rangeFromDestinationToEndCharge)
		{
			yield return null;
		}
		_agent.acceleration = agentAccelerationAtStart;
		_agent.speed = agentSpeedAtStart;
		ignoreThreat = false;	

		_targetPlayer = GetPlayerWithHigestThreat ().playerObject.transform;
	}
}


