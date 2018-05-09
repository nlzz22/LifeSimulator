using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour {
    // Attributes
    [SerializeField]
    private GameObject attributeButton;
    [SerializeField]
    private GameObject attributeCanvas;
    [SerializeField]
    private GameObject attributeGrid;
    [SerializeField]
    private GameObject attributeField;

    // Event functions
    [SerializeField]
    private GameObject eventFunctionButton;
    [SerializeField]
    private GameObject eventFunctionCanvas;
    [SerializeField]
    private GameObject eventFunctionGrid;
    [SerializeField]
    private GameObject eventFunctionField;

    // Extras
    [SerializeField]
    private GameObject conditionDropdown;
    [SerializeField]
    private GameObject actionDropdown;

    private WorldEditControllerScript gameScript;

    private WorldEditControllerScript FindGameScript()
    {
        if (gameScript == null)
        {
            GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
            gameScript = gameController.GetComponent<WorldEditControllerScript>();
            return gameScript;
        } else
        {
            return gameScript;
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void WorldEditor()
    {
        SceneManager.LoadScene("WorldEditor");
    }

    public void EventFunctionTab()
    {
        // save the attributes for another canvas to use.
        FindGameScript().CacheAttributes();
        // change canvas
        attributeCanvas.SetActive(false);
        eventFunctionCanvas.SetActive(true);
        // change buttons
        attributeButton.SetActive(true);
        eventFunctionButton.SetActive(false);
    }

    public void AttributeTab()
    {
        // change canvas
        attributeCanvas.SetActive(true);
        eventFunctionCanvas.SetActive(false);
        // change buttons
        attributeButton.SetActive(false);
        eventFunctionButton.SetActive(true);
    }
    

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void AddAttribute()
    {
        if (attributeGrid != null)
        {
            Instantiate(attributeField, attributeGrid.transform);
        }
    }

    public void AddEventFunction()
    {
        if (eventFunctionGrid != null)
        {
            Instantiate(eventFunctionField, eventFunctionGrid.transform);
        }
    }

    public void AddConditionDropdown()
    {
        GameObject addButton = EventSystem.current.currentSelectedGameObject;
        Transform conditionGrid = addButton.transform.parent.Find("ConditionGrid");
        
        Instantiate(conditionDropdown, conditionGrid);
    }

    public void AddActionDropdown()
    {
        GameObject addButton = EventSystem.current.currentSelectedGameObject;
        Transform actionGrid = addButton.transform.parent.Find("ActionGrid");

        Instantiate(actionDropdown, actionGrid);
    }

    public void SaveAndExit()
    {
        FindGameScript().SaveGame();
        MainMenu();
    }

    public void RemoveItem()
    {
        GameObject cancelButton = EventSystem.current.currentSelectedGameObject;
        if (cancelButton != null)
        {
            GameObject.Destroy(cancelButton.transform.parent.gameObject);
        }
        else
        {
            Debug.Log("BUG: RemoveItem() at ButtonScript has currentSelectedGameObject as null");
        }
    }
}
