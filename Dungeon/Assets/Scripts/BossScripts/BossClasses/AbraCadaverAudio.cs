using UnityEngine;
using System.Collections;

public class AbraCadaverAudio : BossAudio {
	
	public AudioClip teleportSound;
	public AudioClip channelSpellSound;
	public AudioClip raiseSound;

	public void PlaySoundEffectClip(AudioClip clip)
	{
		AudioSource soundEffectsAudioSource = GetSoundEffectAudioSource ();
		soundEffectsAudioSource.PlayOneShot (clip);
	}
	
	public IEnumerator RepeatSoundEffectClip(AudioClip clip)
	{
		float wait = 0;
		AudioSource soundEffectsAudioSource = GetSoundEffectAudioSource ();
		while (true) 
		{
			if(wait <= 0)
			{
				soundEffectsAudioSource.PlayOneShot (clip);
				wait = clip.length;
			}
			wait -= Time.deltaTime;
			//Debug.Log(wait);
			yield return null;	
		}
	}
}
