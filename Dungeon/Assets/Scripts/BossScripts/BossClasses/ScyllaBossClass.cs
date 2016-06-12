using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScyllaBossClass : BaseBossClass
{
	public GameObject slimeZone;
	public float slimeZoneDamage;



	#region State Machines

	//Sounds
	//private AudioSource audioSource;
	public enum State
	{
		idle,
		attack,
		heal
    }
	;     //Different states of the boss.
	public enum Attacking
	{
		jumpSplash,
		jumpOut,
		slimeFling,
		minionSpawn,
		melee }
	;   //state handler for the attacking state.

	#endregion

	#region public variables
	public float healSpeed = 50f;
	public State state;
	public float jumpDistance = 8f;
	public float jumpHeight = 7f;
	public float preJumpRotationSpeed = 0.1f;
	[HideInInspector]
	public bool
		phase1Complete = false, phase2Complete = false, phase3Complete = false, healingFinished = false, healingStart = false, jumpTriggered = false, jumpOutTriggered = false;
	public bool levelStart = false;
	public float attackTimer = 5f;
	public float projectileTimer = 12f, spawnTimer = 27f;
	public GameObject _furthestPlayer, _slimeProjectile, _slimeSpawn, splash, splashWarn;
	public float speed;
	public int _jumpNum = 0;
	public GameObject slimeTrail;
	public TrailCollision trailCollision;
	public float jumpTime;
	public Door[] alcove = new Door[3];
	public List <Player> dmgPlayer = new List<Player> ();
	[Range(0, 100)]
	public float inFightChatPercentage = 20f;
	//public int numPlayers;

	#endregion

	#region Private Variables

	private float _nearestDistance = 0.2f, distance, rotSpeed = 10f;
	private Vector3 _flingPoint, _spawn1, _spawn2, _spawn3, _spawn4, _spawn5, _spawn6, _spawn7, _jumpSpot;
	private Quaternion _wantedFarPlayRot, _curFarPlayRot, wantedTargRot, curTargRot, spawnRot;
	private Vector3[] jumpPoints = new Vector3[3];
	private Vector3[] jumpInPts = new Vector3[3];
	private ScyllaAudio scyllaAudio;
	private bool forceSlimeZoneOff = false;
	#endregion

	#region IEnumerators

	IEnumerator FlingWait ()		//Waits for 2 seconds before returning Scylla to look at her target player, rather than this happening instantly.
	{
		yield return new WaitForSeconds (2f);
		transform.rotation = wantedTargRot;
	}

	IEnumerator StartTrail ()		
	{
		yield return new WaitForSeconds (jumpTime);
		forceSlimeZoneOff = false;
		slimeTrail.SetActive (true); //enable the slime trail
		trailCollision.StartNewTrail ();
		LandFromJump ();
	}
	
	IEnumerator StopTrail ()
	{
        if (trailCollision.isActiveAndEnabled)
        {
            trailCollision.StartCoroutine("ClearTrail");
            int numberOfFrames = 6;
            while (numberOfFrames > 0)
            {
                numberOfFrames -= 1;
                yield return null;
            }
            slimeTrail.SetActive(false);
        }
		
	}

	IEnumerator WaitForThenLandFromJump ()		//Makes the land from jump function not begin until Scylla has actually landed.
	{
		yield return new WaitForSeconds (jumpTime);
		LandFromJump ();
	}

	public IEnumerator BeginBossEncounter ()
	{
		Debug.Log ("Begin bos encounter has been called, this should only be called once at the start of the encounter");
		float wait = scyllaAudio.PlayRandomVoiceClip (scyllaAudio.encounterStart);
		yield return new WaitForSeconds (wait);
		levelStart = true;
	}
	
	IEnumerator SplashSpawn ()		//Spawns the splash particle effect
	{
		yield return new WaitForSeconds (jumpTime);
		Instantiate (splash, transform.position, transform.rotation);
		

        for (int i = GameController.gameController.players.Count - 1; i >=0; i--)
        {
            Player p = GameController.gameController.players[i];

            if (!dmgPlayer.Contains(p))
                p.playerObject.SendMessage("AdjustCurHealth", -30f, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	IEnumerator StartHealSound ()		//Plays a repeated sound during the healing phase.
	{
		yield return new WaitForSeconds (scyllaAudio.landSound.length);	//Wait for the land sound to finish
		scyllaAudio.StartCoroutine ("RepeatSoundEffect", scyllaAudio.healSound);
		yield return new WaitForSeconds (20f);
	}

	IEnumerator RotateToFaceDirectionOverTime (Vector3 eulerAngleDirection, float time)		//Makes Scylla's rotation happen over time.
	{
		float timer = time;
		Vector3 startAngle = transform.eulerAngles;
		while (timer >0) {
			Vector3 myAngle = Vector3.Lerp (startAngle, eulerAngleDirection, time);
			transform.Rotate (myAngle);
			timer -= Time.deltaTime;
			yield return null;
		}
	}

	IEnumerator SplashWarning ()
	{
		forceSlimeZoneOff = true;
		_agent.enabled = false;
		scyllaAudio.PlayRandomVoiceClip (scyllaAudio.beforeJumpIn);
		//GameObject warnSpawn = GameObject.Instantiate (splashWarn, Vector3.zero, Quaternion.identity) as GameObject;
		splashWarn.GetComponentInChildren<SplashWarning> ().StartCoroutine ("WarnPlayersAboutSplash", 3f+jumpTime);
		yield return null;
	}

	IEnumerator JumpInWait ()
	{
		yield return new WaitForSeconds (3f);

		StartCoroutine ("StopTrail");
		
		if (transform.position == _jumpSpot)
			yield return null;
		else {
			if (!jumpTriggered) {
				_agent.enabled = false;
				GetComponentInChildren<MeshCollider> ().enabled = false;
				
				Vector3 jumpTo;
				Vector3 middle;
				Vector3 jump = new Vector3 (0, 5, 0);
				
				jumpTo = _jumpSpot;
				middle = transform.position - jumpTo / 2f;
				middle = middle + jump;
				
				jumpInPts [0] = transform.position;
				jumpInPts [1] = middle;
				jumpInPts [2] = _jumpSpot;
				
				iTween.MoveTo (gameObject, iTween.Hash ("path", jumpInPts, "time", jumpTime, "easetype", iTween.EaseType.linear));
				
				alcove [_jumpNum].StartCoroutine ("OpenDoor");
				
				_jumpNum++;
				StartCoroutine ("WaitForThenLandFromJump");
				StartCoroutine ("SplashSpawn");
				healingStart = true;
				
				jumpTriggered = true;
			}
		}
		
		StartCoroutine ("StartHealSound");
	}

	#endregion

	#region Unity Functions

	void Awake ()
	{

		BossAwake ();
	}
	
	void Start ()		//Initialisation of all variables takes place here.
	{

		healSpeed = healSpeed * GameController.gameController.players.Count;

        scyllaAudio = GetComponent<ScyllaAudio> ();
		_jumpSpot = new Vector3 (2.12f, 0f, 0.01f);
		jumpPoints [0] = _jumpSpot;
		_agent = GetComponent<NavMeshAgent> ();
		state = State.idle;

        if (_curHealth != _maxHealth)
			_curHealth = _maxHealth;
		
		_spawn1 = new Vector3 (11.93f, 0.51f, 5.11f);
		_spawn2 = new Vector3 (-7.4f, 0.51f, 5.16f);
		_spawn3 = new Vector3 (-2.27f, 0.51f, -9.88f);
		_spawn4 = new Vector3 (11.61f, 0.51f, -4.46f);
		_spawn5 = new Vector3 (9.03f, 0.51f, 1.6f);
		_spawn6 = new Vector3 (-9.92f, 0.51f, -6.17f);
		_spawn7 = new Vector3 (9.54f, 0.51f, 11.8f);
		
		BossStart ();
		
		//GetPlayerAgro ();

		GameObject[] temp = GameObject.FindGameObjectsWithTag ("Door");

		for (int i = 0; i < alcove.Length; i++) 
		{
			alcove [i] = temp [i].GetComponent<Door> ();
		}
		/*
		playerAggro = new List<float> ();//(new float [GameController.gameController.players.Count]);
		//for (int i = 0; i < GameController.gameController.players.Count - 1; i++)

        for (int i = 0; i < GameController.gameController.players.Count; i++)
        {
            playerAggro.Add(0);
        }
		*/
	
	}
	
	void Update ()
	{
		StateHandler ();



        if (_furthestPlayer == null)
        { _furthestPlayer = GetFurthestPlayer(); }

        //GetPlayerAgro ();
        Player p = GetPlayerWithHigestThreat();

        if (p.playerObject == null)
            return;


        _targetPlayer = p.playerObject.transform;

		Vector3 stopped = new Vector3 (0, 0, 0);
		
		if (_agent.velocity != stopped) 
		{
			_animator.SetBool ("IsWalking", true);
			SlimeZoneOff();

		}
			else 
		{
			_animator.SetBool ("IsWalking", false);
			if(phase1Complete)
				SlimeZoneOn ();

		}
		if (_curHealth > _maxHealth)
			_curHealth = _maxHealth;
		
		if (attackingNow) {
			attackCooldown -= Time.deltaTime;
			attackingNow = false;
		}
		
		if (_curHealth <= (_maxHealth * 0.75f) && _curHealth >= (_maxHealth * 0.5f) && !phase1Complete) {
			phase1Complete = true;
			JumpSplash ();
		}
		
		if (phase1Complete && _curHealth <= (_maxHealth * 0.5f) && _curHealth >= (_maxHealth * 0.25f) && !phase2Complete) {
			phase2Complete = true;
			JumpSplash ();
		}
		
		if (phase1Complete && phase2Complete && _curHealth <= (_maxHealth * 0.25f) && _curHealth >= (_maxHealth * 0) && !phase3Complete) {
			phase3Complete = true;
			JumpSplash ();
		}

        _furthestPlayer = GetFurthestPlayer();
		
		_flingPoint = new Vector3 (transform.position.x, transform.position.y + 1.5f, transform.position.z + 0.3f);



		_wantedFarPlayRot = Quaternion.LookRotation ((_furthestPlayer.transform.position - transform.position).normalized);
		_curFarPlayRot = Quaternion.Euler (transform.rotation.eulerAngles);
		float _yVal = Mathf.LerpAngle (_curFarPlayRot.y, _wantedFarPlayRot.y, rotSpeed * Time.deltaTime);
		_curFarPlayRot = Quaternion.Euler (_curFarPlayRot.x, _yVal, _curFarPlayRot.z);

		wantedTargRot = Quaternion.LookRotation ((_targetPlayer.transform.position - transform.position).normalized);
		curTargRot = Quaternion.Euler (transform.rotation.eulerAngles);
		float yVal = Mathf.LerpAngle (curTargRot.y, wantedTargRot.y, rotSpeed * Time.deltaTime);
		curTargRot = Quaternion.Euler (curTargRot.x, yVal, curTargRot.z);

		if (healingFinished)
			JumpOut ();


		if (healingStart) 
		{
			state = State.heal;
			SlimeZoneOff ();
		}
		if (dead) 
		{
			//Debug.Log("Scylla Is Dead!");
			forceSlimeZoneOff = true;
			SlimeZoneOff ();
			GetComponentInChildren<MeshCollider> ().enabled = false;
			//GetComponent<SphereCollider> ().enabled = false;
		}
	}

	#endregion


    private GameObject  GetFurthestPlayer()
    {
        _nearestDistance = 0f;
        for (int i = GameController.gameController.players.Count - 1; i >= 0; i--)
        {
            Player p = GameController.gameController.players[i];

            distance = Vector3.Distance(transform.position,  p.playerObject.transform.position);
            if (distance > _nearestDistance)
            {
                _nearestDistance = distance;
                _furthestPlayer = p.playerObject;
            }
        }

        Debug.Log("Furthest Player: " + _furthestPlayer.name);
        return _furthestPlayer;
    }

	#region Attack Functions
	public void SlimeZoneOn()
	{
		if(!forceSlimeZoneOff)
			slimeZone.SetActive(true);

	}
	public void SlimeZoneOff()
	{
		slimeZone.SetActive(false);
	}

	public void CallAttack (int attackType)		//Calls the different attacks, is triggered by the animations on Scylla.
	{

		scyllaAudio.PercentageChanceToPlayRandomVoiceClip (scyllaAudio.inFightSounds, inFightChatPercentage);
		switch (attackType) {
		case 1:

			Attack (_attack, 5f, 0.6f, Attacking.melee);
			if(!scyllaAudio.GetSoundEffectAudioSource().isPlaying)
				scyllaAudio.PlayRandomSoundEffectClip (scyllaAudio.meleeAttackSounds);
			break;
		case 2:
			break;
		case 3:
			Attack ((_attack * 2f), 30, -1, Attacking.slimeFling); 
			if(!scyllaAudio.GetSoundEffectAudioSource().isPlaying)
				scyllaAudio.PlayRandomSoundEffectClip (scyllaAudio.rangedAttackSounds);
			break;
		case 4:
			Attack (0f, 0f, 0f, Attacking.minionSpawn);
			if(!scyllaAudio.GetSoundEffectAudioSource().isPlaying)
				scyllaAudio.PlayRandomSoundEffectClip (scyllaAudio.rangedAttackSounds);
			break;
		}
	}
	
	void Attack (float damage, float range, float meleeArc, Attacking attack)		
	{
		if (damage > 0)
			damage = -damage;
		
		switch (attack) {
		case Attacking.melee:
			if (!attackingNow)
                {
                    for(int i = GameController.gameController.players.Count - 1; i >= 0; i--)
                    {
                        Player player = GameController.gameController.players[i];
                        if (Vector3.Distance(transform.position, player.playerObject.transform.position) < range)
                        {
                            Vector3 dir = (player.playerObject.transform.position - transform.position).normalized;
                            if (Vector3.Dot(dir, transform.forward) > meleeArc)
                            {
                                player.playerObject.SendMessage("AdjustCurHealth", damage, SendMessageOptions.RequireReceiver);
                                attackingNow = true;
                            }
                        }
                    }

				
			}
			break;
		case Attacking.jumpSplash:
			JumpSplash ();
			break;
		case Attacking.slimeFling:
			SlimeFling ();
			break;
		case Attacking.minionSpawn:
			SpawnMinion ();
			break;
		}
		
	}
	
	void AttackingState ()		//Scylla's behaviour when the state is set to Attack
	{
		jumpOutTriggered = false;
		jumpTriggered = false;
		
		if (!attackingNow && !_targetPlayer.GetComponent<BasePlayerClass> ()._isDead) {
			attackTimer -= Time.deltaTime;
			
			if (attackTimer <= 0 && Vector3.Distance (transform.position, _targetPlayer.transform.position) < 5f) {
				_animator.SetTrigger ("Melee");
				attackTimer = 5f;
			}
			
			projectileTimer -= Time.deltaTime;
			
			if (projectileTimer <= 0) {
				_animator.SetTrigger ("Fling");
				projectileTimer = 12f;
			}
			
			spawnTimer -= Time.deltaTime;
			
			if (spawnTimer <= 0) {
				_animator.SetTrigger ("Minion");
				spawnTimer = 27f;
			}
		}
		
		
		Movement ();
		//GetPlayerAgro ();
		
	}

	#endregion

	#region Other State Functions

	void StateHandler ()		//Handles the different states at runtime.
	{
		if (levelStart) {
			JumpOut ();
			levelStart = false;
		}
		//Debug.Log (state.ToString ());
		
		if (state == State.attack) {
			//Debug.Log ("Attack State called");
			AttackingState ();
		} else if (state == State.heal)
			HealingState ();
		
	}
	
	void IdleStart ()		//Describes the Idle state
	{
		if (levelStart)
			StartCoroutine ("IdleWait");
		
	}
	
	void HealingState ()		//Describes the Healing state
	{
		//Debug.Log ("this is the heal state, healingFinished = " + healingFinished ); 

		if (!healingFinished) {
			_curHealth = (_curHealth + (Time.deltaTime * healSpeed));
			healthBar.value = _curHealth;
		} else {
			scyllaAudio.StopCoroutine ("RepeatSoundEffect");
			JumpOut ();
			state = State.attack;
			healingFinished = false;
		}
	}

	#endregion

	#region Scylla Jump Functions

	void LandFromJump ()
	{
		scyllaAudio.PlaySoundEffectClip (scyllaAudio.landSound);
		Camera.main.GetComponent<CameraShake> ().StartShake (scyllaAudio.landSound.length / 3);
	}
	
	
	
	void JumpOut ()		//Picks one of four points at random and then jumps Scylla to that point.
	{
		scyllaAudio.PlayRandomVoiceClip (scyllaAudio.beforeJumpOut);
		if (!jumpOutTriggered) {
			int num = (int)Random.Range (1, 4);

			//Vector3 jump = new Vector3 (0, 5, 0);
			
			switch (num) {
			case 1:
				

				//StartCoroutine (RotateToFaceDirectionOverTime (new Vector3 (0, 0, 0), preJumpRotationSpeed));
				jumpPoints [1] = transform.forward + new Vector3 (0, jumpHeight, jumpDistance / 2);
				jumpPoints [2] = transform.forward + new Vector3 (0, 0, jumpDistance);
				break;
			case 2:
				//StartCoroutine (RotateToFaceDirectionOverTime (new Vector3 (0, 90, 0), preJumpRotationSpeed));
				jumpPoints [1] = transform.forward + new Vector3 (jumpDistance / 2, jumpHeight, 0);
				jumpPoints [2] = transform.forward + new Vector3 (jumpDistance, 0, 0);
				break;
			case 3:
				//StartCoroutine (RotateToFaceDirectionOverTime (new Vector3 (0, 0, 0), preJumpRotationSpeed));
				jumpPoints [1] = transform.forward + new Vector3 (0, jumpHeight, -jumpDistance / 2);
				jumpPoints [2] = transform.forward + new Vector3 (0, 0, -jumpDistance);
				break;
			case 4:
				//StartCoroutine (RotateToFaceDirectionOverTime (new Vector3 (0, 180, 0), preJumpRotationSpeed));
				jumpPoints [1] = transform.forward + new Vector3 (-jumpDistance / 2, jumpHeight, 0);
				jumpPoints [2] = transform.forward + new Vector3 (-jumpDistance, 0, 0);
				break;

            }
            jumpOutTriggered = true;

        }
		
		iTween.MoveTo (gameObject, iTween.Hash ("path", jumpPoints, "time", jumpTime, "easetype", iTween.EaseType.linear));
		
		state = State.attack;
		_agent.enabled = true;
		//GetComponent<Collider> ().enabled = true;
		GetComponentInChildren<MeshCollider> ().enabled = true;
		//GetComponent<SphereCollider> ().enabled = true;
		StartCoroutine ("StartTrail");
	}	
	
	void JumpSplash ()		//Handles when Scylla jumps into the bath triggering the healing state.
	{

		StartCoroutine ("SplashWarning");

		StartCoroutine ("JumpInWait");

	}

	#endregion

	#region Projectile Functions

	void SlimeFling ()
	{

		Quaternion currentFarRot = _wantedFarPlayRot;
		
		transform.rotation = currentFarRot;
		
		if (_furthestPlayer != null && transform.rotation == currentFarRot) {
			Instantiate (_slimeProjectile, _flingPoint, transform.rotation);
			scyllaAudio.PlayRandomSoundEffectClip (scyllaAudio.rangedAttackSounds);
			
		}
		
		StartCoroutine ("FlingWait");
		
	}
	
	void SpawnMinion ()		//Picks one of 7 spots around the room and fligs a projectile at that spot, which spawns a minion at that spot.
	{
		int num = Random.Range (1, 7);
		
		Vector3 fling = new Vector3 (_flingPoint.x, (_flingPoint.y + 2f), _flingPoint.z);
		
		switch (num) {
		case 1:
			spawnRot = Quaternion.LookRotation ((_spawn1 - transform.position).normalized);
			Instantiate (_slimeSpawn, fling, spawnRot);
			
			break;
		case 2:
			spawnRot = Quaternion.LookRotation ((_spawn2 - transform.position).normalized);
			Instantiate (_slimeSpawn, fling, spawnRot);
			
			break;
		case 3:
			spawnRot = Quaternion.LookRotation ((_spawn3 - transform.position).normalized);
			Instantiate (_slimeSpawn, fling, spawnRot);
			
			break;
		case 4:
			spawnRot = Quaternion.LookRotation ((_spawn4 - transform.position).normalized);
			Instantiate (_slimeSpawn, fling, spawnRot);
			
			break;
		case 5:
			spawnRot = Quaternion.LookRotation ((_spawn5 - transform.position).normalized);
			Instantiate (_slimeSpawn, fling, spawnRot);
			
			break;
		case 6:
			spawnRot = Quaternion.LookRotation ((_spawn6 - transform.position).normalized);
			Instantiate (_slimeSpawn, fling, spawnRot);
			
			break;
		case 7:
			spawnRot = Quaternion.LookRotation ((_spawn7 - transform.position).normalized);
			Instantiate (_slimeSpawn, fling, spawnRot);
			
			break;
		}		
	}

	#endregion

	#region Threat Functions

	//MOVED THIS INTO BOSS BASE CLASS SO THAT IT ACTUALLY WORKS!!!!


	//WTF??
	/*
	public void GetPlayerThreat (float threat, GameObject player)		//Gathers the threat from the players as they attack Scylla.
	{
		for (int index = 0; index < GameController.gameController.players.Count; index ++)
		{
			foreach (Player playerThreat in GameController.gameController.players)
			{
				if (playerThreat.playerObject.name == player.name)
				{
					playerAggro[index] += threat; 
				}
				
				index ++;
			}
		}
	}

	void GetPlayerAgro ()		//Function selects the player with the highest threat as the target.
	{
		float highestAgro = 0;
		int playerIndex = 0;
		if (GameController.gameController.players.Count > 0) 
		{
			for (int i = GameController.gameController.players.Count - 1; i >0; i--) 
			{
				if (playerAggro [i] > highestAgro) 
				{
					if (playerAggro [i] == 0)
						break;
					highestAgro = playerAggro [i];
					playerIndex = i;
				}
			}
			_targetPlayer = GameController.gameController.players[playerIndex].playerObject.transform;
			//Debug.Log (_targetPlayer.name);
		}
	}
	*/

	#endregion

	void Movement ()
	{
		//_targetPlayer = GetPlayerWithHigestThreat().playerObject.transform;
		//GetPlayerAgro ();
		if (_targetPlayer != null && Vector3.Distance (this.transform.position, _targetPlayer.position) > 1f) {
			//Debug.Log ("!");
			_agent.speed = _moveSpeed;
			if(_agent.isActiveAndEnabled)
				_agent.SetDestination (_targetPlayer.position);
			
		} else {
			//return;//Debug.Log ("No Target");
		}
	}
	
	void TestMelee ()
	{
		if (attackCooldown <= 0) {
			attackingNow = false;
			attackCooldown = 2f;
		}
	}
	/*void CheckNumberOfPlayers ()
	{
		for (int i =0; i< players.Count; i++) {
			if (players [i] != null)
				numberOfPlayers++;
		}
		
	}*/

	void OnTriggerEnter (Collider col)
	{
		if (col.tag == "Bath") {
			state = State.heal;

		}
	}

	void StopHealSound ()
	{
		scyllaAudio.StopCoroutine ("RepeatSound");
	}
}

