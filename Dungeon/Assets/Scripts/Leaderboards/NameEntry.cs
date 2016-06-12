using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XboxCtrlrInput;

public class NameEntry : MonoBehaviour {

    public Text[] letterBoxes;
    private int boxIndex;
    private int charIndex;
    private float inputCooldown;
    public float inputCooldownTime;

    private string[] charOptions = new string[] {     "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M"
                                                    , "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
                                                    , "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "_"};



    // Use this for initialization
    void Start()
    {
        inputCooldown = inputCooldownTime;
        boxIndex = 0;
        charIndex = 0;
        Debug.Log(charOptions.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (inputCooldown > 0)
            inputCooldown -= Time.deltaTime;

        if (XCI.GetAxis(XboxAxis.LeftStickX) == 1 && inputCooldown <= 0) // Move to box on the right
            ChangeBox(1);

        if (XCI.GetAxis(XboxAxis.LeftStickX) == -1 && inputCooldown <= 0) // Move to box on the left;
            ChangeBox(-1);


        if (XCI.GetAxis(XboxAxis.LeftStickY) == 1 && inputCooldown <= 0)
            ChangeCharacter(1);

        if (XCI.GetAxis(XboxAxis.LeftStickY) == -1 && inputCooldown <= 0)
            ChangeCharacter(-1);

        if (XCI.GetButtonDown(XboxButton.A))
            Debug.Log("Picked Name: " + GetName());
        
    }

    void ChangeBox(int direction) // Right is positive 1, Left is - 1
    {
        letterBoxes[boxIndex].GetComponentInChildren<Image>().enabled = false;
        boxIndex = Mathf.Clamp(boxIndex + direction, 0, 6);
        letterBoxes[boxIndex].GetComponentInChildren<Image>().enabled = true;


        string t = letterBoxes[boxIndex].text;
        for (int i = 0; i < charOptions.Length; i++)
        {   
            if (charOptions[i] == t)
                charIndex = i;
        }

        inputCooldown = inputCooldownTime;
    }

    void ChangeCharacter(int direction)
    {
        charIndex = Mathf.Clamp(charIndex + direction, 0, charOptions.Length -1);
        letterBoxes[boxIndex].text = charOptions[charIndex];
        inputCooldown = inputCooldownTime;
    }

    string GetName()
    {
        string name = "";
        foreach (Text box in letterBoxes)
        {
            name += box.text;
        }
        return name.Replace("_", " ").Trim().TrimEnd();
    }
}
