using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AttributeScript
{
    private string attribute;
    private int defaultValue;

    public AttributeScript(string attributeName, int defaultValue)
    {
        this.attribute = attributeName;
        this.defaultValue = defaultValue;
    }

    public string GetAttributeName()
    {
        return attribute;
    }

    public int GetDefaultValue()
    {
        return defaultValue;
    }
}
