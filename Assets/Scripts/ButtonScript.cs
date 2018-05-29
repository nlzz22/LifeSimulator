using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    private GameObject eventFunctionField;
    private GameObject eventFunctionGrid;

    // Extras
    [SerializeField]
    private GameObject conditionDropdown;
    [SerializeField]
    private GameObject actionDropdown;
    private GameObject mainCanvas;

    private WorldEditControllerScript gameScript;
    public static GameObject addEventFuncButton;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "WorldEditor")
        {
            FindMainCanvas();
        }
        
    }

    private GameObject FindAddEventFuncBtn()
    {
        if (addEventFuncButton == null)
        {
            addEventFuncButton = GameObject.Find("Event Function Canvas").transform.Find("AddButton").gameObject;
        }

        return addEventFuncButton;
    }

    private GameObject FindMainCanvas()
    {
        if (mainCanvas == null)
        {
            GameObject canvas = GameObject.Find("Main Canvas");
            if (canvas == null)
            {
                canvas = GameObject.Find("ButtonActions").GetComponent<ButtonScript>().GetMainCanvas();
            }
            mainCanvas = canvas;
        }

        return mainCanvas;
    }

    public GameObject GetMainCanvas()
    {
        return FindMainCanvas();
    }

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

    private GameObject FindEventFunctionsGrid()
    {
        if (eventFunctionGrid == null)
        {
            eventFunctionGrid = GameObject.Find("Content (Functions Grid)");
            if (eventFunctionGrid == null)
            {
                eventFunctionGrid =
                    eventFunctionCanvas.transform.
                    Find("Scroll View").Find("Viewport").Find("Content (Functions Grid)").gameObject;
            }
        }

        return eventFunctionGrid;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void WorldEditor()
    {
        SceneManager.LoadScene("WorldEditor");
    }

    public void QuitGame()
    {
        Application.Quit();
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

    private void toggleChildActive(GameObject parent, bool setToActive)
    {
        int numChild = parent.transform.childCount;
        for (int i = 0; i < numChild; i++)
        {
            GameObject child = parent.transform.GetChild(i).gameObject;
            child.SetActive(setToActive);
        }
    }

    public void AddEventFunction()
    {
        FindEventFunctionsGrid();

        if (addEventFuncButton == null)
        {
            addEventFuncButton = EventSystem.current.currentSelectedGameObject;
        }
        addEventFuncButton.SetActive(false); // hides the button

        FindMainCanvas().SetActive(false); // hides the main canvas.

        // Hides all children first.
        toggleChildActive(eventFunctionGrid, false);

        // Creates the new event function field for editing.
        Instantiate(eventFunctionField, eventFunctionGrid.transform);

    }

    public void AddConditionDropdown()
    {
        GameObject addButton = EventSystem.current.currentSelectedGameObject;
        Transform conditionGrid = addButton.transform.parent.Find("Scroll View(Condition)").
            Find("Viewport").Find("ConditionGrid");
        
        Instantiate(conditionDropdown, conditionGrid);
    }

    public void AddActionDropdown()
    {
        GameObject addButton = EventSystem.current.currentSelectedGameObject;
        Transform actionGrid = addButton.transform.parent.Find("Scroll View(Action)").
            Find("Viewport").Find("ActionGrid");

        Instantiate(actionDropdown, actionGrid);
    }

    public void DoneEventFunction()
    {
        GameObject doneButton = EventSystem.current.currentSelectedGameObject;
        // Validate abit.

        GameObject eventFunctionInput = doneButton.transform.parent.gameObject;
        string eventName = eventFunctionInput.GetComponent<InputField>().text;
        GameObject feedbackObj = doneButton.transform.Find("Feedback").gameObject;

        // if no event name
        if (eventName.Trim() == "")
        {
            feedbackObj.GetComponent<Text>().text = "Please enter an event name.";
        } else
        {
            feedbackObj.GetComponent<Text>().text = ""; // no error
            GameObject eventFunctionWhole = eventFunctionInput.transform.parent.gameObject;
            GameObject eventFunctionRep = eventFunctionWhole.transform.Find("EventFunctionRep").gameObject;

            // Switch from edit one item to overview screen.
            eventFunctionInput.SetActive(false);
            eventFunctionRep.SetActive(true);

            GameObject eventFunctionButton = eventFunctionRep.transform.Find("EventFunctionButton").gameObject;
            // Update overview button with event name.
            eventFunctionButton.GetComponentInChildren<Text>().text = eventName;
            // Unhide the "add" event function button.
            FindAddEventFuncBtn().SetActive(true);
            // Unhide the main canvas
            FindMainCanvas().SetActive(true);
            // Unhides all child of grid.
            toggleChildActive(FindEventFunctionsGrid(), true);
        }

    }

    private void LoadAllEventFunctionInput()
    {
        FindGameScript().CacheAttributes();
        FindEventFunctionsGrid();
        int numChild = eventFunctionGrid.transform.childCount;
        eventFunctionCanvas.SetActive(true);
        for (int i = 0; i < numChild; i++)
        {
            Transform eventFuncWholeChild = eventFunctionGrid.transform.GetChild(i);
            eventFuncWholeChild.Find("EventFunctionInput").gameObject.SetActive(true);
        }
    }

    public void SaveAndExit()
    {
        transform.GetChild(0).gameObject.SetActive(true); // load saving screen
        LoadAllEventFunctionInput();
        StartCoroutine(Save());
    }

    private IEnumerator Save()
    {
        yield return new WaitForSeconds(1);
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

    public void RemoveItemTwoLevels()
    {
        GameObject cancelButton = EventSystem.current.currentSelectedGameObject;
        if (cancelButton != null)
        {
            GameObject firstParent = cancelButton.transform.parent.gameObject;
            if (firstParent == null)
            {
                Debug.Log("first parent of RemoveItemTwoLevels() at ButtonScript is null.");
            }
            else
            {
                GameObject secondParent = firstParent.transform.parent.gameObject;
                if (secondParent == null)
                {
                    Debug.Log("second parent of RemoveItemTwoLevels() at ButtonScript is null.");
                }
                else
                {
                    Destroy(secondParent);
                }
            }
        }
        else
        {
            Debug.Log("BUG: RemoveItem() at ButtonScript has currentSelectedGameObject as null");
        }
    }
}
