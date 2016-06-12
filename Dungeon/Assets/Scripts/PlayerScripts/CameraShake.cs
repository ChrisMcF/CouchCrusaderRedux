using UnityEngine;
using System;
using System.Collections;
using XInputDotNetPure;

public class CameraShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    private Transform camTransform;

    // How long the object should shake for.
    public float shakeTime = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    private bool active;

    private bool vibrating;

    Vector3 originalPos;

    void Awake()
    {
        camTransform = gameObject.transform;
    }

    IEnumerator GamePadVibrate()
    {
        for (int i = 0; i < GameController.gameController.players.Count;i++)
            GamePad.SetVibration((PlayerIndex)i, 1f, 1f);

        yield return new WaitForSeconds(shakeTime);

        for (int i = 0; i < GameController.gameController.players.Count; i++)
            GamePad.SetVibration((PlayerIndex)i, 0f, 0f);

        vibrating = false;
    }

    void Update()
    {
        if (GameController.gameController.gamePaused)
            return;

        if (shakeTime > 0)
        {
            if (!vibrating)
            {
                vibrating = true;
                StartCoroutine("GamePadVibrate");
            }
            active = true;
            camTransform.localPosition = originalPos + UnityEngine.Random.insideUnitSphere * shakeAmount;
            shakeTime -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            if (active)
            {
                shakeTime = 0f;
                camTransform.localPosition = originalPos;
                active = false;
            }
        }
    }

    public void StartShake(float _shakeTime)
    {
        originalPos = camTransform.localPosition;
        shakeTime = _shakeTime;
    }

	void OnDestroy()
	{
		for (int i = 0; i < GameController.gameController.players.Count; i++)
			GamePad.SetVibration((PlayerIndex)i, 0f, 0f);
	}
}