using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Scylla : BaseBossClass
{

    public enum State { Idle, Attack, Healing, Dead };     //Different states of the boss.
    public State state;     //exposes "State" to inspector

    public enum Attacking { JumpSplash, JumpOut, SlimeFling, MinionSpawn, Melee };   //state handler for the attacking state.


    void Start ()
    {
       // players = GameObject.FindGameObjectsWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }
	
	
	void Update ()
    {
        Movement();
        attackCooldown -= Time.deltaTime;

        if (attackCooldown <= 0 && _animator.GetBool("IsWalking") == false)
            _animator.SetBool("Melee", true);
        else
            _animator.SetBool("Melee", false);
    }

    public void CallAttack(int attackType)
    {
        switch (attackType)
        {
            case 1:
                _targetPlayer.SendMessage("AdjustCurHealth", -_attack);
                break;
            case 2:
                break;
            case 3:
                //Attack((-_attack * 2f), 30, -1, Attacking.slimeFling);
                break;
            case 4:
                //Attack(0f, 0f, 0f, Attacking.minionSpawn);
                break;
        }
    }

    void Movement()
    {
        //GetPlayerAgro();
        float distanceToPlayer = Vector3.Distance(transform.position, _targetPlayer.position);
        if (_targetPlayer != null && distanceToPlayer > 4f)
        {
            _agent.speed = _moveSpeed;
            _agent.SetDestination(_targetPlayer.position);
            _animator.SetBool("IsWalking", true);
        }
        else if (_targetPlayer != null && distanceToPlayer < 4f)
        {
            _agent.speed = 0f;
            _animator.SetBool("IsWalking", false);
        }
    }

	/*
    void GetPlayerAgro()
    {
        float highestAgro = 0;
        int playerIndex = 0;

        for (int i = 0; i < 4; i++)
        {
            if (playerAggro[i] > highestAgro)
            {
                if (playerAggro[i] == 0)
                    return;

                    highestAgro = playerAggro[i];
                    playerIndex = i;
            }

        }
        _targetPlayer = GameController.gameController.players[playerIndex].playerObject.transform;
        //Debug.Log (_targetPlayer.name);

    }

  */
	}
