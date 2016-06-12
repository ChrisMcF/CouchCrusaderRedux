using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XInputDotNetPure;

public struct OldDamageInfo
{
	public Player player;
	public float damage;
	public float threatAmmount;
	public bool isDamageOverTimeAttack;
}

public class DamageInfo
{
    public Player player;
    public float damage;
    public float threatAmount;
    public bool isDamageOverTimeAttack;

    public DamageInfo(Player player, float damage, float threatAmount, bool isDamageOverTimeAttack)
    {
        this.player = player;
        this.damage = damage;
        this.threatAmount = threatAmount;
        this.isDamageOverTimeAttack = isDamageOverTimeAttack;
    }

    public DamageInfo()
    {

    }
}

[RequireComponent(typeof(AudioSource))]
public class BasePlayerClass : MonoBehaviour
{
	private Player thisPlayer;
	// Turn materials red on hit 
	public Renderer rend;
	public Material[] matsToTurnRed;
	public Color[] oldColors;
	private bool isRed = false;

    #region Attack Variables
	public enum ActionType
	{
		Melee,
		Ranged,
		Healing,
		Guard
	}
	;


	public float threatMultiplier = 1f;
	public int controllerIndex;

	private CharacterAudio characterAudio;
    #region HUD
	private GameObject playerHUD;
	private Slider healthBar;
	public Slider specialBar;
	private GameObject specialMeter;
	public Sprite lightAtt, heavyAtt, lightAttCD, heavyAttCD;
	public Image lightImage, heavyImage, lightCD, heavyCD;
	public Sprite portrait;
    #endregion
        
	// variables each action has
	[System.Serializable]
	public class Action
	{

		public ActionType actionType;

		public bool heal = false;

		public GameObject projectile;
		public GameObject healObject;

		public float healthPerSecond;
		public float damage;
		public float range;
		public float meleeArc;
        public float cooldownTime;
        [HideInInspector]
        public float cooldown;
    }



	// Variables for different actions;
	[System.Serializable]
	public class Actions
	{
		public Action light;
		public Action heavy;
		public Action special;

	}

	public Actions actions;

    #endregion

    #region Stat Variables

	// Stat variables
	[System.Serializable]
	public class Stats
	{
		public float curHealth, maxHealth;
		public float moveSpeed, rotationSpeed;
		public float defense, critChance, guardDefense;
		public float specialIncrement, specialAmount;
		public float specialTimer;
	}

	public Stats stats;
    #endregion

	public bool _isDead = false, specialReady = false, specialUsed = false, txtTrigger = false;
	public GameObject mageSpecialSpawn, paladinSpecialSpawn, fire, scrollText;
	public GameObject specialText;

	private Vector3 _warriorSpecPoint, _archerSpecPoint;
	private Animator _animator;

	public float _projectileForce = 100f;
	private Transform _projectileSpawner;

	[HideInInspector]
	public bool guarding;

	[HideInInspector]
	public GameObject enableOnDeath;

	private Resurrection res;

	public void Start ()
    {

		thisPlayer.playerClass = this;
		thisPlayer.playerObject = gameObject;
		res = GetComponent<Resurrection> ();
		enableOnDeath = transform.Find("EnableOnDeath").gameObject as GameObject;
		enableOnDeath.SetActive (false);
        actions.light.cooldown = actions.light.cooldownTime;
        actions.heavy.cooldown = actions.heavy.cooldownTime;
        actions.special.cooldown = actions.special.cooldownTime;

        oldColors = new Color[matsToTurnRed.Length];
		for (int i = 0; i<matsToTurnRed.Length; i++)
		{
			oldColors[i] = matsToTurnRed[i].color;
		}

		characterAudio = GetComponent<CharacterAudio> ();
		_animator = GetComponent<Animator> ();
		InitializePlayer ();
	
	}

	public void InitializePlayer()
	{
		playerHUD = GameObject.Find ("PlayerPanel " + controllerIndex.ToString ());
		healthBar = playerHUD.transform.FindChild ("Player_HealthBar").gameObject.GetComponent<Slider> ();
		healthBar.maxValue = stats.maxHealth;
		healthBar.value = stats.maxHealth;
		specialBar = playerHUD.transform.FindChild ("Player_SpecialBar").gameObject.GetComponent<Slider> ();
        lightImage = playerHUD.transform.FindChild ("Player_LightAttack").gameObject.GetComponent<Image> ();
		heavyImage = playerHUD.transform.FindChild ("Player_HeavyAttack").gameObject.GetComponent<Image> ();
		lightCD = playerHUD.transform.FindChild ("Player_LightAttackCD").gameObject.GetComponent<Image> ();
		heavyCD = playerHUD.transform.FindChild ("Player_HeavyAttackCD").gameObject.GetComponent<Image> ();

		_projectileSpawner = transform.FindChild ("ProjectileSpawner");

		playerHUD.transform.FindChild ("Player_Portrait").gameObject.GetComponent<Image> ().sprite = portrait;

		if (stats.curHealth == 0)
			stats.curHealth = stats.maxHealth;
		
		healthBar.value = stats.maxHealth;
		specialMeter = specialBar.transform.GetChild (1).GetChild (0).gameObject;
		specialMeter.SetActive (false);

		healthBar.transform.GetChild (1).GetChild (0).gameObject.SetActive (true);
		healthBar.fillRect.gameObject.SetActive (true);
	}

	public void Update ()
	{
		specialBar.value = stats.specialAmount / 100;
		
		if (specialBar.value >= 0.5) {
			specialReady = true;
		}

		if (specialBar.value >= 0.5 && !txtTrigger) {
			SpawnScrollText ();
			txtTrigger = true;
		}

		if (specialUsed && gameObject.name == "Paladin") {
			stats.specialAmount -= Time.deltaTime * 4;
			specialReady = false;
		} else if (specialUsed)
        {
			specialReady = false;
		}
		
		if (stats.specialAmount <= 0) {
			specialUsed = false;
			txtTrigger = false;
			
		}
		
		_warriorSpecPoint = new Vector3 (transform.position.x, transform.position.y + 1.5f, transform.position.z);
		_archerSpecPoint = new Vector3 (transform.position.x, transform.position.y + 1.5f, transform.position.z + 0.3f);
		
		healthBar.value = stats.curHealth;

		if (stats.specialAmount > 0) {
			specialMeter.SetActive (true);
		} else if (stats.specialAmount <= 0)
			specialMeter.SetActive (false);

	}

	// Called from Animation Events
	public void CallAttack (int index)
	{
		switch (index) {
		case 1:
			characterAudio.PlayRandomSound (characterAudio.lightAttackSounds);
			DoAction (actions.light);
			break;
		case 2:
			characterAudio.PlayRandomSound (characterAudio.heavyAttackSounds);
			DoAction (actions.heavy);
			break;
		case 3:
			characterAudio.PlayRandomSound (characterAudio.specialAttackSounds);
			SpecialAction (gameObject);
			break;
		case 4:
			break;
		}
	}


	public void AdjustCurHealth (float val)
	{
		res.InterruptResurrection ();
		if (_isDead)
			return;

		if (val < 0) 
		{
			StartCoroutine ("DamageVibrate", controllerIndex);
			StartCoroutine ("ChangeMatsOnHit", 0.3f);
		}


		if(guarding)
			stats.curHealth += val - (val * stats.guardDefense); // positive values is HEAL, negative is DAMAGE
		else
			stats.curHealth += val - (val * stats.defense);


		if (stats.curHealth < 0)
			stats.curHealth = 0;

		if (stats.curHealth > stats.maxHealth)
			stats.curHealth = stats.maxHealth;

		healthBar.value = stats.curHealth;

		if (stats.curHealth == 0) {
			enableOnDeath.SetActive(true);
			characterAudio.PlayRandomSound (characterAudio.deathSounds);
			healthBar.transform.GetChild (1).GetChild (0).gameObject.SetActive (false);
			healthBar.fillRect.gameObject.SetActive (false);
			_isDead = true;
			_animator.SetTrigger("Death");
            //Debug.Log("SetBool");
			GameController.gameController.RemovePlayerFromList (gameObject);
		}
		if (val < 0 && stats.curHealth > 0) {
            if (!_animator.GetCurrentAnimatorStateInfo(1).IsTag("InHitAnim")){
                characterAudio.PlayRandomSound(characterAudio.hurtSounds);
                _animator.SetTrigger("TakeDamage");
            }
		}

		if (gameObject.name == "Paladin" && !specialUsed) {
			stats.specialAmount += -(val * stats.specialIncrement);
		}
	}


	void SpecialAction (GameObject player)
	{
		switch (player.name) {
		case "Paladin":
			if (specialUsed)
            {
				Instantiate (paladinSpecialSpawn, transform.position, transform.rotation);
               
				fire = GameObject.Instantiate (actions.special.healObject, transform.position, transform.rotation) as GameObject;
                    fire.transform.parent = transform;
                    fire.GetComponent<PaladinSpecial>().SendMessage("Initialise", new float[] { actions.special.range, -actions.special.damage, specialBar.value, 3.5f });
                
                stats.specialAmount = 0f;
			}
			break;
		case "Warrior":
                GameObject.Instantiate(actions.special.projectile, _warriorSpecPoint, /*Quaternion.Euler(0, transform.eulerAngles.y, 0)*/transform.rotation);
			//lightning.transform.parent = transform;

			break;
		case "Mage":
			Instantiate (mageSpecialSpawn, transform.position, transform.rotation);
			Instantiate (actions.special.healObject, transform.position, transform.rotation);
			break;
		case "Archer":
			GameObject arrows = GameObject.Instantiate (actions.special.projectile, _archerSpecPoint, transform.rotation) as GameObject;
            Projectile[] projectiles = arrows.GetComponentsInChildren<Projectile>();
			DamageInfo damageInfo = new DamageInfo();
			damageInfo.player = thisPlayer;
			damageInfo.damage = -actions.special.damage;
			damageInfo.threatAmount = -damageInfo.damage * threatMultiplier;
			damageInfo.isDamageOverTimeAttack = false;
			foreach(Projectile arrow in projectiles)
            {
                arrow.Setup(damageInfo, gameObject, -actions.special.damage, 150f, true, true);
            }
		break;

		}

	}

	void PaladinSpecialDamage ()
	{
		if (specialUsed) {
			Collider[] targets = Physics.OverlapSphere (transform.position, actions.special.range);
			foreach (Collider target in targets) 
			{
				if (target.tag == "Enemy") 
					target.SendMessageUpwards ("AdjustCurHealth", -(actions.special.damage + (actions.special.damage * specialBar.value)), SendMessageOptions.RequireReceiver);
				else if(target.tag == "Boss")
				{
                    DamageInfo damageInfo = new DamageInfo(thisPlayer, -actions.special.damage, actions.special.damage * threatMultiplier, false);
                    target.SendMessageUpwards ("AdjustCurHealth",  damageInfo, SendMessageOptions.RequireReceiver);
				}
			}
		}
	}

	void DoAction (Action action)
	{
		switch (action.actionType) {
		case ActionType.Melee:
			MeleeAction (action);
			break;
		case ActionType.Ranged:
			RangedAction (action);
			break;
		case ActionType.Healing:
			break;
		case ActionType.Guard:
			GuardAction (action);
			break;

		}
	}



	// Code for melee attacks
	void MeleeAction (Action action)
	{
		// Get targets within range of player
		Collider[] targets = Physics.OverlapSphere (transform.position, action.range);

		foreach (Collider target in targets) {



			if (target.tag == "Enemy" && target.gameObject.layer == 16) {

				Vector3 dir = (target.transform.position - transform.position).normalized;
				if (Vector3.Dot (dir, transform.forward) > action.meleeArc) 
				{
                    // Send message to target to cause damage
                    DamageInfo damageInfo = new DamageInfo(thisPlayer, -action.damage, action.damage * threatMultiplier, false);
                    target.SendMessageUpwards ("AdjustCurHealth", damageInfo, SendMessageOptions.DontRequireReceiver);
					if (!specialUsed) {
						stats.specialAmount += action.damage * stats.specialIncrement;
					}
				}
			} else if(target.tag == "Boss" && target.gameObject.layer == 16)
			{
				Vector3 dir = (target.transform.position - transform.position).normalized;
				if (Vector3.Dot (dir, transform.forward) > action.meleeArc) 
				{
					// Send message to target to cause damage
					DamageInfo damageInfo = new DamageInfo(thisPlayer, -action.damage, action.damage * threatMultiplier, false);
					target.SendMessageUpwards ("AdjustCurHealth", damageInfo, SendMessageOptions.DontRequireReceiver);
					if (!specialUsed) {
						stats.specialAmount += action.damage * stats.specialIncrement;
					}
				}
			}
		}
	}

	void RangedAction (Action action)
	{
		// Instantiate a ranged attack's projectile; 
		// projectile code goes into it's own script
		Vector3 projectileSpawnPoint = transform.position;
		projectileSpawnPoint.y += 0.5f;
		//GameObject projectile = (GameObject)Instantiate (action.projectile, projectileSpawnPoint, transform.rotation);
		GameObject projectile = (GameObject)Instantiate (action.projectile, _projectileSpawner.position, _projectileSpawner.rotation);
        DamageInfo damageInfo = new DamageInfo(thisPlayer, -action.damage, action.damage * threatMultiplier, false);
        projectile.GetComponent<Projectile> ().Setup (damageInfo, gameObject, -action.damage, _projectileForce);

	}

	void GuardAction (Action action)
	
	{
		Debug.Log ("GUARD!!!");

		GameObject Guard = GameObject.Instantiate(action.projectile, transform.position, transform.rotation) as GameObject;
		Guard.transform.parent = transform;
		guarding = true;
		Debug.Log ("guard set to true");
//		GameObject spawn = GameObject.Instantiate (paladinSpecialSpawn, transform.position, transform.rotation) as GameObject;
//		
//		fire = GameObject.Instantiate (actions.special.healObject, transform.position, transform.rotation) as GameObject;
//		fire.GetComponent<PaladinSpecial>().SendMessage("Initialise", new float[] { actions.special.range, actions.special.damage, specialBar.value, 3.5f });
//		fire.transform.parent = transform;
//		stats.specialAmount = 0f;
	
	}

	public void EndGuardAction ()
	{
		guarding = false;
        _animator.SetInteger("AttIndex", 0);
    }

	public void SpawnScrollText ()
	{
		GameObject txt = (GameObject)Instantiate (specialText, transform.position, Quaternion.identity);
        txt.transform.parent = transform;
	}

	IEnumerator DamageVibrate (int playerIndex)
	{
		GamePad.SetVibration ((PlayerIndex)playerIndex - 1, 1f, 1f);
		yield return new WaitForSeconds (0.2f);
		GamePad.SetVibration ((PlayerIndex)playerIndex - 1, 0f, 0f);
	}
	public void TurnRed()
	{
		for (int i = 0; i<matsToTurnRed.Length; i++) 
		{
			matsToTurnRed[i].color = Color.red;
		}
	}
	public IEnumerator ChangeMatsOnHit(float hitTime)
	{
		if (!isRed) 
		{
			isRed = true;
			TurnRed ();
			yield return new WaitForSeconds (hitTime);
			ChangeMaterialColorsBack ();
			isRed = false;
		}
	}
	void ChangeMaterialColorsBack()
	{
		for (int i = 0; i<matsToTurnRed.Length; i++) 
		{
			matsToTurnRed[i].color = oldColors[i];
		}
	}

    public void DrainSpecialMeter()
    {
        stats.specialAmount = 0;
        specialBar.value = stats.specialAmount;
        specialUsed = true;
    }

	void OnDestroy()
	{
		ChangeMaterialColorsBack ();
	}


}


