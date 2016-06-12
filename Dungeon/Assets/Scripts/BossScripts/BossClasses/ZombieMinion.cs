using UnityEngine;
using System.Collections;


public class ZombieMinion : MonoBehaviour {

	public float attackRange = 0.5f;
	public float damage = 20f;
	//private NavMeshAgent agent;
	public float curHealth;
	public float maxHealth;
	public float waitBeforeWalkTime = 1f;
	private MinionAudio minionAudio;
	
	public float attackDamage;
	public float moveSpeed;
	
	//public GameObject deathEffect;
	
	private NavMeshAgent _agent;

	private Transform _targetPlayer;
	private Animator _anim;
	
	private bool dead = false;
	
	private ScyllaBossClass scylla;
	
	private Collider col;
	public float[] playerAgro = new float[4];
	
	private Vector3 _previousPosition;
	HealthBarBehaviour _healthBarBehaviour;

	void Start ()
	{
		
		_healthBarBehaviour = transform.FindChild("HealthBar").GetComponent<HealthBarBehaviour>();

        col = GetComponent<Collider> ();
		if (damage > 0)
			damage = -damage;
		minionAudio = GetComponent<MinionAudio>();
		_agent = GetComponent<NavMeshAgent>();
		_agent.enabled = false;
		_anim = GetComponent<Animator>();
		if (curHealth != maxHealth)
			curHealth = maxHealth;
	}
	
	void Update ()
	{
		CheckIfWalking ();

		if (dead)
		{
			_agent.enabled = false;
			return;
		}
		/*
		if (waitBeforeWalkTime >= 0) {
			waitBeforeWalkTime -= Time.deltaTime;
		} else {
			if(!dead && !_agent.isActiveAndEnabled)
				_agent.enabled = true;
		}
		*/
		GetPlayerAgro();
		
		if (_targetPlayer != null)
		{
			if (!_targetPlayer.GetComponent<BasePlayerClass>()._isDead)
			{
				float distanceToPlayer = Vector3.Distance(transform.position, _targetPlayer.position);
				
				if (distanceToPlayer > attackRange)
				{
					_anim.SetBool("attacking", false);
					_agent.speed = moveSpeed;
					if(_agent.enabled == true)
						_agent.SetDestination(_targetPlayer.position);
				}
				else
				{
					_anim.SetBool("attacking", true);
					_agent.speed = 0;
				}
			}
			
		}    
		
		
		//TEMP TEST CODE FOR DEATH
		
		
		
		
	}


	void CheckIfWalking()
	{
		if (_previousPosition.x != transform.position.x && _previousPosition.z != transform.position.z) {
			_anim.SetBool ("moving", true);
			if(!dead && !_agent.isActiveAndEnabled)
				_agent.enabled = true;
		}
		else
		{
			_anim.SetBool ("moving", false);
		}
		_previousPosition = transform.position;
	}

	public void Attack()
	{
		Debug.Log(gameObject.name + ": Attacking");
		//	minionAudio.PlayRandomSound (minionAudio.attackSounds);
		if (!_targetPlayer.GetComponent<BasePlayerClass>()._isDead)
			_targetPlayer.SendMessage("AdjustCurHealth", damage);
	}
	
	void Die()
	{
		minionAudio.PlayRandomSound (minionAudio.deathSounds);
        // if (deathEffect != null)
        col.enabled = false;
		if(_agent.enabled)
			_agent.Stop();
		_anim.SetTrigger("die");
		dead = true;
		Destroy (this.gameObject, 10);
	}
	
	void GetPlayerAgro()
	{
		float highestAgro = 0;
		int playerIndex = 0;
		
		for (int i = 0; i < GameController.gameController.players.Count; i++)
		{
			if (playerAgro[i] >= highestAgro)
			{
				if (!GameController.gameController.players[i].playerClass._isDead)
				{
					//Debug.Log(players[i].gameObject.name);
					highestAgro = playerAgro[i];
					playerIndex = i;
				}
				
			}
		}
		
		_targetPlayer = GameController.gameController.players [playerIndex].playerObject.transform;
		// Debug.Log("TargetPlayer: " + players[playerIndex].name);
		
		
	}

	public void EnableAgent()
	{
		_agent.enabled = true;
	}

	public void GetPlayerThreat(float threat, GameObject player)
	{
		for (int index = 0; index < GameController.gameController.players.Count; index ++)
		{
			foreach (Player playerThreat in GameController.gameController.players)
			{
				if (playerThreat.playerObject.name == player.name)
				{
					playerAgro[index] += threat;
				}
				
				index ++;
			}
		}
		
	}
	
	public void AdjustCurHealth(DamageInfo damageInfo)
	{
		float val = damageInfo.damage;

		curHealth += val; // positive values is HEAL, negative is DAMAGE
		if (curHealth > 0 && val < 0) 
		{
			_anim.SetTrigger ("hit");
			minionAudio.PlayRandomSound(minionAudio.hurtSounds);
			_healthBarBehaviour.SetCurrentHealth(curHealth);
		}
		
		Debug.Log("Health: " + curHealth);
		if (curHealth < 0) {
			_healthBarBehaviour.SetCurrentHealth(0);
			curHealth = 0;
		}
		if (curHealth > maxHealth)
			curHealth = maxHealth;
		
		if (curHealth == 0 && !dead)
		{
			_healthBarBehaviour.SetCurrentHealth(0);
			_anim.SetTrigger("die");
			dead = true;
		}
		
		if (curHealth <= 0)
		{
			//GameObject death = GameObject.Instantiate(deathEffect, transform.position, transform.rotation) as GameObject;
			Die();
		}
	}
}
