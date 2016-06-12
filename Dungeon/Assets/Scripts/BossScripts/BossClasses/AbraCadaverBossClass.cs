using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AbraCadaverBossClass : BaseBossClass 
{
	private AbraCadaverAudio abraCadaverAudio;
	private AbraMagicOrb abraMagicOrb;
	private AbraFirestorm abraFireStorm;
	public GameObject[] enableOnCast;
	public float meleeRange;
	private GameObject closestPlayerObject;
	private bool mechanicOver = true;
	private int lastMechanic = 0;
	public float fireAttackTime = 15f;
	public float orbWaitTime = 2f;
	private bool startedBossEncounter = false; 
	public Transform[] teleportationLocations;
	private Transform lastLocation;
	public float teleportTime =1f;
	public Transform onStageLocation;
	public float raiseMausoleumTime = 3f;
	public float waitAfterRaise = 5f;
	public GameObject mausoleum;
	public Transform[] mausoleumSpawnPoints;
	public float meleeAttackRangeMultiplier = 2f;
	public float meleeAttackWait = 2f; 
	public Vector2 explosionOffset;// x = forward offset, y = up offset;
	public GameObject explosionObject;
	private CameraShake cameraShake;
	public float raiseStageTime= 3f;
	public GameObject stage;
	public List<MausoleumBehaviour> mausoleumList; //= new List<MausoleumBehaviour>();
	private bool waitingForMausoleums = false;
	private bool started = false;
	public float explosionDamageRadius = 2f;
	public float explosionDamage = 10f;
	private SceneMusic sceneMusic;
	public GameObject bossHealthBar;
	public Collider abraCollider;
	private bool isDead = false;
	private bool raisedAll = true;

	public GameObject meleeSphere, teleportInEffect, teleportOutEffect;

	void Awake()
	{
		BossAwake ();
	}
	// Use this for initialization
	void Start () 
	{

		abraCollider = transform.FindChild ("Collider").GetComponent<Collider> ();
		mausoleumList = new List<MausoleumBehaviour>();
		GameObject levelSpecificObject = GameObject.FindGameObjectWithTag ("LevelSpecific") as GameObject;
		sceneMusic = levelSpecificObject.GetComponent<SceneMusic> ();
		_agent.enabled = false;
		cameraShake = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShake> ();
		abraFireStorm = GetComponent<AbraFirestorm> ();
		abraMagicOrb = GetComponent<AbraMagicOrb> ();
		abraCadaverAudio = GetComponent<AbraCadaverAudio> ();
		_agent = GetComponent<NavMeshAgent> ();
		BossStart ();
		bossHealthBar.SetActive (false);
		abraCollider.enabled = false;

		if (_curHealth != _maxHealth)
			_curHealth = _maxHealth;
		//StartEncounter ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!isDead)
		{
			if (waitingForMausoleums && mausoleumList.Count == 0) {
				mechanicOver = true;
				waitingForMausoleums = false;
			}
			if (startedBossEncounter && mechanicOver && GameController.gameController.players.Count != 0)
				PerformRandomMechanic ();
		}
	}

	void PerformRandomMechanic()
	{
		mechanicOver = false;
		//choose a random number to represent a mechanic.
		int numberOfMechanics = 4;
		int randomNumber = (int)Random.Range(0, numberOfMechanics);
		while (randomNumber == lastMechanic) 
		{
			randomNumber = Random.Range (0, numberOfMechanics);
		}
		Debug.Log ("Selected Mechanic: " + randomNumber); 
		//
		switch (randomNumber) 
		{
		case 0:

			StartCoroutine(ChannelFireAttack(fireAttackTime));
			StartCoroutine("WaitForSpecifiedPeriodBeforePickingANewMechanic", fireAttackTime+teleportTime);
			break;
		case 1:
			_animator.SetInteger("DanceNum", 2);
			// CAST RANDOM ORB
			CastARandomOrbSpell();
			StartCoroutine("WaitForSpecifiedPeriodBeforePickingANewMechanic", orbWaitTime*2+teleportTime);

			break;
		case 2:
			_animator.SetInteger("DanceNum", 1);
			// Teleport to Stage and then raise Mausoleums
			if(raisedAll)
			{
				StartCoroutine("RaiseSingleRandomMausoleum");
			}
			else
			{
				StartCoroutine(TeleportTo(onStageLocation, teleportTime));
				ChannelRaiseAllMausoleums(raiseMausoleumTime);
			}
			raisedAll = !raisedAll;
			break;
		case 3:
			_animator.SetInteger("DanceNum", 3);
			RandomMeleeAttack();
			StartCoroutine("WaitForSpecifiedPeriodBeforePickingANewMechanic", meleeAttackWait+teleportTime);

		//	CastOrbAttackOnRandomPlayer();
			break;
		default :
				Debug.LogError("Unknown mechanic case is not in range");
			break;
		}
		lastMechanic = randomNumber;
	}

	void TeleportToRandomLocation()
	{
		Transform location = GetRandomTeleportTransform ();
		StartCoroutine(TeleportTo(location, teleportTime));
	}


//	GameObject teleportIn = (GameObject)
//	GameObject teleportOut = (GameObject)

	IEnumerator TeleportToVector(Vector3 location, float castTime)
	{
		Instantiate(teleportInEffect, transform.position+Vector3.up*0.05f, teleportInEffect.transform.rotation);
		Instantiate(teleportOutEffect, location+Vector3.up*0.05f, teleportOutEffect.transform.rotation);
		yield return new WaitForSeconds (castTime);
		abraCadaverAudio.PlaySoundEffectClip (abraCadaverAudio.teleportSound);
		transform.position = location;

		//GameObject teleportOut = (GameObject)Instantiate(teleportOutEffect, transform.position+Vector3.up*0.05f, teleportOutEffect.transform.rotation);

		Debug.Log ("Teleported to punch player");
	}

	IEnumerator TeleportTo(Transform location, float castTime)
	{
		StartCoroutine(TeleportToVector(location.position, castTime));
		transform.rotation = location.rotation;
		yield return null;
	}

	void RandomMeleeAttack()
	{
		MeleeAttackRandomPlayer ();
	}

	void MeleeAttackRandomPlayer()
	{
		GameObject targetPlayer = SelectRandomPlayer ();
		StartCoroutine("MeleeAttackFrontOfTarget", targetPlayer);
		//StartCoroutine("WaitForSpecifiedPeriod", meleeAttackWait);
	}

	IEnumerator MeleeAttackFrontOfTarget(GameObject player)
	{
		//Calculate a normalised Vector 3 to player and then multiply it by the scalar and add it to the players position
		Vector3 targetPosition = transform.position - player.transform.position;
		Vector3 scaledNormTargetPosition = targetPosition.normalized * meleeAttackRangeMultiplier;
		StartCoroutine(TeleportToVector(player.transform.position + scaledNormTargetPosition, teleportTime));
		yield return new WaitForSeconds (teleportTime);
		MeleeAttack (player);
	}
	void MeleeAttack(GameObject player)
	{
		Vector3 lookAtThis = new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z);
		transform.LookAt (lookAtThis);
		//transform.Rotate(player.transform.rotation);
		_animator.SetTrigger ("Melee");
	}

	IEnumerator ChannelFireAttack(float castTime)
	{
		TeleportToRandomLocation ();
		yield return new WaitForSeconds (teleportTime);
		StartCoroutine ("CastFor", castTime);
		abraFireStorm.StartCoroutine ("FireballStorm", castTime);
	}

	void ChannelRaiseAllMausoleums(float castTime)
	{
		if(mausoleumList.Count > 0)
		{
			mechanicOver = true;
			return;
		}
		waitingForMausoleums = true;
		Debug.Log (mausoleumSpawnPoints.Length);
		StartCoroutine ("CastFor", castTime);
		if (mausoleumSpawnPoints.Length > 0) 
		{
			foreach (Transform t in mausoleumSpawnPoints) 
			{
				GameObject mausoleumObject = Instantiate (mausoleum, t.position, t.rotation) as GameObject;
				MausoleumBehaviour mausoleumBehaviour = mausoleumObject.GetComponent<MausoleumBehaviour>();
				mausoleumList.Add(mausoleumBehaviour);
			}
		}
	}

	IEnumerator RaiseSingleRandomMausoleum ()
	{
		if(mausoleumList.Count > 0)
		{
			mechanicOver = true;
			yield break;
		}
		int randomInt = (int)Random.Range (0, mausoleumSpawnPoints.Length);
		Transform t = mausoleumSpawnPoints [randomInt]; 
		GameObject mausoleumObject = Instantiate (mausoleum, t.position, t.rotation) as GameObject;
		MausoleumBehaviour mausoleumBehaviour = mausoleumObject.GetComponent<MausoleumBehaviour>();
		mausoleumList.Add(mausoleumBehaviour);
		yield return new WaitForSeconds(raiseMausoleumTime);
		mechanicOver = true;
	}


	void CastARandomOrbSpell()
	{
		int myRandom = (int)Random.Range (0, 3);
		switch (myRandom) {
		case 0:
			StartCoroutine("CastOrbAttackOnAllPlayers");
			break;
		case 1:
			StartCoroutine("CastOrbAtRangedPlayersOnly");
			break;
		case 2:
			StartCoroutine("CastOrbAttackOnRandomPlayer");
			break;
		default:
			Debug.LogError("Unknown Orb Cast, case index is out of range");
			break;
		}
	}

	GameObject CheckIfPlayerIsInRange(float range)
	{
		closestPlayerObject = null;
		float closestDistance = Mathf.Infinity;
		for (int i = 0; i < GameController.gameController.players.Count; i++) 
		{
			float currentDistance = Vector3.Distance (transform.position, GameController.gameController.players [i].playerObject.transform.position);
			if(currentDistance < closestDistance)
			{
				closestDistance = currentDistance;
				closestPlayerObject = GameController.gameController.players[i].playerObject;
			}
		}
		if (closestPlayerObject != null && closestDistance < range) {
			return closestPlayerObject;
		} else {
			return null;
		}
	}
	Transform GetRandomTeleportTransform ()
	{
		int rand = (int)Random.Range(0, teleportationLocations.Length); 
		Debug.Log ("Rand is: " + rand);
		while (teleportationLocations[rand] == lastLocation && Vector3.Distance(teleportationLocations[rand].position, transform.position) > 1f) 
		{
			rand = (int)Random.Range(0, teleportationLocations.Length);
		}

		lastLocation = teleportationLocations [rand];
		//Debug.Log ("last Teleport Location was number: " + rand);
		return teleportationLocations [rand];
	}

	IEnumerator CastOrbAttackOnAllPlayers()
	{
		Transform location = GetRandomTeleportTransform ();
		StartCoroutine(TeleportTo (location, teleportTime));
		yield return new WaitForSeconds (orbWaitTime);
		for(int i = 0; i< GameController.gameController.players.Count; i++)
		{
			abraMagicOrb.StartCoroutine("ShootOrb", GameController.gameController.players[i].playerObject.transform);
		}
		yield return new WaitForSeconds (orbWaitTime);
	}



	IEnumerator CastOrbAttackOnRandomPlayer()
	{
		Transform location = GetRandomTeleportTransform ();
		StartCoroutine(TeleportTo (location, teleportTime));
		yield return new WaitForSeconds (orbWaitTime);
		Transform targetPlayerTransform = SelectRandomPlayer ().transform;
		abraMagicOrb.StartCoroutine ("ShootOrb", targetPlayerTransform);
		yield return new WaitForSeconds (orbWaitTime);
		//StartCoroutine("WaitForSpecifiedPeriod", orbWaitTime);
	}

	IEnumerator CastOrbAtRangedPlayersOnly()
	{
		Transform location = GetRandomTeleportTransform ();
		StartCoroutine(TeleportTo (location, teleportTime));
		yield return new WaitForSeconds (orbWaitTime);
		for(int i = 0; i< GameController.gameController.players.Count; i++)
		{
			if(GameController.gameController.players[i].playerObject.name == "Mage")
			{
				abraMagicOrb.StartCoroutine("ShootOrb", GameController.gameController.players[i].playerObject.transform);
			} else if (GameController.gameController.players[i].playerObject.name == "Archer")
			{
				abraMagicOrb.StartCoroutine("ShootOrb", GameController.gameController.players[i].playerObject.transform);
			}
		}
		yield return new WaitForSeconds (orbWaitTime);
		//StartCoroutine("WaitForSpecifiedPeriod", orbWaitTime);	
	}

	GameObject SelectRandomPlayer()
	{
		int playerNumber = Random.Range (0, GameController.gameController.players.Count);
		return GameController.gameController.players[playerNumber].playerObject;
	}

	IEnumerator CastFor(float sec)
	{
		foreach (GameObject g in enableOnCast) {
			g.SetActive(true);
		}
		yield return new WaitForSeconds(sec);
		foreach (GameObject g in enableOnCast) {
			g.SetActive(false);
		}
	}

	void MeleeExplosion(){
		Vector3 offsetVector = transform.position + transform.forward * explosionOffset.x + transform.up * explosionOffset.y;  
		Instantiate(meleeSphere, offsetVector, Quaternion.identity);
	}

	//Called
	void MeleeExplosion_OLD()
	{

		Vector3 offsetVector = transform.position + transform.forward * explosionOffset.x + transform.up * explosionOffset.y;  
		Instantiate(explosionObject, offsetVector, Quaternion.identity);
		//Damage all players within the damage radius
		RaycastHit[] _hits = Physics.SphereCastAll (offsetVector, explosionDamageRadius/2, Vector3.up);
		//Physics.SphereCastAll(
		foreach (RaycastHit _hit in _hits) {
			if (_hit.transform.tag == "Player"){
				_hit.transform.SendMessage ("AdjustCurHealth", -explosionDamage);
				//	Debug.Log (_hit.distance);
			}
		}
	}

	IEnumerator WaitForSpecifiedPeriodBeforePickingANewMechanic(float waitTime)
	{
		Debug.Log ("Waiting for: " + waitTime);
		yield return new WaitForSeconds(waitTime);
		//Debug.Log ("The wait is over");
		mechanicOver = true;
	}

	public void StartEncounter()
	{
		if (!started) {
			StartCoroutine ("RaiseStage");
			StartCoroutine ("CastFor", raiseStageTime);
			sceneMusic.PlayMusicArrayIndex (1);
			bossHealthBar.SetActive(true);
			started = true;
			abraCollider.enabled = true;
		}
	}
	IEnumerator RaiseStage()
	{
		Debug.Log ("Start RS");
		cameraShake.StartShake(raiseStageTime);
		Vector3 startPos = stage.transform.localPosition;
		Vector3 endPos = new Vector3 (0, 10, 0);
		float timer = raiseStageTime;
		float ammount = 0;
		while (timer > 0)
		{
			ammount = timer/raiseStageTime;
			stage.transform.localPosition = Vector3.Lerp (startPos, endPos, 1-ammount);
			timer -= Time.deltaTime;
			yield return null;
		}
		Debug.Log ("the ammount is now: " + ammount);
		startedBossEncounter = true;
		Debug.Log ("End RS");
	}

	void OnDeath()
	{
		isDead = true;
        _animator.SetBool("isDead", true);
		abraCollider.enabled = false;
		_animator.SetInteger("DanceNum", 0);
	}

}
