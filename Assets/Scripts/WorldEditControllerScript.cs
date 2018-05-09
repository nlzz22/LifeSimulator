using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class WorldEditControllerScript : GameControllerScript
{
    [SerializeField]
    private GameObject attrFieldPrefab;

    private List<string> tempAttributes;

    private void Start()
    {
        // load the saved values.
        LoadGame();
    }

    // Create the Save object, and put data to store into it.
    private Save CreateSaveGameObject()
    {
        Save save = new Save();
        List<AttributeScript> attributes = new List<AttributeScript>();

        int numberOfChildren = attributesGrid.transform.childCount;
        for (int i = 0; i < numberOfChildren; i++)
        {
            GameObject currChild = attributesGrid.transform.GetChild(i).gameObject;
            string currentAttrName = currChild.GetComponent<InputField>().text;
            int currentAttrValue = (int)currChild.GetComponentInChildren<Slider>().value;

            // if attribute is not empty.
            if (currentAttrName.Trim() != "")
            {
                attributes.Add(new AttributeScript(currentAttrName, currentAttrValue));
            }
        }

        save.SaveAttributes(attributes.ToArray());

        List<EventFunctionScript> eventFuncs = new List<EventFunctionScript>();
        int numChild = eventFunctionsGrid.transform.childCount;
        for (int i = 0; i < numChild; i++)
        {

            GameObject eventFuncWhole = eventFunctionsGrid.transform.GetChild(i).gameObject;
            EventFunctionScript currEventFunction = new EventFunctionScript(eventFuncWhole);
            eventFuncs.Add(currEventFunction);
        }

        save.SaveEventFunctions(eventFuncs.ToArray());

        return save;
    }

    public void CacheAttributes()
    {
        tempAttributes = new List<string>();

        int numberOfChildren = attributesGrid.transform.childCount;
        for (int i = 0; i < numberOfChildren; i++)
        {
            GameObject currChild = attributesGrid.transform.GetChild(i).gameObject;
            string currentAttrName = currChild.GetComponent<InputField>().text;

            // if attribute is not empty.
            if (currentAttrName.Trim() != "")
            {
                tempAttributes.Add(currentAttrName);
            }
        }
    }

    public List<string> GetCachedAttributes()
    {
        return tempAttributes;
    }

    private void BuildTheAttributeGrid(AttributeScript[] attributes)
    {
        foreach (AttributeScript attribute in attributes)
        {
            // create the attribute field 
            GameObject field = Instantiate(attrFieldPrefab);
            field.transform.SetParent(attributesGrid.transform);
            // set the text field.
            field.GetComponent<InputField>().text = attribute.GetAttributeName();
            // set the value for slider.
            field.GetComponentInChildren<Slider>().value = attribute.GetDefaultValue();
        }
        
    }

    private void BuildTheEventFunctionGrid(EventFunctionScript[] eventfuncs)
    {
        for (int i = 0; i < eventfuncs.Length; i++)
        {
            EventFunctionScript currEventFunc = eventfuncs[i];
            string eventName = currEventFunc.GetEventName();
            Debug.Log("event name is : " + eventName);
            EventFunctionScript.ConditionScript[] conds = currEventFunc.GetAllConditions();
            for (int j = 0; j < conds.Length; j++)
            {
                EventFunctionScript.ConditionScript cond = conds[j];
                int dropdownVal = cond.dropdownValue;
                int secDropVal = cond.secondDropdownValue;
                string text = cond.textField;
                string endField = cond.endField;
                Debug.Log("drop : " + dropdownVal + " second drop : " + secDropVal + " text : " + text + " end : " + endField);
            }
            
        }
    }

    public void SaveGame()
    {
        // Create a Save instance with all the data for the current session saved into it.
        Save save = CreateSaveGameObject();

        // Create a BinaryFormatter and a FileStream by passing a path for the Save instance to be saved to. 
        // It serializes the data (into bytes) and writes it to disk and closes the FileStream. 
        // There will now be a file named worldeditor.data on your computer.
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + saveFileName);
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Saved");
    }

    public override void LoadGame()
    {
        // Checks to see that the save file exists.
        if (File.Exists(Application.persistentDataPath + saveFileName))
        {
            // Similar to what you did when saving the game, you again create a BinaryFormatter, 
            // only this time you are providing it with a stream of bytes to read instead of write.
            // So you simply pass it the path to the save file. It creates the Save object and closes the FileStream.
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + saveFileName, FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            // Load save information into the game state.
            BuildTheAttributeGrid(save.GetAttributes());
            BuildTheEventFunctionGrid(save.GetEventFunctions());

            Debug.Log("Game Loaded");
        }
        else
        {
            Debug.Log("No game saved!");
        }

    }
}
