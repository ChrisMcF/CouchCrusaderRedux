using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ScyllaMinionClass : MonoBehaviour {

	#region Public Variables
   
	public float curHealth;
    public float maxHealth;
	public float attackDamage;
    public float attackCooldownTime;
    private float attackCooldown;
    public float moveSpeed;	
	public GameObject deathEffect;
	public List <float> playerAgro = new List<float> ();

	#endregion

	#region Private Variables

	private MinionAudio minionAudio;
    private NavMeshAgent _agent;
    private Transform _targetPlayer;
    private Animator _anim;
	private Collider col;
    private bool dead = false;
	private ScyllaBossClass scylla;
    private HealthBarBehaviour healthBar;

	#endregion

	#region Unity Functions    

	void Start ()
    {
        attackCooldown = attackCooldownTime;
        for (int i = 0; i < GameController.gameController.players.Count - 1; i++)
            playerAgro.Add(0);

		col = GetComponent<Collider> ();
		minionAudio = GetComponent<MinionAudio>();
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();

		if (curHealth != maxHealth)
			curHealth = maxHealth;
        healthBar = GetComponentInChildren<HealthBarBehaviour>();
        healthBar.InitializeMaxAndCurrentHealth(maxHealth);
    }
	
	void Update ()
    {


        attackCooldown -= Time.deltaTime;
        if (dead)
		{
			_agent.enabled = false;
            return;
		}

		GetPlayerAgro();

        if (_targetPlayer != null)
        {
            if (!_targetPlayer.GetComponent<BasePlayerClass>()._isDead)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, _targetPlayer.position);

                if (distanceToPlayer > 15f)
                    return;

                if (distanceToPlayer > 4f)
                {

                    _anim.SetBool("moving", true);
                    _anim.SetBool("attacking", false);
                    _agent.speed = moveSpeed;
                    _agent.SetDestination(_targetPlayer.position);
                }
                else
                {
                    if (attackCooldown <= 0)
                    {
                        _anim.SetBool("moving", false);
                        _anim.SetBool("attacking", true);
                        _agent.speed = 0;
                        attackCooldown = attackCooldownTime;
                    }
                }
            }
            
      	}     
	}

	#endregion

	#region Threat Functions

	void GetPlayerAgro()
	{
		float highestAgro = 0;
		int playerIndex = 0;
		if (GameController.gameController.players.Count > 0) {
	        for (int i = GameController.gameController.players.Count - 1; i > -1; i--)
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

	#endregion

    public void Attack()
    {

		minionAudio.PlayRandomSound (minionAudio.attackSounds);
		if (_targetPlayer.gameObject.tag == "Player") 
		{
			if (!_targetPlayer.GetComponent<BasePlayerClass> ()._isDead)
				_targetPlayer.SendMessage ("AdjustCurHealth", -attackDamage);
		}
    }

    void Die()
    {
        Destroy(healthBar.gameObject);
        minionAudio.PlayRandomSound (minionAudio.deathSounds);
       // if (deathEffect != null)
		_agent.enabled = false;
		col.enabled = false;
        //_agent.Stop();
        _anim.SetBool("attacking", false);
        _anim.SetBool("moving", false);
        _anim.SetTrigger("die");
        dead = true;
    }   

    public void AdjustCurHealth(DamageInfo damageInfo)
    {
        if (dead)
            return;


        curHealth += damageInfo.damage; // positive values is HEAL, negative is DAMAGE
        if (curHealth > 0 && damageInfo.damage < 0) 
		{
			_anim.SetTrigger ("hit");
			minionAudio.PlayRandomSound(minionAudio.hurtSounds);
		}
        
        if (curHealth < 0)
            curHealth = 0;

        if (curHealth > maxHealth)
            curHealth = maxHealth;

        healthBar.SetCurrentHealth(curHealth);

        if (curHealth == 0 && !dead)
        {
            _anim.SetTrigger("die");
            dead = true;
        }

		if (curHealth <= 0)
		{
			Instantiate(deathEffect, transform.position, transform.rotation);
			Die();
		}
    }
}
