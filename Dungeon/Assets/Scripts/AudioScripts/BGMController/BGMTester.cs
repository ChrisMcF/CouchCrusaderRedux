using UnityEngine;
using System.Collections;

public class BGMTester : MonoBehaviour {

	public AudioClip[] testBGM;

	SceneMusic sceneMusic;

	// Use this for initialization
	void Start () {
		sceneMusic = GameObject.Find ("SceneMusic").GetComponent<SceneMusic> ();
	}
	
	// Update is called once per frame
	void Update () {

		//music selection
		if (Input.GetKeyDown("1")){
			BGMController.bgmController.PlayTrack(testBGM[0], BGMController.bgmController.defaultVolume);
		}
		if (Input.GetKeyDown("2")){
			BGMController.bgmController.PlayTrack(testBGM[1], BGMController.bgmController.defaultVolume);
		}
		if (Input.GetKeyDown("3")){
			BGMController.bgmController.PlayTrack(testBGM[2], BGMController.bgmController.defaultVolume);
		}
		if (Input.GetKeyDown("4")){
			BGMController.bgmController.PlayTrack(testBGM[3], BGMController.bgmController.defaultVolume);
		}
		if (Input.GetKeyDown("5")){
			BGMController.bgmController.PlayTrack(sceneMusic.musicArray[0], BGMController.bgmController.defaultVolume);
		}
		if (Input.GetKeyDown("6")){
			BGMController.bgmController.PlayTrack(sceneMusic.musicArray[1], BGMController.bgmController.defaultVolume);
		}

		//scene selection
		if (Input.GetKeyDown("q")){
			Application.LoadLevel (0);
		}
		if (Input.GetKeyDown("w")){
			Application.LoadLevel (1);
		}

		//function testing
		if (Input.GetKeyDown ("a")) {
			BGMController.bgmController.FadeTo(0, 3);
		}
		if (Input.GetKeyDown ("s")) {
			BGMController.bgmController.FadeTo(BGMController.bgmController.defaultVolume, 1);
		}
	}
}
