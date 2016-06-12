using UnityEngine;
using System.Collections;

public class Spewnicorn : BaseBossClass
{

    public int currentPhase = 0;

    public int chargeSpeed = 20;

    private ParticleSystem spewEmitter;

    private bool encounterStarted = false;

	void Awake()
	{
		BossAwake ();
	}

    void Start ()
    {
        BossStart();
        _agent = GetComponent<NavMeshAgent>();
        spewEmitter = GetComponentInChildren<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!encounterStarted) return;

        attackCooldown -= Time.deltaTime;

        if (!attackingNow)
        {
            Movement();
        }

        // THIS CODE NEEDS TO BE FIXED!!

        //if (currentPhase == 0 && _curHealth <= (_maxHealth * 0.75f))
        //{
        //    Debug.Log("PhaseChanged");
        //    ChargeAttack();
        //    currentPhase++;
        //    return;
        //}
        //else if (currentPhase == 1 && _curHealth <= (_maxHealth * 0.5f))
        //{
        //    ChargeAttack();
        //    currentPhase++;
        //    return;
        //}
        //else if (currentPhase == 2 && _curHealth <= (_maxHealth * 0.25f))
        //{
        //    ChargeAttack();
        //    currentPhase++;
        //    return;
        //}


        if (!attackingNow && attackCooldown <= 0)
        {
            float distanceToTarget = Vector3.Distance(transform.position, _targetPlayer.position);
            if (distanceToTarget <= 10f)
            {
                attackCooldown = 3f;
                StartCoroutine("SpewBurp");
            }
        }
           
	}

    IEnumerator SpewBurp()
    {
        attackingNow = true;
        spewEmitter.Play();
        yield return new WaitForSeconds(.5f);
        spewEmitter.Stop();
        attackingNow = false;
    }

    void ChargeAttack()
    {
        attackingNow = true;
        int randomPlayer = Random.Range(0, GameController.gameController.players.Count - 1);
        _targetPlayer = GameController.gameController.players[randomPlayer].playerObject.transform;

        _agent.speed = chargeSpeed;
        _agent.SetDestination(_targetPlayer.position);
        _animator.SetBool("Charge", true);
    }

    void Movement()
    {
        //GetPlayerAgro();
		_targetPlayer = GetPlayerWithHigestThreat ().playerObject.transform;
        

        float distance = Vector3.Distance(transform.position, _targetPlayer.position);


        if (_targetPlayer != null && distance < 12)
        {
            Vector3 targetDir = _targetPlayer.position - transform.position;
            float step = 10f * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
            transform.rotation = Quaternion.LookRotation(newDir);
        }


        if (_targetPlayer != null && Vector3.Distance(transform.position, _targetPlayer.position) > 3f)
        {
            _agent.speed = _moveSpeed;
			if(_agent.isActiveAndEnabled)
	            _agent.SetDestination(_targetPlayer.position);
            _animator.SetBool("Walking", true);
        }
        else
        {            
            _agent.speed = 0;
            _agent.SetDestination(transform.position);
            _animator.SetBool("Walking", false);
        }
    }
	/*
    void GetPlayerAgro()
    {
        float highestAgro = 0;
        int playerIndex = 0;

        for (int i = 0; i < GameController.gameController.players.Count; i++)
        {
            if (playerAggro[i] > highestAgro)
            {
                if (playerAggro[i] == 0)
                    break;

                highestAgro = playerAggro[i];
                playerIndex = i;
            }

        }

        _targetPlayer = GameController.gameController.players[playerIndex].playerObject.transform;
    }
    */

    public void StartEncounter()
    {
        encounterStarted = true;
    }
}
