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

        public int secondDropdownValue; // attribute

        // Attribute only.
        public int attrType;
        // For continuous attributes.
        public int x1Val;
        public float y1Percent;
        public int x2Val;
        public float y2Percent;
        // for discrete attributes.
        public float[] discretePercents;
    }

    [Serializable]
    public class ActionScript
    {
        public int dropdownValue;

        public int secondDropdownValue;
        public int thirdValue;

        // Attribute only.
        public int attrType;
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
        GameObject conditionGrid = input.Find("Scroll View(Condition)").
            Find("Viewport").Find("ConditionGrid").gameObject;
        int numChild = conditionGrid.transform.childCount;
        conditions = new ConditionScript[numChild];
        for (int i = 0; i < numChild; i++)
        {
            GameObject conditionDropdown = conditionGrid.transform.GetChild(i).gameObject;
            int dropdownValue = conditionDropdown.GetComponent<Dropdown>().value;
            ConditionScript condition = new ConditionScript();
            condition.dropdownValue = dropdownValue;
            GameObject conditionalAttachment = conditionDropdown.transform.Find("ConditionalAttachment").gameObject;

            if (dropdownValue == 1) // attribute
            {
                Transform attrInputCond = conditionalAttachment.transform.GetChild(0);
                Transform secDropdown = attrInputCond.Find("Dropdown");
                int secDropdownValue = secDropdown.GetComponent<Dropdown>().value;
                int type = secDropdown.GetComponent<DropdownAttributeCondition>().type;

                if (type == AttributeScript.ATTRIBUTE_TYPE_DISCRETE)
                {
                    GameObject discreteCase = attrInputCond.Find("DiscreteCase").gameObject;
                    Transform contentParent = discreteCase.transform.Find("Scroll View").Find("Viewport").Find("Content");
                    int discreteChild = contentParent.childCount;
                    condition.discretePercents = new float[discreteChild];
                    for (int j = 0; j < discreteChild; j++)
                    {
                        condition.discretePercents[j] = Convert.ToSingle(contentParent.GetChild(j).Find("InputField").
                            GetComponent<InputField>().text);
                    }
                } else if (type == AttributeScript.ATTRIBUTE_TYPE_CONTINUOUS)
                {
                    Transform continuousCase = attrInputCond.Find("ContinuousCase");
                    string xValue1 = continuousCase.Find("X(Value)1").GetComponent<InputField>().text;
                    string xValue2 = continuousCase.Find("X(Value)2").GetComponent<InputField>().text;
                    string yPercent1 = continuousCase.Find("Y(Percent)1").GetComponent<InputField>().text;
                    string yPercent2 = continuousCase.Find("Y(Percent)2").GetComponent<InputField>().text;
                    condition.x1Val = Int32.Parse(xValue1);
                    condition.x2Val = Int32.Parse(xValue2);
                    condition.y1Percent = Convert.ToSingle(yPercent1);
                    condition.y2Percent = Convert.ToSingle(yPercent2);
                }

                condition.secondDropdownValue = secDropdownValue;
                condition.attrType = type;
            }

            conditions[i] = condition;
        }

        // Add actions
        GameObject actionGrid = input.Find("Scroll View(Action)").Find("Viewport").
            Find("ActionGrid").gameObject;
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
                action.secondDropdownValue = secDropdownValue;
                int type = secDropdown.GetComponent<DropdownAttributeAction>().type;

                if (type == AttributeScript.ATTRIBUTE_TYPE_CONTINUOUS)
                {
                    Transform changeField = attrInputAction.Find("ContinuousCase").Find("ChangeField");
                    int change = Int32.Parse(changeField.GetComponent<InputField>().text);
                    action.thirdValue = change;
                } else if (type == AttributeScript.ATTRIBUTE_TYPE_DISCRETE)
                {
                    Transform changeField = attrInputAction.Find("DiscreteCase").Find("Dropdown");
                    int changeAttr = changeField.GetComponent<Dropdown>().value;
                    action.thirdValue = changeAttr;
                }

                action.attrType = type;
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
