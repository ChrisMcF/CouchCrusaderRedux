using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

[RequireComponent(typeof (AudioSource))]

public class BGMController : MonoBehaviour {

	public static BGMController bgmController;

	[HideInInspector]
	public AudioSource bgmAudioSource;
//	[HideInInspector]
//	public AudioMixer bgmAudioMixer;

	[Range(0.0f, 1.0f)]
	public float defaultVolume = 1.0f;



	// Use this for initialization
	void Awake () {

		//Makes this BGM Controller object the one object throughout the whole game. If there is another
		//instance of the object in a new scene it deletes that object and keeps this one.
		if (bgmController == null)
		{
			DontDestroyOnLoad (gameObject);
			bgmController = this;
			bgmAudioSource = GetComponent<AudioSource>();	//makes the AudioSource publicly accessible to other scripts
			bgmAudioSource.volume = defaultVolume;
			bgmAudioSource.loop = true;
		}
		else if (bgmController != this)
		{
			Destroy (gameObject);
		}
	}

	//Starts playing the specified audio file at '_volume'. If you want to fade into the start of the song, us PlayTrack and FadeTo together.
	public void PlayTrack(AudioClip _newTrack, float _volume){
		bgmAudioSource.clip = _newTrack;
		bgmAudioSource.volume = _volume;
		bgmAudioSource.Play();
	}

	//Fades the music to the specified volume over '_time' seconds. Does not stop or pause the music.
	public void FadeTo(float _volume, float _time){
		iTween.AudioTo (gameObject, iTween.Hash ("audiosource", bgmAudioSource, "volume", _volume, "time", _time, "easetype", iTween.EaseType.easeInQuad));
	}


}
