using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class BaseBossClass : MonoBehaviour
{

	#region Public Variables
	
	[Range(0, 10000)]
	public float _curHealth, _maxHealth;
	[Range(10, 100)]
	public float _attack;
	[Range(0, 400)]
	public float _moveSpeed;
	[Range(0, 1)]
	public float _defense, _critChance;
	//public List <float> playerAggro;	
	public bool attackingNow = false;
	public bool dead = false;
	public float attackCooldown = 2f;
	public NavMeshAgent _agent;
	public Transform _targetPlayer = null;
	private int _playerIndex;
	public bool useDeadBoolInAnimator = false; 
	public Animator _animator;
	public Dictionary <string, float> threat = new Dictionary<string, float>();
	#endregion

	#region Private Variables

	public Slider healthBar;
	private BossAudio _bossAudio;
	private Transform _aggroPlayer;

	#endregion

    public void BossAwake()
    {
		//int numPlayers = 

    }

	public void BossStart()
	{
		_maxHealth = _maxHealth * GameController.gameController.players.Count;
		healthBar = GameObject.Find("Boss_HealthBar").GetComponent<Slider>();
		healthBar.maxValue = _maxHealth;
		healthBar.value = _maxHealth;
		_curHealth = _maxHealth;

		_bossAudio = GetComponent<BossAudio> ();
		_animator = GetComponent<Animator> ();
		_agent = GetComponent<NavMeshAgent>();
		InitializeThreatDictionary();
		
	}	

	public void AdjustCurHealth(DamageInfo damageInfo)
	{
		Debug.Log ("Attempting to Adjust bosses current health, recieved damage from: " + damageInfo.player.playerObject.name);
		float val = damageInfo.damage;
        if (val < 0) {//DAMAGE
			if (_bossAudio)
				_bossAudio.PlayRandomSoundEffectClip (_bossAudio.hurtSounds);
			AddThreat (damageInfo);
			_curHealth += damageInfo.damage;
			_animator.SetTrigger ("Damage");
			healthBar.value = _curHealth;
		} else {
			Debug.Log("Healing boss for: " + val + ", from: " + damageInfo.player.playerObject.name);
			_curHealth += val; // positive values is HEAL, negative is DAMAGE
		}
		if (_curHealth < 0)
			_curHealth = 0;
		
		if (_curHealth > _maxHealth)
			_curHealth = _maxHealth;
		
		if (_curHealth <= 0 && !dead)
		{
            _bossAudio.PlayRandomVoiceClip(_bossAudio.deathSounds);
			GameController.gameController.StartCoroutine("Victory");
			if(useDeadBoolInAnimator)
				_animator.SetBool("isDead", true);
			else
				_animator.SetTrigger("Dead");
			_agent.enabled = false;
			dead = true;
			healthBar.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
		}


	}

	public void InitializeThreatDictionary()
	{
        for (int i = GameController.gameController.players.Count - 1; i >= 0; i--)
        {
            Player player = GameController.gameController.players[i];
            threat.Add(player.playerObject.name, 0f);
		}
	}
	public Player GetPlayerWithHigestThreat()
	{
		string keyOfHighestValue = threat.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        for (int i = GameController.gameController.players.Count - 1; i >=0; i--)
        {
            Player player = GameController.gameController.players[i];
			if(player.playerObject.name == keyOfHighestValue)
			{
				return player;
			}
		}
		return new Player();
	}
	public void AddThreat (DamageInfo damageInfo)
	{
		threat [damageInfo.player.playerObject.name] += damageInfo.threatAmount;
	}

}
