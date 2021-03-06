﻿using System;
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
    [SerializeField]
    private GameObject eventFunctionWhole;
    [SerializeField]
    private GameObject condDropdown;
    [SerializeField]
    private GameObject actnDropdown;

    private AttributeScript[] tempAttributes;

    private void Start()
    {
        // load the saved values.
        LoadGame();
    }

    private AttributeScript[] GetAttributesGridValues()
    {
        int numberOfChildren = attributesGrid.transform.childCount;
        AttributeScript[] attributes = new AttributeScript[numberOfChildren];

        for (int i = 0; i < numberOfChildren; i++)
        {
            Transform currChild = attributesGrid.transform.GetChild(i);
            string currentAttrName = currChild.GetComponent<InputField>().text;
            Transform currAttrType = currChild.Find("DropdownAttrType");
            int currentAttrTypeValue = (int)currAttrType.GetComponentInChildren<Dropdown>().value;
            Transform currDropdownAttrAttachment = currAttrType.Find("DropdownAttrAttachment");
            if (currentAttrTypeValue == AttributeScript.ATTRIBUTE_TYPE_DISCRETE)
            {
                Transform parentOfFields = currDropdownAttrAttachment.GetChild(0).Find("Scroll View").
                    Find("Viewport").Find("Content");
                int numChild = parentOfFields.childCount;
                string[] choices = new string[numChild];
                for (int j = 0; j < numChild; j++)
                {
                    choices[j] = parentOfFields.GetChild(j).GetComponent<InputField>().text;
                }
                attributes[i] = new AttributeScript(currentAttrName, currentAttrTypeValue, choices);
            }
            else if (currentAttrTypeValue == AttributeScript.ATTRIBUTE_TYPE_CONTINUOUS)
            {
                Transform continuousAttrParent = currDropdownAttrAttachment.GetChild(0);
                string assignedStart = continuousAttrParent.Find("AssignedStartField").GetComponent<InputField>().text;
                string assignedEnd = continuousAttrParent.Find("AssignedEndField").GetComponent<InputField>().text;
                string limitStart = continuousAttrParent.Find("LimitStartField").GetComponent<InputField>().text;
                string limitEnd = continuousAttrParent.Find("LimitEndField").GetComponent<InputField>().text;
                attributes[i] = new AttributeScript(currentAttrName, currentAttrTypeValue, 
                    Int32.Parse(assignedStart), Int32.Parse(assignedEnd), 
                    Int32.Parse(limitStart), Int32.Parse(limitEnd));
            }
        }

        return attributes;
    }

    // Create the Save object, and put data to store into it.
    private Save CreateSaveGameObject()
    {
        Save save = new Save();
        save.SaveAttributes(GetAttributesGridValues());

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
        tempAttributes = GetAttributesGridValues();
    }

    public AttributeScript[] GetCachedAttributes()
    {
        return tempAttributes;
    }

    private void BuildTheAttributeGrid(AttributeScript[] attributes)
    {
        foreach (AttributeScript attribute in attributes)
        {
            // create the attribute field 
            GameObject field = Instantiate(attrFieldPrefab, attributesGrid.transform);
            Transform dropdownAttrType = field.transform.Find("DropdownAttrType");
            Transform dropdownAttrAttach = dropdownAttrType.Find("DropdownAttrAttachment");

            // set the text field.
            field.GetComponent<InputField>().text = attribute.GetAttributeName();
            // set the attribute type (dropdown)
            int attrType = attribute.GetAttributeType();
            dropdownAttrType.GetComponent<Dropdown>().value = attrType;
            
            if (attrType == AttributeScript.ATTRIBUTE_TYPE_DISCRETE) // discrete
            {
                Transform dropdownDiscreteAttr = dropdownAttrAttach.GetChild(0);
                string[] choices = attribute.GetAttributeChoiceNames();
                dropdownDiscreteAttr.GetComponent<Dropdown>().value = choices.Length - 2;
                Transform parentOfFields = dropdownDiscreteAttr.Find("Scroll View").Find("Viewport").
                    Find("Content");
                for (int i = 0; i < choices.Length; i++)
                {
                    parentOfFields.GetChild(i).GetComponent<InputField>().text = choices[i];
                }
            } else if (attrType == AttributeScript.ATTRIBUTE_TYPE_CONTINUOUS) // continuous
            {
                Transform continuousAttr = dropdownAttrAttach.GetChild(0);
                Transform assignedStartTransform = continuousAttr.Find("AssignedStartField");
                Transform assignedEndTransform = continuousAttr.Find("AssignedEndField");
                Transform limitStartTransform = continuousAttr.Find("LimitStartField");
                Transform limitEndTransform = continuousAttr.Find("LimitEndField");
                int assignedStartValue, assignedEndValue, limitStartValue, limitEndValue;

                attribute.GetRangesForContinuous(out assignedStartValue, out assignedEndValue, 
                    out limitStartValue, out limitEndValue);

                assignedStartTransform.GetComponent<InputField>().text = assignedStartValue.ToString();
                assignedEndTransform.GetComponent<InputField>().text = assignedEndValue.ToString();
                limitStartTransform.GetComponent<InputField>().text = limitStartValue.ToString();
                limitEndTransform.GetComponent<InputField>().text = limitEndValue.ToString();
            }
        }
        
    }

    private void BuildTheEventFunctionGrid(EventFunctionScript[] eventfuncs)
    {
        for (int i = 0; i < eventfuncs.Length; i++)
        {
            GameObject eventfuncwhole = Instantiate(eventFunctionWhole, eventFunctionsGrid.transform);
            EventFunctionScript currEventFunc = eventfuncs[i];
            string eventName = currEventFunc.GetEventName();

            // populate event names.
            Transform eventFuncRep = eventfuncwhole.transform.Find("EventFunctionRep");
            eventFuncRep.Find("EventFunctionButton").
                GetComponentInChildren<Text>().text = eventName;
            Transform eventFuncInput = eventfuncwhole.transform.Find("EventFunctionInput");
            eventFuncInput.GetComponent<InputField>().text = eventName;

            // populate the message display.
            Transform msgDisplay = eventFuncInput.Find("MessageDisplay");
            msgDisplay.GetComponent<InputField>().text = currEventFunc.GetMessageDisplay();

            // populate conditional statements.
            Transform conditionGrid = eventFuncInput.Find("Scroll View(Condition)").Find("Viewport").Find("ConditionGrid");
            Destroy(conditionGrid.GetChild(0).gameObject); // remove all child (it has only 1 child by default)
            EventFunctionScript.ConditionScript[] conds = currEventFunc.GetAllConditions();
            for (int j = 0; j < conds.Length; j++)
            {
                // retrieve all saved values for population.
                EventFunctionScript.ConditionScript cond = conds[j];
                int dropdownVal = cond.dropdownValue;
                int secDropVal = cond.secondDropdownValue;
                int attributeType = cond.attrType;

                // start populating to real world.
                GameObject conditionalDropdown = Instantiate(condDropdown, conditionGrid);
                conditionalDropdown.GetComponent<Dropdown>().value = dropdownVal;
                Transform condAttachment = conditionalDropdown.transform.Find("ConditionalAttachment");

                if (dropdownVal == 1)  // attribute
                {
                    Transform attrInputCond = condAttachment.GetChild(0);
                    Transform dropdown = attrInputCond.Find("Dropdown");

                    dropdown.GetComponent<Dropdown>().value = secDropVal;
                    dropdown.GetComponent<DropdownAttributeCondition>().type = attributeType;
                    dropdown.GetComponent<DropdownAttributeCondition>().attributeIndexSelected = secDropVal;
                    if (attributeType == AttributeScript.ATTRIBUTE_TYPE_CONTINUOUS)
                    {
                        int x1Value = cond.x1Val;
                        float y1Percent = cond.y1Percent;
                        int x2Value = cond.x2Val;
                        float y2Percent = cond.y2Percent;
                        Transform continuousCase = attrInputCond.Find("ContinuousCase");
                        continuousCase.Find("X(Value)1").GetComponent<InputField>().text = x1Value.ToString();
                        continuousCase.Find("X(Value)2").GetComponent<InputField>().text = x2Value.ToString();
                        continuousCase.Find("Y(Percent)1").GetComponent<InputField>().text = y1Percent.ToString();
                        continuousCase.Find("Y(Percent)2").GetComponent<InputField>().text = y2Percent.ToString();
                    }
                    else if (attributeType == AttributeScript.ATTRIBUTE_TYPE_DISCRETE)
                    {
                        float[] percentages = cond.discretePercents;
                        // only update when data is populated.
                        dropdown.GetComponent<DropdownAttributeCondition>().percentagesFeed = percentages;
                    }
                }
            }

            // populate action statements.
            Transform actionGrid = eventFuncInput.Find("Scroll View(Action)").Find("Viewport").Find("ActionGrid");
            Destroy(actionGrid.GetChild(0).gameObject); // remove all child (it has only 1 child by default)
            EventFunctionScript.ActionScript[] actns = currEventFunc.GetAllActions();
            for (int j = 0; j < actns.Length; j++)
            {
                // retrieve all saved values for population.
                EventFunctionScript.ActionScript actn = actns[j];
                int dropdownVal = actn.dropdownValue;
                int secDropVal = actn.secondDropdownValue;
                int thirdVal = actn.thirdValue;
                int type = actn.attrType;

                // start populating to real world.
                GameObject actionDropdown = Instantiate(actnDropdown, actionGrid);
                actionDropdown.GetComponent<Dropdown>().value = dropdownVal;
                
                Transform actnAttachment = actionDropdown.transform.Find("ActionAttachment");

                if (dropdownVal == 1) // attribute
                {
                    Transform attrInputAction = actnAttachment.GetChild(0);
                    Transform dropdown = attrInputAction.Find("Dropdown");
                    dropdown.GetComponent<Dropdown>().value = secDropVal;
                    dropdown.GetComponent<DropdownAttributeAction>().type = type;
                    dropdown.GetComponent<DropdownAttributeAction>().attributeIndexSelected = secDropVal;
                    dropdown.GetComponent<DropdownAttributeAction>().thirdValue = thirdVal;

                    if (type == AttributeScript.ATTRIBUTE_TYPE_CONTINUOUS)
                    {
                        Transform continuousCase = attrInputAction.Find("ContinuousCase");
                        continuousCase.Find("ChangeField").GetComponent<InputField>().text = thirdVal.ToString();
                    } else if (type == AttributeScript.ATTRIBUTE_TYPE_DISCRETE)
                    {
                        Transform discreteCase = attrInputAction.Find("DiscreteCase");
                        discreteCase.Find("Dropdown").GetComponent<Dropdown>().value = thirdVal;
                    }
                }
            }
            
            // Hide and unhide the necessary so that rep view is shown.
            eventFuncRep.gameObject.SetActive(true);
            eventFuncInput.gameObject.SetActive(false);
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
        FileStream file = File.Create(saveFileLocation);
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Saved");
    }

    public override void LoadGame()
    {
        // Checks to see that the save file exists.
        if (File.Exists(saveFileLocation))
        {
            // Similar to what you did when saving the game, you again create a BinaryFormatter, 
            // only this time you are providing it with a stream of bytes to read instead of write.
            // So you simply pass it the path to the save file. It creates the Save object and closes the FileStream.
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(saveFileLocation, FileMode.Open);
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
