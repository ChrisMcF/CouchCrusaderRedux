using UnityEngine;
using System.Xml;
using System.Collections;
using System;

public class LeaderboardScore
{
    private string playerName;
    private int totalTime;

	public LeaderboardScore(XmlNode scoreNode)
    {
        TotalTime =  10000 - Convert.ToInt32(scoreNode.SelectSingleNode("seconds").InnerText);
        PlayerName = scoreNode.SelectSingleNode("name").InnerText;
    }

    public string PlayerName
    {
        get { return playerName; }
        set
        {
            playerName = value;
        }
    }

    public int TotalTime
    {
        get
        {
            return totalTime;
        }

        set
        {
            totalTime = value;
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
