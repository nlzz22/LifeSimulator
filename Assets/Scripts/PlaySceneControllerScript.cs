using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class PlaySceneControllerScript : GameControllerScript
{
    [SerializeField]
    private GameObject attrFieldPrefab;

    private Hashtable attributeTable; // maps attribute name to the game object which controls the value.

    private static string ATTRIBUTE_NAME = "Attribute Text";
    private static string VALUE_NAME = "Value Text";
    
    private void Start()
    {
        attributeTable = new Hashtable();
        // load the saved values.
        LoadGame();
    }

    public void ChangeValue(string attributeName, int amountToChange)
    {
        if (attributeTable.ContainsKey(attributeName))
        {
            GameObject valueController = (GameObject) attributeTable[attributeName];
            string previousValue = valueController.GetComponent<Text>().text;
            int newValue = Int32.Parse(previousValue) + amountToChange;
            valueController.GetComponent<Text>().text = newValue.ToString();
        }
    }

    private void BuildTheAttributeGrid(AttributeScript[] attributes)
    {
        foreach (AttributeScript attribute in attributes)
        {
            // create the attribute field 
            GameObject field = Instantiate(attrFieldPrefab);
            field.transform.SetParent(attributesGrid.transform);
            // set the text field.
            GameObject attributeName = field.transform.Find(ATTRIBUTE_NAME).gameObject;
            string attributeNameText = attribute.GetAttributeName();
            attributeName.GetComponent<Text>().text = attributeNameText;
            // set the default value.
            GameObject attributeValue = field.transform.Find(VALUE_NAME).gameObject;
            attributeValue.GetComponent<Text>().text = attribute.GetDefaultValue().ToString();
            // map to hashtable
            attributeTable.Add(attributeNameText, attributeValue);
        }
        
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

            Debug.Log("Game Loaded");
        }
        else
        {
            Debug.Log("No game saved!");
        }

    }
}
