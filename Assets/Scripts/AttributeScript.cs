using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AttributeScript
{
    private string attributeName;
    private int attributeType; // 1 discrete, 2 continuous

    // for discrete attributes.
    private string[] attributeChoiceNames;

    // for continuous attributes;
    private int assignedStartField;
    private int assignedEndField;
    private int limitStartField;
    private int limitEndField;

    // For discrete attr
    public AttributeScript(string attributeName, int attributeType, string[] attrChoicesNames)
    {
        this.attributeName = attributeName;
        this.attributeType = attributeType;
        if (attributeType != 1)
        {
            Debug.Log("error in initialising attribute script, non discrete type found.");
        }
        attributeChoiceNames = attrChoicesNames;
    }

    // For continuous attr
    public AttributeScript(string attributeName, int attributeType, int assignedStartField,
        int assignedEndField, int limitStartField, int limitEndField)
    {
        this.attributeName = attributeName;
        this.attributeType = attributeType;
        if (attributeType != 2)
        {
            Debug.Log("error in initialising attribute script, non continuous type found.");
        }
        this.assignedStartField = assignedStartField;
        this.assignedEndField = assignedEndField;
        this.limitStartField = limitStartField;
        this.limitEndField = limitEndField;
}

    public string GetAttributeName()
    {
        return attributeName;
    }

    public int GetAttributeType()
    {
        return attributeType;
    }

    public string[] GetAttributeChoiceNames()
    {
        return attributeChoiceNames;
    }

    public void GetRangesForContinuous(out int assignedStart, out int assignedEnd, out int limitStart,
        out int limitEnd)
    {
        assignedStart = assignedStartField;
        assignedEnd = assignedEndField;
        limitStart = limitStartField;
        limitEnd = limitEndField;
    }
}
