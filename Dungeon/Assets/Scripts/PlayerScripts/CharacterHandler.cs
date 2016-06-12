using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XboxCtrlrInput;
using XInputDotNetPure;

[RequireComponent(typeof(CharacterController))]
public class CharacterHandler : MonoBehaviour
{
    #region Components
    private CharacterController _charController;
    private Animator _animator;
    private BasePlayerClass _baseClass;
    private CharacterAudio _charAudio;
    #endregion

    private Vector3 _moveDirection;
    private bool _movementDisabled;
    private bool _actionsDisabled;
    
    private bool _attackingLight = false;
    private bool _attackingHeavy = false;
    private bool _attackingSpecial = false;

    private float _lightAttackCooldown;
    private float _heavyAttackCooldown;

    private float lightTriggerThisFrame, lightTriggerLastFrame;
    private float heavyTriggerThisFrame, heavyTriggerLastFrame;
    private bool _lightTriggerReleased, _heavyTriggerReleased;

    private float triggerThreshold = 0.99f;
    private float failSoundVolume = 0.4f;

    public float gravity = 20f;
    public bool allowAutomaticFire;

    void Start()
    {
        _charAudio = GetComponent<CharacterAudio>();
        _charController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _baseClass = GetComponent<BasePlayerClass>();

        StartCoroutine(DisableCooldownIcons());
    }

    IEnumerator DisableCooldownIcons()
    {
        yield return new WaitForEndOfFrame();
        _baseClass.lightCD.enabled = false;
        _baseClass.heavyCD.enabled = false;
    }
    public void EnableMovement()
    {
        _movementDisabled = false;
    }

    public void DisableMovement()
    {
        _movementDisabled = true;
    }

    public void DisableActions()
    {
        _actionsDisabled = true;
    }
    public void EnableActions()
    {
        _actionsDisabled = false;
    }

    void Update()
    {
        if(!allowAutomaticFire)
            CheckTriggerStates();

        CheckCooldowns();

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
    }


    void HandleMovement()
    {
        float hori_m = 0;
        float vert_m = 0;
        float hori_r = 0;
        float vert_r = 0;

        if (_movementDisabled)
            return;

        hori_m = XCI.GetAxis(XboxAxis.LeftStickX, _baseClass.controllerIndex);
        vert_m = XCI.GetAxis(XboxAxis.LeftStickY, _baseClass.controllerIndex);
        hori_r = XCI.GetAxis(XboxAxis.RightStickX, _baseClass.controllerIndex);
        vert_r = XCI.GetAxis(XboxAxis.RightStickY, _baseClass.controllerIndex);
        

        //Return from movement if no input is detected
        if (hori_r == 0 && vert_r == 0 && hori_m == 0 && vert_m == 0)
            return;

        // Get MoveDirection relative to camera position/angle
        Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;
        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        Vector3 moveDirection = (hori_m * right + vert_m * forward);

        Vector3 lookDirection;
        if (hori_r == 0 && vert_r == 0 && (hori_m != 0 || vert_m != 0)) // Look at movement dir if not getting right stick input
            lookDirection = Camera.main.transform.TransformDirection(new Vector3(hori_m, 0, vert_m));
        else // Look at right stick dir
            lookDirection = Camera.main.transform.TransformDirection(new Vector3(hori_r, 0, vert_r));

        // Rotate towards lookDirection
        lookDirection.y = 0.0f;
        Quaternion rotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * _baseClass.stats.rotationSpeed);

        //Supply moveSpeeds to animator
        _animator.SetFloat("VSpeed", vert_m);
        _animator.SetFloat("HSpeed", hori_m);

        //Move CharacterController
        _charController.Move(moveDirection * _baseClass.stats.moveSpeed * Time.deltaTime);
    }

    void HandleActions()
    {
        AnimatorStateInfo _info = _animator.GetCurrentAnimatorStateInfo(1);

        if (_info.IsTag("InAttack"))
            if (_animator.GetInteger("AttIndex") != 0)
                _animator.SetInteger("AttIndex", 0);

        if (!_actionsDisabled)
        {
            Debug.Log(_heavyTriggerReleased);
            if (!allowAutomaticFire && _lightTriggerReleased)
                LightAttack();
            else
                LightAttack();
            if (!allowAutomaticFire && _heavyTriggerReleased)
            {
                HeavyAttack();
            }
            else
                HeavyAttack();

            SpecialAttack();
        }
    }

    void LightAttack()
    {
        if (XCI.GetAxis(XboxAxis.RightTrigger, _baseClass.controllerIndex) == 1 && !_attackingLight)
        {
            _attackingLight = true;
            _animator.SetInteger("AttIndex", 1);
            _lightAttackCooldown = _baseClass.actions.light.cooldownTime;
            _baseClass.lightCD.enabled = true;
            _animator.SetBool("TriggerDown", true);
            _lightTriggerReleased = false;
            StartCoroutine(AnimateAttackCooldown(_lightAttackCooldown, _baseClass.lightCD));
        }
        else if (lightTriggerLastFrame < triggerThreshold && lightTriggerThisFrame >= triggerThreshold)
        {
            _charAudio.PlayRandomSound(_charAudio.failedAttackSounds, failSoundVolume);
        }
    }

    void HeavyAttack()
    {
        if (XCI.GetAxis(XboxAxis.LeftTrigger, _baseClass.controllerIndex) == 1 && !_attackingHeavy)
        {
            _attackingHeavy = true;
            _animator.SetInteger("AttIndex", 2);
            _heavyAttackCooldown = _baseClass.actions.heavy.cooldownTime;
            _baseClass.heavyCD.enabled = true;
            _heavyTriggerReleased = false;
            StartCoroutine(AnimateAttackCooldown(_heavyAttackCooldown, _baseClass.heavyCD));
        }
        else if (heavyTriggerLastFrame < triggerThreshold && heavyTriggerThisFrame >= triggerThreshold)
        {
            _charAudio.PlayRandomSound(_charAudio.failedAttackSounds, failSoundVolume);
        }
    }

    void SpecialAttack()
    {
        if (XCI.GetButtonDown(XboxButton.LeftBumper, _baseClass.controllerIndex) && !_attackingSpecial)
        {
            if (!_baseClass.specialUsed && _baseClass.specialReady)
            {
                _attackingSpecial = true;
                _animator.SetInteger("AttIndex", 3);
                _baseClass.specialUsed = true;
            }
            else if (XCI.GetButtonDown(XboxButton.LeftBumper, _baseClass.controllerIndex) && _attackingSpecial)
            {
                _charAudio.PlayRandomSound(_charAudio.failedAttackSounds, failSoundVolume);
            }
        }
    }

    void CheckTriggerStates()
    {
        lightTriggerLastFrame = lightTriggerThisFrame;
        heavyTriggerLastFrame = heavyTriggerThisFrame;
        lightTriggerThisFrame = XCI.GetAxis(XboxAxis.RightTrigger, _baseClass.controllerIndex);
        heavyTriggerThisFrame = XCI.GetAxis(XboxAxis.LeftTrigger, _baseClass.controllerIndex);

        if (lightTriggerThisFrame < 0.1)
            _lightTriggerReleased = true;
        if (heavyTriggerThisFrame < 0.1)
            _heavyTriggerReleased = true;
    }

    void CheckCooldowns()
    {
        if (_lightAttackCooldown > 0)
            _lightAttackCooldown -= Time.deltaTime;

        if (_lightAttackCooldown <= 0 && _attackingLight)
            _attackingLight = false;

        if (_heavyAttackCooldown > 0)
            _heavyAttackCooldown -= Time.deltaTime;

        if (_heavyAttackCooldown <= 0 && _attackingHeavy)
            _attackingHeavy = false;
    }

    IEnumerator AnimateAttackCooldown(float time, Image sprite)
    {
        float timer = time;
        float amount;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            amount = timer / time;
            sprite.fillAmount = amount;

            if (timer <= 0)
            {
                sprite.enabled = false;
            }
            yield return null;
        }
    }

    IEnumerator Vibrate(float time)
    {
        GamePad.SetVibration((PlayerIndex)_baseClass.controllerIndex, 1f, 1f);
        yield return new WaitForSeconds(time);
        GamePad.SetVibration((PlayerIndex)_baseClass.controllerIndex, 0f, 0f);
    }
}
