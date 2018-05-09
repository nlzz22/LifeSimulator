using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeScript : MonoBehaviour {

    private InputField currentField;

    private int previousValue = -1;

    // Use this for initialization
    void Start () {
        currentField = GetComponent<InputField>();		
	}

    private bool isWithinAcceptableRange(int num)
    {
        return (num >= -100 && num <= 100);
    }

    public void ValidateField()
    {
        bool hasPassedValidation = true;
        int currValue = -1;
        try
        {
            string thisField = currentField.text;
            currValue = Int32.Parse(thisField);

            if (!isWithinAcceptableRange(currValue))
            {
                hasPassedValidation = false;
            }
        } catch(Exception ) {
            hasPassedValidation = false;
        }

        if (hasPassedValidation)
        {
            previousValue = currValue;
        }
        else
        // fail validation
        {
            // if previous value is not yet set
            if (previousValue == -1)
            {
                currentField.text = "";
            }
            else
            // there is a previous value set
            {
                currentField.text = previousValue.ToString();
            }
        }
    }	
}
