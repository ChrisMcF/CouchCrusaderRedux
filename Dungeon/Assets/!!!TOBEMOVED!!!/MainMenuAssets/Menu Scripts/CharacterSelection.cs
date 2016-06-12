using UnityEngine;
using System.Collections;

public class CharacterSelection : MonoBehaviour
{
	
	public string levelToLoad;
	//ROTATION
	public float rotationDegree = 180;
	public GameObject archer, paladin, reaver, mage;
	// AUDIO
	public AudioClip selectorSound;
	public AudioClip chosenSound;
	public AudioClip join;
	public bool[] charTaken = new bool[4];
	//Character 1 selector 

	public GameObject[] p1Pos;
	private bool p1SelectorHasMoved = false;
	public GameObject p1Selector;
	private int p1PosIndex = 0;
	public GameObject particleP1;
	private bool player1HasChosen = false;

	//Character 2 selector
	public GameObject[] p2Pos;
	private bool p2SelectorHasMoved = false;
	public GameObject p2Selector;
	private int p2PosIndex = 0;
	public GameObject particleP2;
	private bool player2HasChosen = false;
	public GameObject p2TextToJoin;
	bool player2isOut = true;

	//Character 3 selector
	public GameObject[] p3Pos;
	private bool p3SelectorHasMoved = false;
	public GameObject p3Selector;
	private int p3PosIndex = 0;
	public GameObject particleP3;
	private bool player3HasChosen = false;
	public GameObject p3TextToJoin;
	bool player3isOut = true;

	//Character 4 selector
	public GameObject[] p4Pos;
	private bool p4SelectorHasMoved = false;
	public GameObject p4Selector;
	private int p4PosIndex = 0;
	public GameObject particleP4;
	private bool player4HasChosen = false;
	public GameObject p4TextToJoin;
	bool player4isOut = true;

	//Setting each character to the controller number.
	[HideInInspector]
	public GameController controller;
	public GameObject[] character = new GameObject[4];
	// Use this for initialization
	void Start ()
	{
		charTaken [0] = false;
		charTaken [1] = false;
		charTaken [2] = false;
		charTaken [3] = false;

		controller = GameObject.Find ("GameController").GetComponent<GameController> ();

		character = GameObject.FindGameObjectsWithTag ("Player");

		//levelToLoad = "Scylla";
	}
	
	// Update is called once per frame
	void Update ()
	{

		Player1Selector ();

		Player2Selector ();

		Player3Selector ();

		Player4Selector ();

		if (player1HasChosen /*&& player2HasChosen && player3HasChosen && player4HasChosen */) {
			LoadLevel ();
		}

	}


	public void LoadLevel ()
	{
		Application.LoadLevel (levelToLoad);
	}

	void Player1Selector ()
	{
		//P1 SELECTOR CODE ///////////////////////////////////////////////////////////////////////////////
		float h1 = Input.GetAxisRaw ("LeftHori_player1");
		if (player1HasChosen == false) {
			if (p1SelectorHasMoved) {
				if (h1 < 0.2 && h1 > -0.2)
					p1SelectorHasMoved = false;
			}
			if (p1SelectorHasMoved == false && (h1 > 0.2 || h1 < -0.2)) {
				p1SelectorHasMoved = true;
				if (h1 <= -0.1) {
					p1PosIndex--;
					if (p1PosIndex < 0)
						p1PosIndex = 3;
					p1Selector.transform.position = p1Pos [p1PosIndex].transform.position;
					AudioSource.PlayClipAtPoint (selectorSound, Camera.main.transform.position);
				} else if (h1 >= 0.1) {
					p1PosIndex++;
					if (p1PosIndex > 3)
						p1PosIndex = 0;
					p1Selector.transform.position = p1Pos [p1PosIndex].transform.position;
					AudioSource.PlayClipAtPoint (selectorSound, Camera.main.transform.position);
				}
			}
		}
		
		if (Input.GetKeyDown (KeyCode.Joystick1Button0)) {
			AudioSource.PlayClipAtPoint (chosenSound, Camera.main.transform.position);
			if (p1Selector.transform.position == p1Pos [0].transform.position && !charTaken [0]) {
				particleP1.SetActive (true);
				player1HasChosen = true;
				Debug.Log ("P1 has chosen Archer");
				charTaken [0] = true;

				GetController ("Archer", 1);
			}
			if (p1Selector.transform.position == p1Pos [1].transform.position && !charTaken [1]) {
				Debug.Log ("P1 has chosen Paladin");
				charTaken [1] = true;
				particleP1.SetActive (true);
				player1HasChosen = true;

				GetController ("Paladin", 1);
			}
			if (p1Selector.transform.position == p1Pos [2].transform.position && !charTaken [2]) {
				Debug.Log ("P1 has chosen Reaver");
				charTaken [2] = true;
				particleP1.SetActive (true);
				player1HasChosen = true;

				GetController ("Warrior", 1);
			}
			if (p1Selector.transform.position == p1Pos [3].transform.position && !charTaken [3]) {
				Debug.Log ("P1 has chosen Mage");
				charTaken [3] = true;
				particleP1.SetActive (true);
				player1HasChosen = true;

				GetController ("Mage", 1);
			}
		}
		if (Input.GetKey (KeyCode.Joystick1Button0)) {
			Debug.Log ("B pushed");
			if (p1Selector.transform.position == p1Pos [0].transform.position) {
				archer.transform.Rotate (0, rotationDegree, 0);
				rotationDegree = 0;	
			}
			if (p1Selector.transform.position == p1Pos [1].transform.position) {
				paladin.transform.Rotate (0, rotationDegree, 0);
				rotationDegree = 0;	
			}
			if (p1Selector.transform.position == p1Pos [2].transform.position) {
				reaver.transform.Rotate (0, rotationDegree, 0);
				rotationDegree = 0;	
			}
			if (p1Selector.transform.position == p1Pos [3].transform.position) {
				mage.transform.Rotate (0, rotationDegree, 0);
				rotationDegree = 0;	
			}
		}
		if (Input.GetKeyUp (KeyCode.Joystick1Button0)) {
			Debug.Log ("B release");
			if (p1Selector.transform.position == p1Pos [0].transform.position && rotationDegree == 0) {
				rotationDegree = 180;
				archer.transform.Rotate (0, rotationDegree, 0);
			}
			if (p1Selector.transform.position == p1Pos [1].transform.position) {
				rotationDegree = 180;
				paladin.transform.Rotate (0, rotationDegree, 0);
			}
			if (p1Selector.transform.position == p1Pos [2].transform.position) {
				rotationDegree = 180;
				reaver.transform.Rotate (0, rotationDegree, 0);
			}
			if (p1Selector.transform.position == p1Pos [3].transform.position) {
				rotationDegree = 180;
				mage.transform.Rotate (0, rotationDegree, 0);
			}
		}
	}

	void Player2Selector ()
	{
		//P2 SELECTOR CODE /////////////////////////////////////////////////////////////////////////////////
		float h2 = Input.GetAxisRaw ("LeftHori_player2");
		if (Input.GetKeyDown (KeyCode.Joystick2Button2) && player2isOut == true) {
			Debug.Log ("P2 pressed X");
			player2isOut = false;
			p2TextToJoin.SetActive (false);
			p2Selector.transform.position = p2Pos [1].transform.position;
			AudioSource.PlayClipAtPoint (join, Camera.main.transform.position);
		}
		if (player2HasChosen == false && player2isOut == false) {
			if (p2SelectorHasMoved) {
				if (h2 < 0.2 && h2 > -0.2)
					p2SelectorHasMoved = false;
			}
			if (p2SelectorHasMoved == false && (h2 > 0.2 || h2 < -0.2)) {
				p2SelectorHasMoved = true;
				if (h2 <= -0.1) {
					p2PosIndex--;
					if (p2PosIndex < 0)
						p2PosIndex = 3;
					p2Selector.transform.position = p2Pos [p2PosIndex].transform.position;
					AudioSource.PlayClipAtPoint (selectorSound, Camera.main.transform.position);
				} else if (h2 >= 0.1) {
					p2PosIndex++;
					if (p2PosIndex > 3)
						p2PosIndex = 0;
					p2Selector.transform.position = p2Pos [p2PosIndex].transform.position;
					AudioSource.PlayClipAtPoint (selectorSound, Camera.main.transform.position);
				}
			}
		}
		
		if (Input.GetKeyDown (KeyCode.Joystick2Button1) && player2HasChosen == false && player2isOut == false) {
			Debug.Log ("A pushed");
			AudioSource.PlayClipAtPoint (chosenSound, Camera.main.transform.position);
			if (p2Selector.transform.position == p2Pos [0].transform.position && !charTaken [0]) {
				particleP2.SetActive (true);
				player2HasChosen = true;
				Debug.Log ("P2 has chosen Archer");
				charTaken [0] = true;

				GetController ("Archer", 2);
			}
			if (p2Selector.transform.position == p2Pos [1].transform.position && !charTaken [1]) {
				Debug.Log ("P2 has chosen Paladin");
				particleP2.SetActive (true);
				player2HasChosen = true;
				charTaken [1] = true;

				GetController ("Paladin", 2);
			}
			if (p2Selector.transform.position == p2Pos [2].transform.position && !charTaken [2]) {
				Debug.Log ("P2 has chosen Reaver");
				particleP2.SetActive (true);
				player2HasChosen = true;
				charTaken [2] = true;

				GetController ("Warrior", 2);
			}
			if (p2Selector.transform.position == p2Pos [3].transform.position && !charTaken [3]) {
				Debug.Log ("P2 has chosen Mage");
				particleP2.SetActive (true);
				player2HasChosen = true;
				charTaken [3] = true;

				GetController ("Mage", 2);
			}
		}
		if (Input.GetKey (KeyCode.Joystick2Button0) && player2HasChosen == false && player2isOut == false) {
			Debug.Log ("B pushed");
			if (p2Selector.transform.position == p2Pos [0].transform.position) {
				archer.transform.Rotate (0, rotationDegree, 0);
				rotationDegree = 0;	
			}
			if (p2Selector.transform.position == p2Pos [1].transform.position) {
				paladin.transform.Rotate (0, rotationDegree, 0);
				rotationDegree = 0;	
			}
			if (p2Selector.transform.position == p2Pos [2].transform.position) {
				reaver.transform.Rotate (0, rotationDegree, 0);
				rotationDegree = 0;	
			}
			if (p2Selector.transform.position == p2Pos [3].transform.position) {
				mage.transform.Rotate (0, rotationDegree, 0);
				rotationDegree = 0;	
			}
		}
		if (Input.GetKeyUp (KeyCode.Joystick2Button0) && player2HasChosen == false && player2isOut == false) {
			Debug.Log ("B release");
			if (p2Selector.transform.position == p2Pos [0].transform.position && rotationDegree == 0) {
				rotationDegree = 180;
				archer.transform.Rotate (0, rotationDegree, 0);
			}
			//archer.transform.Rotate(Vector3.up, 180);
			if (p2Selector.transform.position == p2Pos [1].transform.position) {
				rotationDegree = 180;
				paladin.transform.Rotate (0, rotationDegree, 0);
			}
			if (p2Selector.transform.position == p2Pos [2].transform.position) {
				rotationDegree = 180;
				reaver.transform.Rotate (0, rotationDegree, 0);
			}
			if (p2Selector.transform.position == p2Pos [3].transform.position) {
				rotationDegree = 180;
				mage.transform.Rotate (0, rotationDegree, 0);
			}
		}
	}

	void Player3Selector ()
	{
		//P3 SELECTOR CODE ///////////////////////////////////////////////////////////////////////////////
		
		
		float h3 = Input.GetAxisRaw ("LeftHori_player3");
		if (Input.GetKeyDown (KeyCode.Joystick3Button2) && player3isOut == true) {
			Debug.Log ("P3 pressed X");
			player3isOut = false;
			p3TextToJoin.SetActive (false);
			p3Selector.transform.position = p3Pos [2].transform.position;
			AudioSource.PlayClipAtPoint (join, Camera.main.transform.position);
		}
		if (player3HasChosen == false && player3isOut == false) {
			if (p3SelectorHasMoved) {
				if (h3 < 0.2 && h3 > -0.2)
					p3SelectorHasMoved = false;
			}
			if (p3SelectorHasMoved == false && (h3 > 0.2 || h3 < -0.2)) {
				p3SelectorHasMoved = true;
				if (h3 <= -0.1) {
					p3PosIndex--;
					if (p3PosIndex < 0)
						p3PosIndex = 3;
					p3Selector.transform.position = p3Pos [p3PosIndex].transform.position;
					AudioSource.PlayClipAtPoint (selectorSound, Camera.main.transform.position);
				} else if (h3 >= 0.1) {
					p3PosIndex++;
					if (p3PosIndex > 3)
						p3PosIndex = 0;
					p3Selector.transform.position = p3Pos [p3PosIndex].transform.position;
					AudioSource.PlayClipAtPoint (selectorSound, Camera.main.transform.position);
				}
			}
		}
		
		if (Input.GetKeyDown (KeyCode.Joystick3Button0) && player3HasChosen == false && player3isOut == false) {
			AudioSource.PlayClipAtPoint (chosenSound, Camera.main.transform.position);
			if (p3Selector.transform.position == p3Pos [0].transform.position && !charTaken [0]) {
				particleP3.SetActive (true);
				player3HasChosen = true;
				Debug.Log ("P3 has chosen Archer");
				charTaken [0] = true;

				GetController ("Archer", 3);
			}
			if (p3Selector.transform.position == p3Pos [1].transform.position && !charTaken [1]) {
				Debug.Log ("P3 has chosen Paladin");
				particleP3.SetActive (true);
				player3HasChosen = true;
				charTaken [1] = true;

				GetController ("Paladin", 3);
			}
			if (p3Selector.transform.position == p3Pos [2].transform.position && !charTaken [2]) {
				Debug.Log ("P3 has chosen Reaver");
				particleP3.SetActive (true);
				player3HasChosen = true;
				charTaken [2] = true;

				GetController ("Warrior", 3);
			}
			if (p3Selector.transform.position == p3Pos [3].transform.position && !charTaken [3]) {
				Debug.Log ("P3 has chosen Mage");
				particleP3.SetActive (true);
				player3HasChosen = true;
				charTaken [3] = true;

				GetController ("Mage", 3);
			}
		}

		if (Input.GetKey (KeyCode.Joystick3Button0) && player3HasChosen == false && player3isOut == false) {
			Debug.Log ("B pushed");
			if (p3Selector.transform.position == p3Pos [0].transform.position) {
				archer.transform.Rotate (0, rotationDegree, 0);
				rotationDegree = 0;	
			}
			if (p3Selector.transform.position == p3Pos [1].transform.position) {
				paladin.transform.Rotate (0, rotationDegree, 0);
				rotationDegree = 0;	
			}
			if (p3Selector.transform.position == p3Pos [2].transform.position) {
				reaver.transform.Rotate (0, rotationDegree, 0);
				rotationDegree = 0;	
			}
			if (p3Selector.transform.position == p3Pos [3].transform.position) {
				mage.transform.Rotate (0, rotationDegree, 0);
				rotationDegree = 0;	
			}
		}
		if (Input.GetKeyUp (KeyCode.Joystick3Button0) && player3HasChosen == false && player3isOut == false) {
			Debug.Log ("B release");
			if (p3Selector.transform.position == p3Pos [0].transform.position && rotationDegree == 0) {
				rotationDegree = 180;
				archer.transform.Rotate (0, rotationDegree, 0);
			}
			//archer.transform.Rotate(Vector3.up, 180);
			if (p3Selector.transform.position == p3Pos [1].transform.position) {
				rotationDegree = 180;
				paladin.transform.Rotate (0, rotationDegree, 0);
			}
			if (p3Selector.transform.position == p3Pos [2].transform.position) {
				rotationDegree = 180;
				reaver.transform.Rotate (0, rotationDegree, 0);
			}
			if (p3Selector.transform.position == p3Pos [3].transform.position) {
				rotationDegree = 180;
				mage.transform.Rotate (0, rotationDegree, 0);
			}
		}
	}

	void Player4Selector ()
	{
		//P4 SELECTOR CODE ///////////////////////////////////////////////////////////////////////////////
		
		
		float h4 = Input.GetAxisRaw ("LeftHori_player4");
		if (Input.GetKeyDown (KeyCode.Joystick4Button2) && player4isOut == true) {
			Debug.Log ("P4 pressed X");
			player4isOut = false;
			p4TextToJoin.SetActive (false);
			p4Selector.transform.position = p4Pos [3].transform.position;
			AudioSource.PlayClipAtPoint (join, Camera.main.transform.position);
		}
		if (player4HasChosen == false && player4isOut == false) {
			if (p4SelectorHasMoved) {
				if (h4 < 0.2 && h4 > -0.2)
					p4SelectorHasMoved = false;
			}
			if (p4SelectorHasMoved == false && (h4 > 0.2 || h4 < -0.2)) {
				p4SelectorHasMoved = true;
				if (h4 <= -0.1) {
					p4PosIndex--;
					if (p4PosIndex < 0)
						p4PosIndex = 3;
					p4Selector.transform.position = p4Pos [p4PosIndex].transform.position;
					AudioSource.PlayClipAtPoint (selectorSound, Camera.main.transform.position);
				} else if (h4 >= 0.1) {
					p4PosIndex++;
					if (p4PosIndex > 3)
						p4PosIndex = 0;
					p4Selector.transform.position = p4Pos [p4PosIndex].transform.position;
					AudioSource.PlayClipAtPoint (selectorSound, Camera.main.transform.position);
				}
			}
		}
		
		if (Input.GetKeyDown (KeyCode.Joystick4Button0) && player4HasChosen == false && player4isOut == false) {
			AudioSource.PlayClipAtPoint (chosenSound, Camera.main.transform.position);
			if (p4Selector.transform.position == p4Pos [0].transform.position) {
				particleP4.SetActive (true);
				player4HasChosen = true;
				Debug.Log ("P4 has chosen Archer");

				GetController ("Archer", 4);
			}
			if (p4Selector.transform.position == p4Pos [1].transform.position) {
				Debug.Log ("P4 has chosen Paladin");
				particleP4.SetActive (true);
				player4HasChosen = true;

				GetController ("Paladin", 4);
			}
			if (p4Selector.transform.position == p4Pos [2].transform.position) {
				Debug.Log ("P4 has chosen Reaver");
				particleP4.SetActive (true);
				player4HasChosen = true;
			}
			if (p4Selector.transform.position == p4Pos [3].transform.position) {
				Debug.Log ("P4has chosen Mage");
				particleP4.SetActive (true);
				player4HasChosen = true;

				GetController ("Mage", 4);
			}
		}
		if (Input.GetKey (KeyCode.Joystick4Button0) && player4HasChosen == false && player4isOut == false) {
			Debug.Log ("B pushed");
			if (p4Selector.transform.position == p4Pos [0].transform.position) {
				archer.transform.Rotate (0, rotationDegree, 0);
				rotationDegree = 0;	
			}
			if (p4Selector.transform.position == p4Pos [1].transform.position) {
				paladin.transform.Rotate (0, rotationDegree, 0);
				rotationDegree = 0;	
			}
			if (p4Selector.transform.position == p4Pos [2].transform.position) {
				reaver.transform.Rotate (0, rotationDegree, 0);
				rotationDegree = 0;	
			}
			if (p4Selector.transform.position == p4Pos [3].transform.position) {
				mage.transform.Rotate (0, rotationDegree, 0);
				rotationDegree = 0;	
			}
		}
		if (Input.GetKeyUp (KeyCode.Joystick4Button0) && player4HasChosen == false && player4isOut == false) {
			Debug.Log ("B release");
			if (p4Selector.transform.position == p4Pos [0].transform.position && rotationDegree == 0) {
				rotationDegree = 180;
				archer.transform.Rotate (0, rotationDegree, 0);
			}
		
			if (p4Selector.transform.position == p4Pos [1].transform.position) {
				rotationDegree = 180;
				paladin.transform.Rotate (0, rotationDegree, 0);
			}
			if (p4Selector.transform.position == p4Pos [2].transform.position) {
				rotationDegree = 180;
				reaver.transform.Rotate (0, rotationDegree, 0);
			}
			if (p4Selector.transform.position == p4Pos [3].transform.position) {
				rotationDegree = 180;
				mage.transform.Rotate (0, rotationDegree, 0);
			}
		}
	}

	public void GetController (string playerName, int controllerNum)
	{
		for (int index = 0; index < character.Length; index ++)
		{
			foreach (GameObject p in character)
			{
				if (playerName == p.name)
					p.GetComponent<BasePlayerClass> ().controllerIndex = controllerNum;
			}
			index ++;
		}
	}
}
