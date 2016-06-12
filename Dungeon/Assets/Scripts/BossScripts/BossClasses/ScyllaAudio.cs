using UnityEngine;
using System.Collections;

public class ScyllaAudio : BossAudio {

	public AudioClip[] beforeJumpIn;
	public AudioClip[] beforeJumpOut;
	public AudioClip[] whilstInPool;
	public AudioClip landSound;
	public AudioClip healSound;


	public void PlaySoundEffectClip(AudioClip clip)
	{
		AudioSource audioSource = GetSoundEffectAudioSource ();
		audioSource.PlayOneShot (clip);
	}

	public IEnumerator RepeatSoundEffect(AudioClip clip)
	{
		float wait = 0;
		AudioSource audioSource = GetSoundEffectAudioSource ();
		while (true) 
		{
			if(wait <= 0)
			{
				audioSource.PlayOneShot (clip);
				wait = clip.length;
			}
			wait -= Time.deltaTime;
			//Debug.Log(wait);
			yield return null;
			
		}
	}
}
