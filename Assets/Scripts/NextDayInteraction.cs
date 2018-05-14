using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextDayInteraction : MonoBehaviour {
    private PlaySceneControllerScript gameController;
    private Transform nextDayButton;
    private bool isAutomaticOn;

	// Use this for initialization
	void Start () {
        gameController = GameObject.Find("GameController").GetComponent<PlaySceneControllerScript>();
        nextDayButton = transform.Find("NextDayButton");
        isAutomaticOn = false;
    }
	
	public void TriggerAutomaticNextDay()
    {
        isAutomaticOn = !isAutomaticOn;
        if (isAutomaticOn)
        {
            nextDayButton.gameObject.SetActive(false); // disable next day button if automatic is on.
            gameController.SetAutomaticNextDay(true);
        } else
        {
            nextDayButton.gameObject.SetActive(true); // make next day button active for press.
            gameController.SetAutomaticNextDay(false);
        }
    }

    public void GoToNextDay()
    {
        gameController.PassOneCounter();
    }
}
