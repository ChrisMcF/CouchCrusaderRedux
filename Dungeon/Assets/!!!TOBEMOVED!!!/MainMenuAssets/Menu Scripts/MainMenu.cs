using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XboxCtrlrInput;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
	public bool selectingCharacters = false;
	public string nextScene;
	public string creditsScene;
	public GameObject mainMenu;
	public GameObject settings;
	private bool isMainMenuOn = true;
	private float volumeSpeed = 100f;
	public GameObject volumeSliderObj;
	public Slider volumeSlider;
	private float volume = 100f;
	public AudioClip aSound;
	public AudioClip bSound;
	private AudioSource audioSource;
	public AudioMixer mixer;
	// Use this for initialization
	void Start ()
	{
		audioSource = GetComponent<AudioSource> ();
		//audioSource = BGMController.bgmController.bgmAudioSource;
		//audioSource.volume = (volume/100);
		volumeSlider.value = volume;

		GameObject levelSpecificObject = GameObject.FindGameObjectWithTag ("LevelSpecific");
		SceneMusic sceneMusic = levelSpecificObject.GetComponent<SceneMusic> ();
		sceneMusic.PlayMusicArrayIndex (0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!selectingCharacters)
        {
            for (int i = 1; i <= XCI.GetNumPluggedCtrlrs(); i++)
            {
                if (XCI.GetAxis(XboxAxis.LeftStickX, i) > 0.1 && !isMainMenuOn)
                {
                    RaiseVolume();
                }
                //if (XCI.GetAxis(XboxAxis.LeftStickX) < 0 && XCI.GetAxis(XboxAxis.LeftStickX) > -1) 
                if (XCI.GetAxis(XboxAxis.LeftStickX, i) < -0.1 && !isMainMenuOn)
                {
                    LowerVolume();
                }

                if (XCI.GetButtonDown(XboxButton.A, i))
                {
                    Debug.Log("A pressed");
                    audioSource.PlayOneShot(aSound);
                    Application.LoadLevelAdditive(nextScene);
                    mainMenu.SetActive(false);
                    selectingCharacters = true;
                }

                if (XCI.GetButtonDown(XboxButton.B, i))
                {
                    Debug.Log("B pressed");
                    audioSource.PlayOneShot(bSound);
                    if (isMainMenuOn == true)
                    {
                        turnSettingsMenuOn();

                    }
                    else if (isMainMenuOn == false)
                    {
                        turnMainMenuOn();
                    }
                }
                if (XCI.GetButtonDown(XboxButton.X, i))
                {
                    Debug.Log("X pressed");
                    audioSource.PlayOneShot(bSound);

                    Application.LoadLevel(creditsScene);

                }
                if (XCI.GetButtonDown(XboxButton.Y, i))
                {
                    Debug.Log("Y pressed");
                    Application.Quit();
                }
            }
			//if (XCI.GetAxis(XboxAxis.LeftStickX) < 1 && XCI.GetAxis(XboxAxis.LeftStickX) > 0) 
			
		}
	}
	public void turnMainMenuOn ()
	{
		Debug.Log ("menu on");
		isMainMenuOn = true;
		settings.SetActive (false);
		volumeSliderObj.SetActive (false);
		mainMenu.SetActive (true);
	}
	public void turnSettingsMenuOn ()
	{
		Debug.Log ("settings on");
		isMainMenuOn = false;
		settings.SetActive (true);
		volumeSliderObj.SetActive (true);
		mainMenu.SetActive (false);
	}
	public void RaiseVolume ()
	{
		Debug.Log ("Increasing volume");
		if (volume < 100f)
			volume += Time.deltaTime * volumeSpeed * XCI.GetAxis (XboxAxis.LeftStickX);
		else
			volume = 100f;
		volumeSlider.value = volume;
		//audioSource.volume = (volume/100);
		mixer.SetFloat ("MasterVolume", GetDBValueFromVolume (volume));

	}
	public void LowerVolume ()
	{
		Debug.Log ("Decreasing volume");
		if (volume > 0) 
			volume -= Time.deltaTime * volumeSpeed * -XCI.GetAxis (XboxAxis.LeftStickX);
		else
			volume = 0;

		volumeSlider.value = volume;
		//audioSource.volume = (volume/100);
		mixer.SetFloat ("MasterVolume", GetDBValueFromVolume (volume));
	}
	public float GetDBValueFromVolume (float volume)
	{
		float ammount = volume / 100;
		return ammount * 80 - 80;
	}
}
