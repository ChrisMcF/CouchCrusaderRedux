using UnityEngine;
using System.Collections;
using XboxCtrlrInput;
using XInputDotNetPure;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {


    private Image panelImage;

	// Use this for initialization
	void Start ()
    {
        panelImage = GetComponent<Image>();
        Color tempColor = panelImage.color;
        tempColor.a = 0f;
        panelImage.color = tempColor;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (XCI.GetButtonDown(XboxButton.Start, 1) && !GameController.gameController.gamePaused)
        {
            Time.timeScale = 0f;
            GameController.gameController.gamePaused = true;
            Color tempColor = panelImage.color;
            tempColor.a = 0.5f;
            panelImage.color = tempColor;
        }
        else if (XCI.GetButtonDown(XboxButton.Start, 1) && GameController.gameController.gamePaused)
        {
            Time.timeScale = 1f;
            GameController.gameController.gamePaused = false;
            Color tempColor = panelImage.color;
            tempColor.a = 0f;
            panelImage.color = tempColor;
        }
    }
}
