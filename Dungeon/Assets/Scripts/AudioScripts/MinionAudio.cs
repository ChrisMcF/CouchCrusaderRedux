using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MinionAudio : MonoBehaviour {
	public AudioClip[] deathSounds, hurtSounds, attackSounds;
	private AudioSource audioSource;
	
	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}
	
	public float PlayRandomSound(AudioClip[] clips)
	{
		if (clips.Length > 0) {
			int clipIndex = Random.Range (0, clips.Length);
			audioSource.PlayOneShot (clips [clipIndex]);
			return clips[clipIndex].length;
		} else {
			Debug.LogError("Error: No clips assigned for this action. Assign the AudioClipsInTheInspector");
			return 0f;
		}
	}
	
	public AudioSource GetAudioSource()
	{
		return audioSource;
	}
}
