using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XboxCtrlrInput;
using XInputDotNetPure;

[RequireComponent (typeof(CharacterController))]
public class Old_CharacterHandler : MonoBehaviour
{
	public enum ControllerStyle
	{
		TwinStick,
		TwinStickAlternate,
		Regular
	}
	[Tooltip("If enabled, this bool allows continuous firing of attacks whilst the trigger is held in")]
	public bool allowAutomaticFiring = false;

	private CharacterAudio characterAudio;
	public ControllerStyle controllerStyle;

	private CharacterController _charController;
	private Animator _animator;
	private BasePlayerClass _baseClass;

	private float _lightAttackCooldown;
	private float _heavyAttackCooldown;

    [HideInInspector]
	public float _specialAttackCooldown;
	private float _evadeCooldown;

	private bool _attackingLight = false;
	private bool _attackingHeavy = false;
	private bool _attackingSpecial = false;

	private XboxAxis lightAction_Axis;
	private XboxButton lightAction_Button;
	private XboxAxis heavyAction_Axis;
	private XboxButton heavyAction_Button;
	private XboxAxis specialAction_Axis;
	private XboxButton specialAction_Button;
	private XboxAxis dodgeAction_Axis;
	private XboxButton dodgeAction_Button;
	private float failSoundVolume = 0.4f;


	public bool _attackEnabled = true;

	public Vector3 _moveDirection;

	public float gravity = 20.0F;

    private bool movementDisabled = false;

    //Values to store the trigger data for this & the last frame;
    private float lightTriggerLastFrame, lightTriggerThisFrame, heavyTriggerLastFrame, heavyTriggerThisFrame;
	private float triggerThreshold = 0.99f;

	private bool _lightTriggerWasReleased = false, _heavyTriggerWasReleased = false;

	void Start ()
	{

		characterAudio = GetComponent<CharacterAudio> ();
		_charController = GetComponent<CharacterController> ();
		_animator = GetComponent<Animator> ();
		_baseClass = GetComponent<BasePlayerClass> ();

      	
            
		switch (controllerStyle) {
		case ControllerStyle.TwinStick:
			lightAction_Axis = XboxAxis.RightTrigger;
			heavyAction_Axis = XboxAxis.LeftTrigger;
			dodgeAction_Button = XboxButton.RightBumper;
			specialAction_Button = XboxButton.LeftBumper;
			break;
		case ControllerStyle.Regular:
			lightAction_Button = XboxButton.A;
			heavyAction_Button = XboxButton.X;
			dodgeAction_Button = XboxButton.B;
			specialAction_Button = XboxButton.Y;
			break;
		case ControllerStyle.TwinStickAlternate:
			lightAction_Button = XboxButton.LeftBumper;
			heavyAction_Axis = XboxAxis.LeftTrigger;
			dodgeAction_Button = XboxButton.RightBumper;
			specialAction_Axis = XboxAxis.RightTrigger;
			break;
		}

		StartCoroutine ("DisableHudStuff");
	}
	IEnumerator DisableHudStuff ()
	{
		yield return new WaitForSeconds (0.1f);
		_baseClass.lightCD.enabled = false;
		_baseClass.heavyCD.enabled = false;
	}


    public void EnableMovement()
    {
        movementDisabled = false;
    }

    public void DisableMovement()
    {
        movementDisabled = true;
    }

    // Update is called once per frame
    void Update ()
	{
		lightTriggerLastFrame = lightTriggerThisFrame;
		heavyTriggerLastFrame = heavyTriggerThisFrame;
		heavyTriggerThisFrame = XCI.GetAxis (heavyAction_Axis, _baseClass.controllerIndex);
		lightTriggerThisFrame = XCI.GetAxis (lightAction_Axis, _baseClass.controllerIndex);

		if (lightTriggerThisFrame < 0.1)
			_lightTriggerWasReleased = true;
		if (heavyTriggerThisFrame < 0.1)
			_heavyTriggerWasReleased = true;

        RaycastHit groundDetect;
        Debug.DrawRay(transform.position + (Vector3.up * 2f), -Vector3.up * 3, Color.green, 5f);
        if (Physics.Raycast(transform.position + (Vector3.up * 2f), -Vector3.up, out groundDetect, 5f))
        {
                HandleMovement();
                HandleActions();
        }
        else
        {
            _moveDirection = new Vector3(0, 0, 0);
            _moveDirection.y -= gravity * Time.deltaTime;
            _charController.Move(((_moveDirection * _baseClass.stats.moveSpeed) * Time.deltaTime));
        }

        // Always handle movement
        // Only handle attack code if not in attackCooldown;

		if (_lightAttackCooldown > 0) 
		{
			_lightAttackCooldown -= Time.deltaTime;
			if(_lightAttackCooldown <= 0)
			{
				if(_attackingLight)
				{
					_attackingLight = false;
					//if (_attackingLight && XCI.GetAxis(lightAction_Axis, _baseClass.controllerIndex) == 0)
					//	_attackingLight = false;
				}
			}
		}
		if (_heavyAttackCooldown > 0) 
		{
			_heavyAttackCooldown -= Time.deltaTime;
			if(_heavyAttackCooldown <= 0)
			{
				if(_attackingHeavy)
					_attackingHeavy = false;
			}
		}
		if (_specialAttackCooldown > 0) 
		{
			_specialAttackCooldown -= Time.deltaTime;
			if(_specialAttackCooldown <= 0)
			{
				if(_attackingSpecial)
					_attackingSpecial = false;
			}	
		}
	}

	
	void HandleMovement () // Code to handle AnalogStick Input for movement and rotation
	{
		AnimatorStateInfo _info = _animator.GetCurrentAnimatorStateInfo (1);


		if (_info.IsTag ("InAttack"))
        {
			if (_animator.GetInteger ("AttIndex") != 0)
            {
                _animator.SetInteger("AttIndex", 0);
            }
		}

        // Get left analogue stick input (movement)
        float _hori = 0;
        float _vert = 0;
        if (!movementDisabled)
        {
            _hori = XCI.GetAxis(XboxAxis.LeftStickX, _baseClass.controllerIndex);
            _vert = XCI.GetAxis(XboxAxis.LeftStickY, _baseClass.controllerIndex);
        }

        if (controllerStyle == ControllerStyle.TwinStick || controllerStyle == ControllerStyle.TwinStickAlternate) {
			float _rotHori = XCI.GetAxis (XboxAxis.RightStickX, _baseClass.controllerIndex);
			float _rotVert = XCI.GetAxis (XboxAxis.RightStickY, _baseClass.controllerIndex);

			if (_rotHori != 0 || _rotVert != 0) {
				transform.eulerAngles = new Vector3 (0, Mathf.Atan2 (_rotHori, _rotVert) * 180 / Mathf.PI, 0);
			} else if (_hori != 0 || _vert != 0) {
				transform.eulerAngles = new Vector3 (0, Mathf.Atan2 (_hori, _vert) * 180 / Mathf.PI, 0);
			}

		} else {
			if (_hori != 0 || _vert != 0) {
				transform.eulerAngles = new Vector3 (0, Mathf.Atan2 (_hori, _vert) * 180 / Mathf.PI, 0);
			}
		}
		// Get right analogue stick input (rotation)

		// Set animator variables to control blendtree
		_animator.SetFloat ("VSpeed", _vert);
		_animator.SetFloat ("HSpeed", _hori);

		// rotate only if right analogue stick is off centre (prevents snapping to original position)
        
        
		// Move Player
		_moveDirection = new Vector3 (_hori, 0, _vert);
		_moveDirection.y -= gravity * Time.deltaTime;


		//slow down characters speed in AnimatorState's tag is "InGuard"
		if (_animator.GetCurrentAnimatorStateInfo (1).IsTag ("InGuard"))
			_charController.Move (((_moveDirection * 2) * Time.deltaTime));
		else
			_charController.Move (((_moveDirection * _baseClass.stats.moveSpeed) * Time.deltaTime));

	}

	void HandleActions ()
	{

		if (_attackEnabled) {
			switch (controllerStyle) {
			case ControllerStyle.Regular:
				RegularControls ();
				break;
			case ControllerStyle.TwinStick:
				TwinStickControls ();
				break;
			case ControllerStyle.TwinStickAlternate:
				TwinStickAlternateControls ();
				break;
			}
		}
	}


	void RegularControls ()
	{
		if (XCI.GetButtonDown (lightAction_Button, _baseClass.controllerIndex) && !_attackingLight) {
			_attackingLight = true;
			_animator.SetInteger ("AttIndex", 1);
			_lightAttackCooldown = _baseClass.actions.light.cooldownTime;
			_animator.SetBool ("TriggerDown", true);

		}
		if (XCI.GetButtonUp (lightAction_Button, _baseClass.controllerIndex)) {
			_animator.SetBool ("TriggerDown", false);
			
		}
		if (XCI.GetButtonDown (heavyAction_Button, _baseClass.controllerIndex) && !_attackingHeavy) {
			_attackingHeavy = true;
			_animator.SetInteger ("AttIndex", 2);
			_heavyAttackCooldown = _baseClass.actions.heavy.cooldownTime;
		}
		if (XCI.GetButtonDown (specialAction_Button, _baseClass.controllerIndex) && !_attackingSpecial) {
			if (!_baseClass.specialUsed && _baseClass.specialReady) {
				_attackingSpecial = true;
				_animator.SetInteger ("AttIndex", 3);
			}
		}
		if (XCI.GetButtonDown (dodgeAction_Button, _baseClass.controllerIndex)) {

			_animator.SetInteger ("AttIndex", 4);
			_lightAttackCooldown = _baseClass.actions.light.cooldownTime;
		}
	}

	void TwinStickControls ()
	{
		if (!allowAutomaticFiring) {
			if (_lightTriggerWasReleased) {
				TwinStickTryLightAttack ();
			}
		} else {
			TwinStickTryLightAttack ();
		}

		if (XCI.GetAxis (lightAction_Axis, _baseClass.controllerIndex) == 0) {
			_animator.SetBool ("TriggerDown", false);
		}
        
		if (!allowAutomaticFiring) {
			if (_heavyTriggerWasReleased) {
				TwinStickTryHeavyAttack ();
			}
		} else {
			TwinStickTryHeavyAttack ();
		}




		TwinStickTrySpecialAttack ();
	}
	void TwinStickAlternateControls ()
	{
		if (XCI.GetButton (lightAction_Button, _baseClass.controllerIndex) && !_attackingLight) 
		{
			_attackingLight = true;
			_animator.SetInteger ("AttIndex", 1);
			_lightAttackCooldown = _baseClass.actions.light.cooldownTime;
			_baseClass.lightCD.enabled = true;
			StartCoroutine (AttackCooldown (_lightAttackCooldown, _baseClass.lightCD));
			_animator.SetBool ("TriggerDown", true);
			
		}
		if (XCI.GetButtonUp (lightAction_Button, _baseClass.controllerIndex)) {
			_animator.SetBool ("TriggerDown", false);
			
		}
        
		if (XCI.GetAxis (heavyAction_Axis, _baseClass.controllerIndex) == 1 && !_attackingHeavy) {

			_attackingHeavy = true;
			_animator.SetInteger ("AttIndex", 2);
			_heavyAttackCooldown = _baseClass.actions.heavy.cooldownTime;
			_baseClass.heavyCD.enabled = true;
			StartCoroutine (AttackCooldown (_heavyAttackCooldown, _baseClass.heavyCD));
		}
		if (XCI.GetAxis (specialAction_Axis, _baseClass.controllerIndex) == 1 && !_attackingSpecial)
        {
			if (!_baseClass.specialUsed && _baseClass.specialReady) {
				//Debug.Log(specialAction_Button.ToString() + " - Player " + _baseClass.controllerIndex);
				_attackingSpecial = true;
				_animator.SetInteger ("AttIndex", 3);
				_specialAttackCooldown = _baseClass.actions.special.cooldownTime;
				_baseClass.specialUsed = true;
			} else if(XCI.GetAxis (specialAction_Axis, _baseClass.controllerIndex) ==1){
				//characterAudio.PlayRandomSound(characterAudio.failedAttackSounds);
			}
		}
		if (XCI.GetButtonDown (dodgeAction_Button, _baseClass.controllerIndex)) {
			_animator.SetInteger ("AttIndex", 4);
			_lightAttackCooldown = _baseClass.actions.light.cooldownTime;
		}
	}

	IEnumerator AttackCooldown (float time, Image sprite)
	{
		float timer = time;
		float amount;
		while (timer > 0) {
			timer -= Time.deltaTime;
			amount = timer / time;
			sprite.fillAmount = amount;

			if (timer <= 0) {
				sprite.enabled = false;
			}
			yield return null;
		}
	}

	//Disables actions when certain conditions are met, player holding bath salts, etc
	public void DisableActions ()
	{
		_attackEnabled = false;
	}

	IEnumerator Vibrate (float time)
	{
		PlayerIndex index = PlayerIndex.One;
		switch (_baseClass.controllerIndex) {
		case 1:
			index = PlayerIndex.One;
			break;
		case 2:
			index = PlayerIndex.Two;
			break;
		case 3:
			index = PlayerIndex.Three;
			break;
		case 4:
			index = PlayerIndex.Four;
			break;
		}
		GamePad.SetVibration (index, 1f, 1f);
		yield return new WaitForSeconds (time);
		GamePad.SetVibration (index, 0, 0);
	}


	//Enables actions when certain conditions are met, player is no longer holding bath salts, etc
	public void EnableActions ()
	{
		_attackEnabled = true;
	}

	public void TwinStickTryLightAttack()
	{
		if (XCI.GetAxis (lightAction_Axis, _baseClass.controllerIndex) == 1 && !_attackingLight) {
			_attackingLight = true;
			_animator.SetInteger ("AttIndex", 1);
			_lightAttackCooldown = _baseClass.actions.light.cooldownTime;
			_baseClass.lightCD.enabled = true;
			StartCoroutine (AttackCooldown (_lightAttackCooldown, _baseClass.lightCD));
			_animator.SetBool ("TriggerDown", true);
			_lightTriggerWasReleased = false;
		}//} else if (XCI.GetAxis (lightAction_Axis, _baseClass.controllerIndex) == 1 && _attackingLight) {
		else if (lightTriggerLastFrame < triggerThreshold && lightTriggerThisFrame >= triggerThreshold)
		{
			characterAudio.PlayRandomSound (characterAudio.failedAttackSounds, failSoundVolume);
		}
	}
	public void TwinStickTryHeavyAttack()
	{
		if (XCI.GetAxis (heavyAction_Axis, _baseClass.controllerIndex) == 1 && !_attackingHeavy) {
			
			_attackingHeavy = true;
			_animator.SetInteger ("AttIndex", 2);
			_heavyAttackCooldown = _baseClass.actions.heavy.cooldownTime;
			_baseClass.heavyCD.enabled = true;
			StartCoroutine (AttackCooldown (_heavyAttackCooldown, _baseClass.heavyCD));
			_heavyTriggerWasReleased = false;
			
		}
		else if (heavyTriggerLastFrame < triggerThreshold && heavyTriggerThisFrame >= triggerThreshold) {
			characterAudio.PlayRandomSound (characterAudio.failedAttackSounds, failSoundVolume);
		}
	}
	public void TwinStickTrySpecialAttack()
	{
		if (XCI.GetButtonDown (specialAction_Button, _baseClass.controllerIndex) && !_attackingSpecial) {
			if (!_baseClass.specialUsed && _baseClass.specialReady) {
				//Debug.Log(specialAction_Button.ToString() + " - Player " + _baseClass.controllerIndex);
				_attackingSpecial = true;
				_animator.SetInteger ("AttIndex", 3);
				_specialAttackCooldown = _baseClass.actions.special.cooldownTime;
				_baseClass.specialUsed = true;
			} else if(XCI.GetButtonDown (specialAction_Button, _baseClass.controllerIndex) && _attackingSpecial) {
				characterAudio.PlayRandomSound(characterAudio.failedAttackSounds, failSoundVolume);
			}
		}
		if (XCI.GetButtonDown (dodgeAction_Button, _baseClass.controllerIndex)) {
			_animator.SetInteger ("AttIndex", 4);
			_lightAttackCooldown = _baseClass.actions.light.cooldownTime;
		}
	}
}
