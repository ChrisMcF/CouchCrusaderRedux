using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class CharacterAudio : MonoBehaviour {
	public AudioClip[] deathSounds, hurtSounds, cheerSounds, revivedSounds, specialAttackUsedSounds, lightAttackSounds, heavyAttackSounds, specialAttackSounds, failedAttackSounds;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}

	public float PlayRandomSound(AudioClip[] clips)
	{
        if (audioSource.isPlaying)
            return 0;

		if (clips.Length > 0) {
			int clipIndex = Random.Range (0, clips.Length);
			audioSource.PlayOneShot(clips [clipIndex]);
			return clips[clipIndex].length;
		} else {
			Debug.Log("Error: No clips assigned for this action. Assign the AudioClipsInTheInspector");
			return 0f;
		}
	}

	public float PlayRandomSound(AudioClip[] clips, float volume)
	{
		if (audioSource.isPlaying)
			return 0;
		
		if (clips.Length > 0) {
			int clipIndex = Random.Range (0, clips.Length);
			audioSource.PlayOneShot(clips [clipIndex], volume);
			return clips[clipIndex].length;
		} else {
			Debug.Log("Error: No clips assigned for this action. Assign the AudioClipsInTheInspector");
			return 0f;
		}
	}

	public AudioSource GetAudioSource()
	{
		return audioSource;
	}
}
