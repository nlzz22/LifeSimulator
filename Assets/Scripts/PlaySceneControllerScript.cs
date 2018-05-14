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
    [SerializeField]
    private GameObject dayTextObject;
    [SerializeField]
    private GameObject feedbackChoiceFuncObject;
    [SerializeField]
    private GameObject feedbackEventFuncObject;
    [SerializeField]
    private int numberOfEventsPerDay; // number of events that occur naturally in one day.
    [SerializeField]
    private float timeBetweenEvents; // time in seconds between 2 consecutive events happening.

    private int dayNumber; // signify which day it is, as a number.
    private int dayCounter; // a counter to tell when is it time to change to a new day.
    private bool isAutomaticNextDay; // true: next day triggered by time. false: next day triggered by button press.

    private Hashtable attributeTable; // maps attribute name to the game object which controls the value.
    private List<string> listAttributes; // maps number with attribute name
    private EventFunctionScript[] eventFunctions; // all event functions stored here.

    private static string ATTRIBUTE_NAME = "Attribute Text";
    private static string VALUE_NAME = "Value Text";
    private static string DAY_TEXT = "Day ";

    private static int CONDITION_PERCENTAGE = 1;
    private static int CONDITION_ATTRIBUTE = 2;
    private static int ACTION_ATTRIBUTE = 1;
    private static int ACTION_STATUS = 2;

    public class AttributeDisplay
    {
        public Transform attributeValueController;
        public int type;
        public int limitStart;
        public int limitEnd;
        public string[] choices;

        public AttributeDisplay(Transform controller, int type, int start, int end)
        {
            attributeValueController = controller;
            this.type = type;
            limitStart = start;
            limitEnd = end;
        }

        public AttributeDisplay(Transform controller, int type, string[] choices)
        {
            attributeValueController = controller;
            this.type = type;
            this.choices = choices;
        }
    }
    
    private void Start()
    {
        dayNumber = 1;
        dayCounter = 0;
        isAutomaticNextDay = false;
        attributeTable = new Hashtable();
        listAttributes = new List<string>();
        // load the saved values.
        LoadGame();
    }

    public void PassOneCounter()
    {
        dayCounter++;
        CheckEventFunctions();

        // if it is time for a new day.
        if (dayCounter >= numberOfEventsPerDay)
        {
            dayCounter = 0;
            dayNumber++;
            dayTextObject.GetComponent<Text>().text = DAY_TEXT + dayNumber;
        }
    }

    private void ActivateAutomaticDayRun()
    {
        StartCoroutine(AutomaticRunDay());
    }

    public void SetAutomaticNextDay(bool isAutomatic)
    {
        isAutomaticNextDay = isAutomatic;
        if (isAutomaticNextDay)
        {
            ActivateAutomaticDayRun();
        }
    } 

    private IEnumerator AutomaticRunDay()
    {
        yield return new WaitForSeconds(timeBetweenEvents);

        // confirms that next day should be run automatically.
        if (isAutomaticNextDay)
        {
            PassOneCounter();
            StartCoroutine(AutomaticRunDay());
        }
    }

    private bool WillOccurWithChance(int percentage)
    {
        return (UnityEngine.Random.Range(1, 100) <= percentage);
    }

    private int GetCurrentAttributeValue(string attributeName)
    {
        Transform valueController = ((AttributeDisplay)attributeTable[attributeName]).attributeValueController;
        return Int32.Parse(valueController.GetComponent<Text>().text);
    }

    public void UpdateEventFunction(string feedback, bool toAppend)
    {
        if (toAppend)
        {
            feedbackEventFuncObject.GetComponent<Text>().text += feedback + "\n";
        } else
        {
            feedbackEventFuncObject.GetComponent<Text>().text = feedback + "\n";
        }
        
    }

    private void CheckEventFunctions()
    {
        bool hasAlreadyUpdatedToday = false;
        foreach(EventFunctionScript eventFunction in eventFunctions)
        {
            bool isAllConditionsSatisfied = true;
            string eventName = eventFunction.GetEventName();
            EventFunctionScript.ConditionScript[] conditions = eventFunction.GetAllConditions();
            foreach (EventFunctionScript.ConditionScript condition in conditions)
            {
                int conditionType = condition.dropdownValue;
                if (conditionType == CONDITION_PERCENTAGE)
                {
                    string percentageChanceToOccur = condition.textField;
                    int percentageChance = Int32.Parse(percentageChanceToOccur);
                    if (!WillOccurWithChance(percentageChance))
                    {
                        isAllConditionsSatisfied = false;
                        break;
                    }
                } else if (conditionType == CONDITION_ATTRIBUTE)
                {
                    int attributeIndex = condition.secondDropdownValue;
                    string attributeName = listAttributes[attributeIndex];
                    string attrStartRangeString = condition.textField;
                    string attrEndRangeString = condition.endField;
                    int attrStartRange = Int32.Parse(attrStartRangeString);
                    int attrEndRange = Int32.Parse(attrEndRangeString);
                    int currentAttributeValue = GetCurrentAttributeValue(attributeName);
                    if (currentAttributeValue < attrStartRange || currentAttributeValue > attrEndRange)
                    {
                        isAllConditionsSatisfied = false;
                        break;
                    }
                }
            }

            if (isAllConditionsSatisfied)
            {
                // Perform all actions.
                EventFunctionScript.ActionScript[] actions = eventFunction.GetAllActions();
                foreach (EventFunctionScript.ActionScript action in actions)
                {
                    int actionType = action.dropdownValue;
                    if (actionType == ACTION_ATTRIBUTE)
                    {
                        int attributeIndex = action.secondDropdownValue;
                        string attributeName = listAttributes[attributeIndex];
                        int amountToChange = Int32.Parse(action.textField);
                        ChangeValue(attributeName, amountToChange);
                    } else if (actionType == ACTION_STATUS)
                    {
                        // not yet implemented.
                    }
                }

                if (hasAlreadyUpdatedToday)
                {
                    UpdateEventFunction(eventName + " : " + eventFunction.GetMessageDisplay(), true);
                } else
                {
                    hasAlreadyUpdatedToday = true;
                    UpdateEventFunction(eventName + " : " + eventFunction.GetMessageDisplay(), false);
                }
                
            }
        }
    }

    public void ChangeValue(string attributeName, int amountToChange)
    {
        if (attributeTable.ContainsKey(attributeName))
        {
            Transform valueController = ((AttributeDisplay)attributeTable[attributeName]).attributeValueController;
            string previousValue = valueController.GetComponent<Text>().text;
            int newValue = Int32.Parse(previousValue) + amountToChange;

            int limitStart = ((AttributeDisplay)attributeTable[attributeName]).limitStart;
            int limitEnd = ((AttributeDisplay)attributeTable[attributeName]).limitEnd;

            int finalValue = (int) Mathf.Clamp(newValue, limitStart, limitEnd);

            valueController.GetComponent<Text>().text = finalValue.ToString();
        }
    }

    public void ChangeDiscreteValue(string attributeName, bool canRepeat)
    {
        if (attributeTable.ContainsKey(attributeName))
        {
            Transform valueController = ((AttributeDisplay)attributeTable[attributeName]).attributeValueController;
            string[] choices = ((AttributeDisplay)attributeTable[attributeName]).choices;
            
            int numChoices = choices.Length;
            int randomChoice = UnityEngine.Random.Range(0, numChoices);

            if (canRepeat)
            {
                valueController.GetComponent<Text>().text = choices[randomChoice];
            }
            else
            {
                string previousValue = valueController.GetComponent<Text>().text;
                string newValue = choices[randomChoice];
                if (previousValue == newValue)
                {
                    newValue = choices[(randomChoice + 1) % numChoices];
                }

                valueController.GetComponent<Text>().text = newValue;
            }
        }
    }

    private void BuildTheAttributeGrid(AttributeScript[] attributes)
    {
        foreach (AttributeScript attribute in attributes)
        {
            // create the attribute field 
            GameObject field = Instantiate(attrFieldPrefab, attributesGrid.transform);
            // set the text field.
            Transform attributeName = field.transform.Find(ATTRIBUTE_NAME);
            string attributeNameText = attribute.GetAttributeName();
            attributeName.GetComponent<Text>().text = attributeNameText;
            // set the default value.
            Transform attributeValue = field.transform.Find(VALUE_NAME);
            int attrType = attribute.GetAttributeType();
            if (attrType == 1) // discrete
            {
                string[] choices = attribute.GetAttributeChoiceNames();
                int numChoices = choices.Length;
                attributeValue.GetComponent<Text>().text = choices[UnityEngine.Random.Range(0, numChoices)];
                // map to hashtable
                attributeTable.Add(attributeNameText, new AttributeDisplay(attributeValue, attrType, choices));
            } else if (attrType == 2) // continuous
            {
                int startAssign, endAssign, startLimit, endLimit;
                attribute.GetRangesForContinuous(out startAssign, out endAssign, out startLimit, out endLimit);
                attributeValue.GetComponent<Text>().text = UnityEngine.Random.Range(startAssign, endAssign + 1).ToString();
                // map to hashtable
                attributeTable.Add(attributeNameText, new AttributeDisplay(attributeValue, attrType, startLimit, endLimit));
            }

            // map to list
            listAttributes.Add(attributeNameText);
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
            eventFunctions = save.GetEventFunctions();

            Debug.Log("Game Loaded");
        }
        else
        {
            Debug.Log("No game saved!");
        }

    }
}
