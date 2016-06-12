using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneMusic : MonoBehaviour {

//	public List<AudioClip> musicList = new List<AudioClip>();	//A list holding all music in the specific scene
	public AudioClip[] musicArray;

	void Start()
	{
		/*
		foreach (AudioClip i in musicArray) 
		{
			BGMController.bgmController.activeSongs.Add(i);
		}
		*/
		if(musicArray.Length>0)
			PlayMusicArrayIndex(0);
	}

	public void PlayMusicArrayIndex (int indexOfSongInMusicArray)
	{
		//AudioClip myClip = musicArray [indexOfSongInMusicArray];
		BGMController.bgmController.PlayTrack (musicArray[indexOfSongInMusicArray], 1f);
	}
}
