using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
[RequireComponent(typeof(AudioSource))]

//This script requires a child game object called "SoundEffects" that has an AudioSourceComponent
public class BossAudio : MonoBehaviour 
{
	public AudioMixerGroup soundEffectsMixerGroup;
	private GameObject soundEffectsObject;
	public AudioClip[] deathSounds, hurtSounds, inFightSounds, encounterStart, cheerSounds, rangedAttackSounds, meleeAttackSounds;
	private AudioSource voiceActingAudioSource;
	private AudioSource soundEffectsAudioSource;

	// Use this for initialization
	void Awake () {
		soundEffectsObject = new GameObject ("SoundEffects");
		soundEffectsObject.transform.parent = this.gameObject.transform;
		soundEffectsAudioSource = soundEffectsObject.AddComponent<AudioSource> ();
		soundEffectsAudioSource.outputAudioMixerGroup = soundEffectsMixerGroup;
		voiceActingAudioSource = GetComponent<AudioSource> ();
	}

	public float PercentageChanceToPlayRandomVoiceClip(AudioClip[] clips, float percentage)
	{
		float randomValue = (float)Random.Range (0.0f, 100.0f);
		if (randomValue < percentage) {
			float returnVal = PlayRandomVoiceClip(clips);
			return returnVal;
		} else
			return 0f;
	}


	public float PlayRandomVoiceClip(AudioClip[] clips)
	{
		if (clips.Length > 0) 
		{
			//Stop playing old clip
			voiceActingAudioSource.Stop();
			int clipIndex = Random.Range (0, clips.Length);
			voiceActingAudioSource.PlayOneShot (clips [clipIndex]);
			return clips[clipIndex].length;
		} else 
		{
			Debug.Log("Error: No clips assigned for this action. Assign the AudioClipsInTheInspector");
			return 0f;
		}
	}

	public float PlayRandomSoundEffectClip(AudioClip[] clips)
	{
		if (clips.Length > 0) 
		{
			int clipIndex = Random.Range (0, clips.Length);
			soundEffectsAudioSource.PlayOneShot (clips [clipIndex]);
			return clips[clipIndex].length;
		} else 
		{
			Debug.Log("Error: No clips assigned for this action. Assign the AudioClipsInTheInspector");
			return 0f;
		}
	}
	
	public float PercentageChanceToPlayRandomSoundEffectClip(AudioClip[] clips, float percentage)
	{
		float randomValue = (float)Random.Range (0.0f, 100.0f);
		if (randomValue < percentage) {
			float returnVal = PlayRandomSoundEffectClip(clips);
			return returnVal;
		} else
			return 0f;
	}
	public AudioSource GetVoiceActingAudioSource()
	{
		return voiceActingAudioSource;
	}

	public AudioSource GetSoundEffectAudioSource()
	{
		return soundEffectsAudioSource;
	}
}
