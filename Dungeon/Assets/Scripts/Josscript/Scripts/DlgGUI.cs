using UnityEngine;
using System.Collections;

public class DlgGUI : DlgManager
{

	public Texture2D textBGTop			;	//you can display different text backgrounds for dialogue that
	public Texture2D textBGBottom		;	//is displayed on the top or bottom part of the screen

	public Texture2D avatarBGTop		;	//optional image to draw behind your avatar image (i.e. a frame)
	public Texture2D avatarBGBottom		;

	public Texture2D highlightBarTop	;	//in case you want your highlight bar to fade one direction or the next
	public Texture2D highlightBarBottom	;

	public GUIStyle thisFont;		//how the text will display

	private Rect avatarRect	;
	private Rect textBGRect	;
	private Rect textPos	;
	private Texture2D textBackground;
	private Texture2D avatarBG		;
	private Texture2D highlightBar	;

	// Use this for initialization
	void Start () {
		Load();
		Save ();
	}

	void Update()
	{
		HandleInput ();

		if(Input.GetKeyDown(KeyCode.F))
		{
			GameObject targ = null;
			float range = 10;
			foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Useable"))
			{
				float dist = Vector3.Distance(transform.position, obj.transform.position);
				if(dist < range)
				{
					targ = obj;
					range = dist;
				}
			}
			
			if(targ != null)
			{
				gameObject.BroadcastMessage("StartDialogue", targ.GetComponent<Interact>().speakToIndex);
				DlgManager.GameKeys.Seti("Interaction", targ.gameObject.GetInstanceID());
			}
		}
	}

	void OnGUI()
	{
		if (HasEnded)
			return;

		//show the player's dialogue in the bottom right corner of the screen
		//and the other character's dialogue in the top left
		if (WhoIsSpeakingID > 3)
		{
			avatarRect	= new Rect(005, 05, 180, 220);
			textBGRect	= new Rect(190f, 05f, Screen.width-200, 110f);
			textPos		= new Rect(210f, 15f, Screen.width-230, 125f);
		
			textBackground	= textBGTop;
			avatarBG		= avatarBGTop;
			highlightBar	= highlightBarTop;
		} else

		{	
			avatarRect	= new Rect(Screen.width - 185, Screen.height - 225, 180f, 220f);
			textBGRect	= new Rect(10, Screen.height - 115, Screen.width - 200, 110f);
			textPos		= new Rect(30, Screen.height - 105, Screen.width - 240f, 125f);
		
			textBackground	= textBGBottom;
			avatarBG		= avatarBGBottom;
			highlightBar	= highlightBarBottom;
		}

		if (avatarBG)
						GUI.Box (avatarRect, "");//GUI.DrawTexture(avatarRect, avatarBG);
		if (textBackground) 
						GUI.Box (textBGRect, "");//GUI.DrawTexture(textBGRect, textBackground);
		else				GUI.Box(textBGRect,"");
							GUI.DrawTexture(avatarRect, _currentAvatar);
	
		GUI.BeginGroup(textPos);
			Rect thisRect = new Rect(0,0,0,0);

			if (CurrentIsChoice)
			{
				for (int x=0; x < CurrentOptionCount; x++)
				{
					thisRect = new Rect(0, (x * 27f) + 5, textPos.width, 30f);
					if (x == Selection )
						GUI.DrawTexture(thisRect, highlightBar);

					if (GUI.Button(thisRect,CurrentOptions(x), thisFont) )
					{
						if (x == Selection )
						{
							Speak();
						} else OptionSelect(x);
					}
				}
			} else
			{
				thisRect = new Rect(0, 0, textPos.width, textPos.height);
				GUI.Label(thisRect, FormattedText , thisFont);
			}
		GUI.EndGroup();	
	}

	void Save()
	{
		Invoke ("Save", 30f);
		DlgManager.GameKeys.Save("Josskeys",true);
	}
	void Load()
	{
		DlgManager.GameKeys.Load("Josskeys");
	}
}