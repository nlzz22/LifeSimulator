using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    private AttributeScript[] attributes;

    public void SaveAttributes(AttributeScript[] in_attributes)
    {
        attributes = in_attributes;
    }

    public AttributeScript[] GetAttributes()
    {
        return attributes;
    }
}