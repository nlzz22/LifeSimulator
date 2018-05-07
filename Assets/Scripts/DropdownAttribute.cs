using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownAttribute : MonoBehaviour {

    private Dropdown dropdownself;
    private WorldEditControllerScript gameScript;

    private WorldEditControllerScript FindGameScript()
    {
        if (gameScript == null)
        {
            GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
            gameScript = gameController.GetComponent<WorldEditControllerScript>();
            return gameScript;
        }
        else
        {
            return gameScript;
        }
    }

    private void PopulateDropdown()
    {
        List<string> attributes = FindGameScript().GetCachedAttributes();
        if (attributes != null)
        {
            dropdownself.GetComponent<Dropdown>().AddOptions(attributes);
        }
        
    }

    // Use this for initialization
    void Start () {
        dropdownself = GetComponent<Dropdown>();
        PopulateDropdown();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
