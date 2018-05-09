using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    private AttributeScript[] attributes;
    private EventFunctionScript[] eventfuncs;

    public void SaveAttributes(AttributeScript[] in_attributes)
    {
        attributes = in_attributes;
    }

    public void SaveEventFunctions(EventFunctionScript[] in_eventfuncs)
    {
        eventfuncs = in_eventfuncs;
    }

    public AttributeScript[] GetAttributes()
    {
        return attributes;
    }

    public EventFunctionScript[] GetEventFunctions()
    {
        return eventfuncs;
    }
}