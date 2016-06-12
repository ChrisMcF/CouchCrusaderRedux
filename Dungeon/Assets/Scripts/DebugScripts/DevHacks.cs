using UnityEngine;
using System.Collections.Generic;

public class DevHacks : MonoBehaviour {

    
	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            foreach (Player p in GameController.gameController.players)
            {
                p.playerClass.stats.specialAmount = 100;
            }
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            foreach (Player p in GameController.gameController.players)
            {
                p.playerClass.stats.curHealth = p.playerClass.stats.maxHealth;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            foreach (Player p in GameController.gameController.players)
            {
                p.playerClass.SendMessage("AdjustCurHealth", -9999);
            }
        }
    }
}
