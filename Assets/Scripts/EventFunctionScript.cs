using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[Serializable]
public class EventFunctionScript
{
    [Serializable]
    public class ConditionScript
    {
        public int dropdownValue;

        public int secondDropdownValue; // for attributes only
        public string textField; // for both attributes and percentage.
        public string endField; // for attributes only.
    }

    [Serializable]
    public class ActionScript
    {
        public int dropdownValue;

        public int secondDropdownValue;
        public string textField;
    }

    private string repText;
    private string messageDisplay;
    private ConditionScript[] conditions;
    private ActionScript[] actions;

    public EventFunctionScript(GameObject eventFunctionWhole)
    {
        Transform rep = eventFunctionWhole.transform.Find("EventFunctionRep");
        GameObject eventFuncBtn = rep.Find("EventFunctionButton").gameObject;

        repText = eventFuncBtn.GetComponentInChildren<Text>().text;
        // event function text follows that of reptext, no need to save again.

        Transform input = eventFunctionWhole.transform.Find("EventFunctionInput");
        Transform msgDisplay = input.Find("MessageDisplay");
        messageDisplay = msgDisplay.GetComponent<InputField>().text;

        // Add conditions.
        GameObject conditionGrid = input.Find("ConditionGrid").gameObject;
        int numChild = conditionGrid.transform.childCount;
        conditions = new ConditionScript[numChild];
        for (int i = 0; i < numChild; i++)
        {
            GameObject conditionDropdown = conditionGrid.transform.GetChild(i).gameObject;
            int dropdownValue = conditionDropdown.GetComponent<Dropdown>().value;
            ConditionScript condition = new ConditionScript();
            condition.dropdownValue = dropdownValue;
            GameObject conditionalAttachment = conditionDropdown.transform.Find("ConditionalAttachment").gameObject;

            if (dropdownValue == 1) // percentage
            {
                string value = conditionalAttachment.transform.GetChild(0).Find("InputField").
                    GetComponent<InputField>().text;
                condition.textField = value;
            } else if (dropdownValue == 2) // attribute
            {
                Transform attrInputCond = conditionalAttachment.transform.GetChild(0);
                Transform secDropdown = attrInputCond.Find("Dropdown");
                int secDropdownValue = secDropdown.GetComponent<Dropdown>().value;
                Transform rangeStart = attrInputCond.Find("RangeStartField");
                Transform rangeEnd = attrInputCond.Find("RangeEndField");
                condition.secondDropdownValue = secDropdownValue;
                condition.textField = rangeStart.GetComponent<InputField>().text;
                condition.endField = rangeEnd.GetComponent<InputField>().text;
            }

            conditions[i] = condition;
        }

        // Add actions
        GameObject actionGrid = input.Find("ActionGrid").gameObject;
        int numOfChild = actionGrid.transform.childCount;
        actions = new ActionScript[numOfChild];
        for (int i = 0; i < numOfChild; i++)
        {
            GameObject actionDropdown = actionGrid.transform.GetChild(i).gameObject;
            int dropdownValue = actionDropdown.GetComponent<Dropdown>().value;
            ActionScript action = new ActionScript();
            action.dropdownValue = dropdownValue;
            GameObject actionAttachment = actionDropdown.transform.Find("ActionAttachment").gameObject;

            if (dropdownValue == 1) // attribute
            {
                Transform attrInputAction = actionAttachment.transform.GetChild(0);
                Transform secDropdown = attrInputAction.Find("Dropdown");
                int secDropdownValue = secDropdown.GetComponent<Dropdown>().value;
                Transform text = attrInputAction.Find("ChangeField");
                action.secondDropdownValue = secDropdownValue;
                action.textField = text.GetComponent<InputField>().text;
            }
            else if (dropdownValue == 2) // status: not yet implemented.
            {

            }

            actions[i] = action;
        }

    }

    public string GetEventName()
    {
        return repText;
    }

    public string GetMessageDisplay()
    {
        return messageDisplay;
    }

    public ConditionScript[] GetAllConditions()
    {
        return conditions;
    }

    public ActionScript[] GetAllActions()
    {
        return actions;
    }
    
}
