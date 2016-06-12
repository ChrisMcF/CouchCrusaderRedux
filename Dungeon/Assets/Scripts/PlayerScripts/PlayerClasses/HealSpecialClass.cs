using UnityEngine;
using System.Collections;
using XboxCtrlrInput;

public class HealSpecialClass : MonoBehaviour {

    //public GameController control;
    public BasePlayerClass mage;
    public CharacterHandler mageHandler;
    public float healTime = 10f;

	Collider[] hits;
	public float radius = 3.5f;

    private bool healPlaced = false;
    private int controllerIndex;
    private GameObject mageObject;

    public XboxAxis xAxis;
    public XboxAxis yAxis;

    void Start()
    {
        mageObject = GameObject.Find("Mage");
        

        transform.parent = mageObject.transform;
        mage = mageObject.GetComponent<BasePlayerClass>();
        mageHandler = mageObject.GetComponent<CharacterHandler>();
        controllerIndex = mage.controllerIndex;
        mageHandler.DisableMovement();
	}

	void Update ()
	{
        if (!healPlaced && XCI.GetButton(XboxButton.LeftBumper, controllerIndex))
        {
            float vert = XCI.GetAxis(yAxis, controllerIndex);

            float distance = Vector3.Distance(transform.position + (new Vector3(0, 0, vert) * 10f * Time.deltaTime), transform.parent.position);
            if (distance < 10f )
            {
                transform.localPosition += new Vector3(0, 0, vert) * 10f * Time.deltaTime;
            }
        }

        if (!XCI.GetButton(XboxButton.LeftBumper, controllerIndex) && !healPlaced)
        {
            transform.parent = null;
            mageHandler.EnableMovement();
            healPlaced = true;
            StartCoroutine("ApplyAOEHeals");
        }
	}

	public IEnumerator ApplyAOEHeals()
	{
        //Debug.Log ("Started heals");
        float counter = 0;
		//Debug.Log (healTime);
		while (counter < healTime) 
		{
            foreach (Player targ in GameController.gameController.players)
            {
                //are they close enough to me?
                if (Vector3.Distance(targ.playerObject.transform.position, transform.position) < radius)
                {
                    //are they within the scan FOV?
                    Vector3 _dir = (targ.playerObject.transform.position - transform.position).normalized;
                    if (Vector3.Dot(transform.forward, _dir) > -1)
                    {
                        targ.playerClass.AdjustCurHealth(mage.actions.special.healthPerSecond * Time.deltaTime);
                    }
                }
            }

            counter += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		healPlaced = false;
		Destroy(gameObject);
        yield return null;
	}

}
