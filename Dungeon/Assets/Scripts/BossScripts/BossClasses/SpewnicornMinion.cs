using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AudioSource))]
public class SpewnicornMinion : MonoBehaviour
{

	[System.Serializable]
	public struct MinionStats
	{
		public float curHealth;
		public float maxHealth;
		public float attackDamage;
		public float attackCooldownTime;
		public float moveSpeed;
	}
	public MinionStats stats;


	private float attackCooldown;
	private bool isDead;

    #region Components

	private NavMeshAgent _agent;
	private Animator _anim;
	private HealthBarBehaviour _healthBar;
	private Collider _collider;
    #endregion

	private GameObject _targetPlayer;
	private BasePlayerClass _targetPlayerBaseClass;



	void Start ()
	{
		_agent = GetComponent<NavMeshAgent> ();
		_anim = GetComponent<Animator> ();
		_healthBar = GetComponentInChildren<HealthBarBehaviour> ();
		_collider = GetComponent<Collider> ();
		_targetPlayer = PickRandomPlayer ();
		_targetPlayerBaseClass = _targetPlayer.GetComponent<BasePlayerClass> ();

		attackCooldown = stats.attackCooldownTime;
		_agent.speed = stats.moveSpeed;
		_healthBar.InitializeMaxAndCurrentHealth (stats.maxHealth);
	}

	void Update ()
	{
		if (isDead)
			return;

		if (attackCooldown > 0)
			attackCooldown -= Time.deltaTime;

		float distanceToPlayer = Vector3.Distance (transform.position, _targetPlayer.transform.position);
		if (distanceToPlayer > 3f) {
			Movement ();
		} else if (distanceToPlayer < 3f) {
			Attack ();
		}
	}

	void Movement ()
	{
		if (_agent.speed == 0)
			_agent.speed = stats.moveSpeed;

		_anim.SetBool ("moving", true);
		_anim.SetBool ("attacking", false);

		_agent.SetDestination (_targetPlayer.transform.position);
	}

	void Attack ()
	{
		if (_agent.speed > 0)
			_agent.speed = 0;

		_anim.SetBool ("moving", false);
		_anim.SetBool ("attacking", true);
	}

	public void DamagePlayer ()
	{
		Debug.Log ("Damage Player");
		_targetPlayerBaseClass.SendMessage ("AdjustCurHealth", -stats.attackDamage);
	}

	GameObject PickRandomPlayer ()
	{
		int randomPlayerNum = Random.Range (0, GameController.gameController.players.Count);
		return GameController.gameController.players [randomPlayerNum].playerObject;
	}

	public void AdjustCurHealth (DamageInfo damageInfo)
	{
		float val = damageInfo.damage;

		stats.curHealth += val; // positive values is HEAL, negative is DAMAGE
		if (stats.curHealth > 0 && val < 0) {
			_anim.SetTrigger ("hit");
			//_audio.PlayRandomSound(minionAudio.hurtSounds);
		}

		if (stats.curHealth < 0)
			stats.curHealth = 0;

		if (stats.curHealth > stats.maxHealth)
			stats.curHealth = stats.maxHealth;

		_healthBar.SetCurrentHealth (stats.curHealth);

		if (stats.curHealth == 0 && !isDead) {
			_collider.enabled = false;
			_agent.Stop ();
			Destroy (_healthBar.gameObject);
			_anim.SetTrigger ("die");
			isDead = true;
		}
	}


}
